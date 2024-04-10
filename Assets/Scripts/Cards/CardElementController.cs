using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <summary>
/// Script that will contain references towards all elements present in a prefab or display card
/// </summary>
public class CardElementController : MonoBehaviour
{
    [Header("Card Prefab Elements")]
    [SerializeField] protected Image _cardSymbol;
    [SerializeField] protected Image _cardImage;
    [SerializeField] protected TextMeshProUGUI _cardName;
    [SerializeField] protected TextMeshProUGUI _cardDescription;
    [SerializeField] protected GameObject _cardBackBG;

    protected void OnValidate()
    {
        Assert.IsNotNull(_cardName);
        Assert.IsNotNull(_cardDescription);
        Assert.IsNotNull(_cardSymbol);
        Assert.IsNotNull(_cardImage);
        Assert.IsNotNull(_cardBackBG);
    }

    protected virtual void DisplayCard(CardController card)
    {
        _cardName.text = card.CardName != "" ? card.CardName : _cardName.text;
        _cardDescription.text = card.CardDescription != "" ? card.CardDescription : _cardDescription.text;
        _cardImage.sprite = card.CardSprite != null ? card.CardSprite : _cardImage.sprite;
        _cardSymbol.sprite = card.CardName != null ? card.CardSymbol : _cardSymbol.sprite;
    }

    /// <summary>
    /// Function that will make the card show only the back of the card
    /// </summary>
    public virtual void ShowBackCard()
    {
        _cardBackBG.SetActive(true);
    }
    /// <summary>
    /// Function that will make the card show only the front of the card
    /// </summary>
    public virtual void ShowFrontCard()
    {
        _cardBackBG.SetActive(false);
    }
}
