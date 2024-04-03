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
    
    private void Start()
    {
        _cardsInPlay = new Queue<GameObject>();
    }

    /// <summary>
    /// Function that will trigger the end of play, triggering all the cards' active effect in ORDER THEY WERE PUT DOWN IN THE PLAY
    /// </summary>
    public void TriggerEndOfPlay()
    {
        while(_cardsInPlay.Count > 0)
        {
            GameObject curCard = _cardsInPlay.Dequeue();
            curCard.GetComponent<CardPrefabController>().Card.ActivateCardEffect(curCard);
        }
    }

    /// <summary>
    /// Function that will add a card to the play. Wild cards will not be added, as they should be activated by that point.
    /// </summary>
    /// <param name="card">Card to add to the play</param>
    public void AddCardToPlay(GameObject card)
    {
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
        if (_cardsInPlay.Count <= 1)
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
