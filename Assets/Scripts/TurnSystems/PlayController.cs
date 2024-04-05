using Fish.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will control the play. The play represents all the cards that have been put down.
/// This script will only know which cards have been put (IN ORDER), put them in a correct spot in the game, and UPON RECEIVING A MESSAGE SIGNALING THE END OF THE PLAY,
/// this script will trigger all the cards' effects (at the exception of wild cards, since they will have been activated BEFORE arriving in the play).
/// This script CANNOT know WHEN the play ends as some cards (wild) might force his own end. As such, it will be a dumb script that is triggered either by wild cards or the hand itself.
/// </summary>
public class PlayController : MonoBehaviour
{
    #region SINGLETON DESIGN PATTERN
    public static PlayController Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    #endregion

    [Header("Position Elements")]
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Transform _p1;
    [SerializeField] private Transform _p2;

    private Queue<GameObject> _cardsInPlay;
    private bool _playIsTriggered;
    private HandController _activeHandController; //hand controller that is currently playing
    private DeckController _activeDeckController;

    private GameObject _player;
    private GameObject _opponent;
    
    private void Start()
    {
        _cardsInPlay = new Queue<GameObject>();
    }

    /// <summary>
    /// Function triggered by the GameManager, that will reset the boolean of the Play
    /// </summary>
    public void ResetPlay()
    {
        _playIsTriggered = false;
    }

    #region PLAYER AND OPPONENT RELATED FUNCTIONS
    /// <summary>
    /// Function triggered by external scripts handling the encounter/turn system, that will assign the roles of the player (the one playing the cards) and the opponent (the other one)
    /// </summary>
    /// <param name="player">GameObject of the one playing the cards right now</param>
    /// <param name="opponent">GameObject of their opponent</param>
    public void AssignPlayerAndOpponent(GameObject player, GameObject opponent)
    {
        Debug.Log("New play parameters : player is now "+player.gameObject.name+ " and opponent is now "+opponent.gameObject.name);
        _player = player;
        _opponent = opponent;
    }

    public GameObject GetPlayer() { return _player; }
    public GameObject GetOpponent() { return _opponent; }
    #endregion

    /// <summary>
    /// Function that will trigger the end of play, triggering all the cards' active effect in ORDER THEY WERE PUT DOWN IN THE PLAY
    /// </summary>
    public IEnumerator TriggerEndOfPlay()
    {
        if (_playIsTriggered) { yield return null; } //if the play has already been triggered, we avoid triggering it again immediately - this is necessary to avoid issues when a wild card that triggers the end of play is ALSO the last card played (that would lead to two calls to this function)
        else
        {
            _playIsTriggered=true;
            while(_cardsInPlay.Count > 0)
            {
                GameObject curCard = _cardsInPlay.Dequeue();
                curCard.GetComponent<CardPrefabController>().Card.ActivateCardEffect(curCard);
                _activeDeckController.AddCardToBottomOfDeck(curCard.GetComponent<CardPrefabController>().Card);
                yield return new WaitForSeconds(1f);
            }
            TurnEventsHandler.Instance.PlayEvent.Invoke(new PlayEventArg() { Holder = _player.transform.tag == "Player" ? PLAY_HOLDER_TYPE.PLAYER : PLAY_HOLDER_TYPE.OPPONENT, State = PLAY_EVENT_STATE.PLAY_END });
        }
    }

    /// <summary>
    /// Function that will add a card to the play. Wild cards will not be added, as they should be activated by that point.
    /// </summary>
    /// <param name="card">Card to add to the play</param>
    public void AddCardToPlay(GameObject card, HandController handController)
    {
        _activeHandController = handController;
        _activeDeckController = _activeHandController.GetDeckController();
        if(card.gameObject.GetComponent<CardPrefabController>() != null) //wild cards are not added to the queue, since they were activated BEFORE entering the play. HandController, by this point, should NOT have given them to PlayController
        {
            _cardsInPlay.Enqueue(card);
        }
        UpdateCardsPosition();
    }

    /// <summary>
    /// Function that will update the positions of the cards following a Bezier Curve.
    /// </summary>
    private void UpdateCardsPosition()
    {
        int i = 0;
        if (_cardsInPlay.Count == 1)
        {
            foreach (var card in _cardsInPlay)
            {
                card.transform.position = BezierCurveHandler.GetPointOnBezierCurve(_startPoint.position, _p1.position, _p2.position, _endPoint.position, 0);
            }
        }
        else
        {
            foreach (var card in _cardsInPlay)
            {
                card.transform.position = BezierCurveHandler.GetPointOnBezierCurve(_startPoint.position, _p1.position, _p2.position, _endPoint.position, (1f / (float)(_cardsInPlay.Count - 1)) * (float)i);
                i++;
            }
        }
    }
}
