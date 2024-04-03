using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// This script will handle the PREFAB of the card. Indeed, we will not create all the different card prefabs (because that would make too many of them and subsequent modifications would be too much).
/// As such, the only data the game will store will be the CardController Scriptable objects, while the prefabs will be handled by this script.
/// In effect, this script will only act as a glorified container.
/// </summary>
public class CardPrefabController : MonoBehaviour
{
    #region VARIABLES

    [Header("Card Prefab Elements")]
    [SerializeField] private Image _cardBG;
    [SerializeField] private Image _cardImage;
    [SerializeField] private TextMeshProUGUI _cardName;
    [SerializeField] private TextMeshProUGUI _cardDescription;

    private CardController _card;
    private HandController _hand;
    private Collider2D _cardPrefabCollider;
    #endregion

    private void Awake()
    {
        _cardPrefabCollider = GetComponent<Collider2D>();
    }

    private void OnValidate()
    {
        Assert.IsNotNull(_cardName);
        Assert.IsNotNull(_cardDescription);
        Assert.IsNotNull(_cardBG);
        Assert.IsNotNull(_cardImage);
    }

    /// <summary>
    /// Function called when the card is spawned in the game.
    /// </summary>
    public void SpawnNewCard(CardController card, HandController hand)
    {
        _card = card;
        _hand = hand;
        DisplayCardData();
    }

    /// <summary>
    /// When the prefab is activated, we display the correct data.
    /// Since there will always be a limited number of cards in play, we should do a pool.
    /// </summary>
    public void OnEnable()
    {
        if(_card != null)
        {
            DisplayCardData();
        }
    }

    /// <summary>
    /// Function that will display card data
    /// </summary>
    private void DisplayCardData()
    {
        _cardName.text = _card.CardName != "" ? _card.CardName : _cardName.text;
        _cardDescription.text = _card.CardDescription != "" ? _card.CardDescription : _cardDescription.text;
        _cardImage.sprite = _card.CardSprite != null ? _card.CardSprite : _cardImage.sprite;
        _cardBG.sprite = _card.CardName != null ? _card.CardBGSprite : _cardBG.sprite;
        _cardPrefabCollider.enabled = true;
    }

    /// <summary>
    /// Function called by external scripts that will deactivate the card's collider, usually after being played.
    /// </summary>
    public void DeactivateCardCollider()
    {
        _cardPrefabCollider.enabled = false;
    }

    /// <summary>
    /// When the card is clicked, it will tell the HandController.
    /// ONLY THE HANDCONTROLLER MOVES/DEACTIVATE THE CARD PREFAB. CARDPREFABCONTROLLER HAS NO SAY IN THIS.
    /// </summary>
    private void OnMouseDown()
    {
        Debug.Log("Card "+_card.CardName +"has been selected : player wants to play it");
        _hand.PlayCard(this._card);
    }
}
