using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script used by cards that are part of pure UI displays, such as the pick a card menu, or card reader.
/// </summary>
public class DisplayCardController : MonoBehaviour
{
    #region VARIABLES
    private CardController _card;
    [Header("Card Elements")]
    [SerializeField] private Image _cardSymbol;
    [SerializeField] private Image _cardImage;
    [SerializeField] private TextMeshProUGUI _cardName;
    [SerializeField] private TextMeshProUGUI _cardDescription;
    #endregion

    /// <summary>
    /// Function called by external scripts (such as UI/GameManager) that will give this DisplayCard the data it needs to display
    /// </summary>
    /// <param name="card"></param>
    public void AttributeCardData(CardController card)
    {
        _card = card;
        DisplayCard();
    }

    /// <summary>
    /// Displays the card's data on the card
    /// </summary>
    private void DisplayCard()
    {
        _cardName.text = _card.CardName != "" ? _card.CardName : _cardName.text;
        _cardDescription.text = _card.CardDescription != "" ? _card.CardDescription : _cardDescription.text;
        _cardImage.sprite = _card.CardSprite != null ? _card.CardSprite : _cardImage.sprite;
        _cardSymbol.sprite = _card.CardName != null ? _card.CardSymbol : _cardSymbol.sprite;
    }

    /// <summary>
    /// Function triggered by the card's button that will launch the start encounter event.
    /// </summary>
    public void OnCardClicked()
    {
        Debug.Log("[DISPLAY CARD CONTROLLER] : Player picked card : " + _card.CardName);
        //Sends a signal or something
        TurnEventsHandler.Instance.EncounterEvent.Invoke(new EncounterEventArg() { State=ENCOUNTER_EVENT_STATE.ENCOUNTER_START});
    }
}
