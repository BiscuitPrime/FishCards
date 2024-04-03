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

    [Header("Cards elements")]
    [SerializeField] private int _handCardNumbers;
    //[SerializeField] private List<GameObject> _handCards; //cards within the hand : can be null
    private Dictionary<CardController, GameObject> _handCardsDict;
    #endregion

    private void Awake()
    {
        _deck = GetComponent<DeckController>();
        //_handCards = new List<GameObject>();
        _handCardsDict = new Dictionary<CardController, GameObject>();
    }

    /// <summary>
    /// Will attempt to draw cards from the deck
    /// </summary>
    public void DrawCards()
    {
        int cardsToDraw = _handCardNumbers - _handCardsDict.Count;
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
    }

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
                _handCardsDict.Remove(cardController);
                return;
            }
        }
    }
}
