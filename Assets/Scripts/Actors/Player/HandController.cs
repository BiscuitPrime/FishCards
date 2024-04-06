using Fish.Utils;
using System;
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
    [Header("Holder Elements")]
    [SerializeField] private PLAY_HOLDER_TYPE _holderType=PLAY_HOLDER_TYPE.OPPONENT;

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

    private void Start()
    {
        TurnEventsHandler.Instance.EncounterEvent.AddListener(OnEncounterEventReceived);
        TurnEventsHandler.Instance.TurnEvent.AddListener(OnTurnEventReceived);
        //TurnEventsHandler.Instance.PlayEvent.AddListener(OnPlayEventReceived);
    }

    private void OnDestroy()
    {
        TurnEventsHandler.Instance.EncounterEvent?.RemoveListener(OnEncounterEventReceived);
        TurnEventsHandler.Instance.TurnEvent?.RemoveListener(OnTurnEventReceived);
        //TurnEventsHandler.Instance.PlayEvent?.RemoveListener(OnPlayEventReceived);
    }

    public DeckController GetDeckController()
    {
        return _deck;
    }
    public PLAY_HOLDER_TYPE GetHolderType()
    {
        return _holderType;
    }

    #region EVENT RECEIVER FUNCTIONS
    /// <summary>
    /// Function called when the encounter event is received. 
    /// When this event signals the END of an encounter, the hand will give back all their currently held cards to the deck.
    /// </summary>
    /// <param name="arg">Event args</param>
    /// <exception cref="NotImplementedException"></exception>
    private void OnEncounterEventReceived(EncounterEventArg arg)
    {
        if (arg.State == ENCOUNTER_EVENT_STATE.ENCOUNTER_END)
        {
            Debug.Log("[HAND CONTROLLER] : Encounter End Event received : pushing all the hand to the deck.");
            foreach(var card in _handCardsDict.Keys)
            {
                _deck.AddCardToBottomOfDeck(card);
                GameObject removedCard = _handCardsDict[card];
                Destroy(removedCard);
            }
            _handCardsDict.Clear();
        }
    }

    /// <summary>
    /// Function called when the turn event is received.
    /// TURN START : we draw cards. 
    /// </summary>
    /// <param name="arg0"></param>
    private void OnTurnEventReceived(TURN_EVENT_STATE arg0)
    {
        if (arg0 == TURN_EVENT_STATE.TURN_START)
        {
            DrawCards();
        }
    }

    ///// <summary>
    ///// Function called when the play event is received.
    ///// PLAY START : IF this._holder != arg.Holder, then its the play of the other actor. As such, we deactivate all the cards' colliders in the hand
    ///// </summary>
    ///// <param name="arg"></param>
    //private void OnPlayEventReceived(PlayEventArg arg)
    //{
    //    if (arg.State == PLAY_EVENT_STATE.PLAY_BEGIN)
    //    {
    //        if(_holder != arg.Holder)
    //        {
    //            DeactivateCardsColliders();
    //        }
    //        else
    //        {
    //            ActivateCardsColliders();
    //        }
    //    }
    //}
    #endregion

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
            Debug.Log("[HandController] : Adding card : " + drawnCards[i].name + " to hand");
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
    /// <param name="numberOfCards">Number of cards to add to the hand.</param>
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
                    StartCoroutine(PlayController.Instance.TriggerEndOfPlay());
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

    /// <summary>
    /// Function that will deactivate the collider of the cards present in the hand
    /// </summary>
    public void DeactivateCardsColliders()
    {
        Debug.Log("[HAND CONTROLLER] : Deactivating cards for : "+gameObject.name);
        foreach (var card in _handCardsDict.Keys)
        {
            _handCardsDict[card].GetComponent<CardPrefabController>().DeactivateCardCollider();
        }
    }
    /// <summary>
    /// Function that will deactivate the collider of the cards present in the hand
    /// </summary>
    public void ActivateCardsColliders()
    {
        Debug.Log("[HAND CONTROLLER] : Activating cards for : " + gameObject.name);
        foreach (var card in _handCardsDict.Keys)
        {
            _handCardsDict[card].GetComponent<CardPrefabController>().ActivateCardCollider();
        }
    }
}
