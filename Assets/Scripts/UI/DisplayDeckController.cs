using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script used by the deck display at the start of the game, to show the Player all their current cards in their deck.
/// </summary>
public class DisplayDeckController : CardElementController
{
    #region VARIABLES
    private List<CardController> _cardsList;
    private CardsListData _playerInitCardList;
    private CardController _curDisplayedCard;
    #endregion

    private void OnEnable()
    {
        _playerInitCardList = GameManager.Instance.GetPlayerCards();
        _cardsList = new List<CardController>();
        foreach(CardController card in _playerInitCardList.Cards)
        {
            _cardsList.Add(card);
        }
        _curDisplayedCard = _cardsList[0];
        DisplayCard(_curDisplayedCard);
    }

    public void OnCardClicked()
    {
        _cardsList.Remove(_curDisplayedCard);
        if(_cardsList.Count < 1)
        {
            _curDisplayedCard = null;
            UIController.Instance.HideDisplayDeck();
            GameManager.Instance.StartGame();
            return;
        }
        else
        {
            _curDisplayedCard = _cardsList[0];
            DisplayCard(_curDisplayedCard);
        }
    }

    ///// <summary>
    ///// Function that will display the cards
    ///// TODO : Regroup it (and all other DisplayCard methods) in an interface
    ///// </summary>
    //private void DisplayCard()
    //{
    //    _cardName.text = _curDisplayedCard.CardName != "" ? _curDisplayedCard.CardName : _cardName.text;
    //    _cardDescription.text = _curDisplayedCard.CardDescription != "" ? _curDisplayedCard.CardDescription : _cardDescription.text;
    //    _cardImage.sprite = _curDisplayedCard.CardSprite != null ? _curDisplayedCard.CardSprite : _cardImage.sprite;
    //    _cardSymbol.sprite = _curDisplayedCard.CardName != null ? _curDisplayedCard.CardSymbol : _cardSymbol.sprite;
    //}
}
