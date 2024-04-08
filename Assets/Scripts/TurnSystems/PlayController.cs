using Fish.Utils;
using System;
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
    [SerializeField] private bool _playIsTriggered;
    private HandController _activeHandController; //hand controller that is currently playing
    private DeckController _activeDeckController;

    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _opponent;

    private Coroutine _activateCardsCoroutine;
    
    private void Start()
    {
        _cardsInPlay = new Queue<GameObject>();
        TurnEventsHandler.Instance.DeathEvent.AddListener(OnDeathEventReceived);
    }


    private void OnDestroy()
    {
        TurnEventsHandler.Instance.DeathEvent?.RemoveListener(OnDeathEventReceived);
    }

    /// <summary>
    /// Function triggered by the GameManager, that will reset the boolean of the Play
    /// </summary>
    public void ResetPlay()
    {
        _playIsTriggered = false;
    }

    #region EVENT RECEIVER FUNCTIONS
    /// <summary>
    /// Function called when the death event (triggered by ActorValuesController when an actor dies) is received.
    /// We stop the ongoing play coroutine (if not already done by then), remove all the cards from the play back into the hand AND NOTHING ELSE.
    /// This will avoid the call to the next play event to be made and avoid any bugs. Also prevents remaining cards from being activated and triggering other effects which could result in bugs.
    /// Safety people, safety : )
    /// </summary>
    /// <param name="arg"></param>
    private void OnDeathEventReceived(HOLDER_TYPE arg)
    {
        if (_activateCardsCoroutine != null)
        {
            StopCoroutine(_activateCardsCoroutine);
        }
        while (_cardsInPlay.Count > 0)
        {
            GameObject curCard = _cardsInPlay.Dequeue();
            _activeDeckController.AddCardToBottomOfDeck(curCard.GetComponent<CardPrefabController>().Card);
            Destroy(curCard);
        }
    }
    #endregion

    #region PLAYER AND OPPONENT RELATED FUNCTIONS
    /// <summary>
    /// Function triggered by external scripts handling the encounter/turn system, that will assign the roles of the player (the one playing the cards) and the opponent (the other one).
    /// It will also activate/deactivate colliders of the cards to allow players to select them.
    /// </summary>
    /// <param name="player">GameObject of the one playing the cards right now</param>
    /// <param name="opponent">GameObject of their opponent</param>
    public void AssignPlayerAndOpponent(GameObject player, GameObject opponent)
    {
        Debug.Log("[PLAY CONTROLLER] : New play parameters : player is now "+player.gameObject.name+ " and opponent is now "+opponent.gameObject.name);
        _player = player;
        _opponent = opponent;
        _activeHandController = _player.GetComponent<HandController>();
        _activeDeckController = _activeHandController.GetDeckController();

        if (_activeHandController.GetHolderType() == HOLDER_TYPE.PLAYER) //ONLY IF THE HOLDER IS A PLAYER DO WE ACTIVATE THE CARDS' COLLIDERS (since the opponent, for now, is only a bot)
        {
            _activeHandController.ActivateCardsColliders();
        }
        _opponent.GetComponent<HandController>().DeactivateCardsColliders();
    }

    public GameObject GetPlayer() { return _player; }
    public GameObject GetOpponent() { return _opponent; }
    #endregion

    /// <summary>
    /// Function that will return the next attack card currently in play.
    /// </summary>
    /// <returns></returns>
    public GameObject ObtainNextAttackCardInPlay()
    {
        var cardsTmp = _cardsInPlay.ToArray();
        if(cardsTmp.Length < 1 ) { Debug.Log("[PLAY CONTROLLER] : No attack card found next"); return null; }
        GameObject resultCard = cardsTmp[0];
        int i = 0;
        while (i < cardsTmp.Length)
        {
            if (cardsTmp[i]!=null && cardsTmp[i].GetComponent<CardPrefabController>().Card.GetType() == typeof(AttackCardController))
            {
                Debug.Log("[PLAY CONTROLLER] : Next attack card found : " + cardsTmp[i].GetComponent<CardPrefabController>().Card.CardName);
                return cardsTmp[i];
            }
            i++;
        }
        Debug.Log("[PLAY CONTROLLER] : No attack card found next");
        return null;
    }

    /// <summary>
    /// Function called by external scripts that will launch the coroutine that will activate all the cards.
    /// The coroutine is saved as to ensure that it can be stopped if need be.
    /// </summary>
    public void RequestEndOfPlay()
    {
        _activateCardsCoroutine = StartCoroutine(TriggerEndOfPlay());
    }

    /// <summary>
    /// Function that will trigger the end of play, triggering all the cards' active effect in ORDER THEY WERE PUT DOWN IN THE PLAY
    /// </summary>
    private IEnumerator TriggerEndOfPlay()
    {
        if (_playIsTriggered) { yield return null; } //if the play has already been triggered, we avoid triggering it again immediately - this is necessary to avoid issues when a wild card that triggers the end of play is ALSO the last card played (that would lead to two calls to this function)
        else
        {
            _activeHandController.DeactivateCardsColliders();
            _playIsTriggered=true;
            while(_cardsInPlay.Count > 0)
            {
                GameObject curCard = _cardsInPlay.Dequeue();
                curCard.GetComponent<CardPrefabController>().AnimateActivation();
                yield return new WaitForSeconds(0.70f);
                curCard.GetComponent<CardPrefabController>().Card.ActivateCardEffect(curCard);
                _activeDeckController.AddCardToBottomOfDeck(curCard.GetComponent<CardPrefabController>().Card);
                yield return new WaitForSeconds(0.5f);
            }
            TurnEventsHandler.Instance.PlayEvent.Invoke(new PlayEventArg() { Holder = _player.transform.tag == "Player" ? HOLDER_TYPE.PLAYER : HOLDER_TYPE.OPPONENT, State = PLAY_EVENT_STATE.PLAY_END });
        }
        _activateCardsCoroutine = null;
    }

    /// <summary>
    /// Function that will add a card to the play. Wild cards will not be added, as they should be activated by that point.
    /// </summary>
    /// <param name="card">Card to add to the play</param>
    public void AddCardToPlay(GameObject card, HandController handController)
    {
        //_activeHandController = handController;
        //_activeDeckController = _activeHandController.GetDeckController();
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
