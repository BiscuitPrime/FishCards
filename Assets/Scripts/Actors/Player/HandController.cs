using Fish.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will control the HAND of the player/opponent
/// The hand is the ensemble of cards a player has (in-hand) during a turn, and that they can play during their play.
/// The hand will be drawn from the Deck, in the order where they are following a FILO logic
/// </summary>
[RequireComponent(typeof(DeckController))]
public class HandController : MonoBehaviour
{
    #region VARIABLES
    [Header("Deck")]
    [SerializeField] private DeckController _deck;

    [Header("Hand Elements")]
    [SerializeField] private Transform _startPoint;
    [SerializeField] private Transform _endPoint;
    [SerializeField] private Transform _p1;
    [SerializeField] private Transform _p2;

    [Header("Cards elements")]
    [SerializeField] private int _baseNumberOfCardsInHand;
    //[SerializeField] private List<GameObject> _handCards; //cards within the hand : can be null
    private Dictionary<CardController, GameObject> _handCardsDict;
    #endregion

    private void Awake()
    {
        _deck = GetComponent<DeckController>();
        //_handCards = new List<GameObject>();
        _handCardsDict = new Dictionary<CardController, GameObject>();
    }

    public DeckController GetDeckController()
    {
        return _deck;
    }

    #region ADDING NEW CARDS FUNCTIONS
    /// <summary>
    /// Will attempt to draw cards from the deck and add them to the hand.
    /// </summary>
    public void DrawCards()
    {
        int cardsToDraw = _baseNumberOfCardsInHand - _handCardsDict.Count;
        Debug.Log("[HandController] : Attempting to draw " + cardsToDraw + " cards due to HandCardNumbers : "+_baseNumberOfCardsInHand+" and Dict size : "+_handCardsDict.Count);
        CardController[] drawnCards = _deck.DrawCards(cardsToDraw);
        //TODO : CHANGE THIS TO A POOL SYSTEM RATHER THAN AN INSTANTIATE
        for(int i=0; i < drawnCards.Length; i++)
        {
            Debug.Log("Adding card : " + drawnCards[i].name + " to hand");
            GameObject cardPrefab = Resources.Load("Card") as GameObject;
            GameObject newCard = Instantiate(cardPrefab);
            newCard.GetComponent<CardPrefabController>().SpawnNewCard(drawnCards[i], this);
            _handCardsDict.Add(drawnCards[i], newCard);
        }
        UpdateCardsPosition();
    }

    /// <summary>
    /// Will attempt to draw cards from the deck and add them to the hand.
    /// </summary>
    public void DrawCards(int numberOfCards)
    {
        Debug.Log("[HandController] : Attempting to draw " + numberOfCards + " cards due to function requirements");
        CardController[] drawnCards = _deck.DrawCards(numberOfCards);
        //TODO : CHANGE THIS TO A POOL SYSTEM RATHER THAN AN INSTANTIATE
        for (int i = 0; i < drawnCards.Length; i++)
        {
            Debug.Log("Adding card : " + drawnCards[i].name + " to hand");
            GameObject cardPrefab = Resources.Load("Card") as GameObject;
            GameObject newCard = Instantiate(cardPrefab);
            newCard.GetComponent<CardPrefabController>().SpawnNewCard(drawnCards[i], this);
            _handCardsDict.Add(drawnCards[i], newCard);
        }
        UpdateCardsPosition();
    }

    /// <summary>
    /// Function that will add a new card to the hand, regardless if the player drew cards or not.
    /// </summary>
    public void AddNewCards()
    {
        DrawCards();
    }

    /// <summary>
    /// Function that will add new cards to the hand, with inputted number of cards.
    /// </summary>
    /// <param name="numberOfCards">Number of cards to add to the hand.</param>
    public void AddNewCards(int numberOfCards)
    {
        DrawCards(numberOfCards);
    }
    #endregion

    /// <summary>
    /// Function that will play the inputted card from within the hand. Will throw an error if the card is not within the hand.
    /// </summary>
    /// <param name="card"></param>
    public void PlayCard(CardController card)
    {
        if (!_handCardsDict.ContainsKey(card))
        {
            Debug.LogError("Card " + card.name + " not present in the hand");
            return;
        }
        RemoveCardFromHand(card);
    }

    /// <summary>
    /// Function that will remove a card from a hand
    /// </summary>
    /// <param name="card"></param>
    private void RemoveCardFromHand(CardController card)
    {
        foreach(CardController cardController in _handCardsDict.Keys)
        {
            if(cardController == card)
            {
                if(cardController is WildCardController) //wild cards are triggered IMMEDIATELY upon being selected, and do not enter the Play logic
                {
                    cardController.ActivateCardEffect(_handCardsDict[cardController]);
                    _handCardsDict.Remove(cardController);
                }
                else
                {
                    GameObject removedCard = _handCardsDict[cardController];
                    _handCardsDict.Remove(cardController);
                    PlayController.Instance.AddCardToPlay(removedCard, this);
                }
                if (_handCardsDict.Count == 0)
                {
                    PlayController.Instance.TriggerEndOfPlay();
                }
                UpdateCardsPosition();
                return;
            }
        }
    }

    /// <summary>
    /// Function that will update the position of the cards in the hand.
    /// New cards are added at the right of previously added card, with an update of all the cards positions.
    /// </summary>
    private void UpdateCardsPosition()
    {
        CardController[] keys = new CardController[_handCardsDict.Count];
        int i = 0;
        foreach(var key in _handCardsDict.Keys)
        {
            keys[i] = key;
            i++;
        }
        if(keys.Length ==1) 
        {
            _handCardsDict[keys[0]].transform.position = BezierCurveHandler.GetPointOnBezierCurve(_startPoint.position, _p1.position, _p2.position, _endPoint.position, 0);
        }
        else
        {
            for (int j = 0; j < _handCardsDict.Count; j++)
            {
                _handCardsDict[keys[j]].transform.position = BezierCurveHandler.GetPointOnBezierCurve(_startPoint.position, _p1.position, _p2.position, _endPoint.position, (1f / (float)(_handCardsDict.Count - 1)) * (float)j);
            }
        }
    }
}
