using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class CardElementController : MonoBehaviour
{
    [Header("Card Prefab Elements")]
    [SerializeField] protected Image _cardSymbol;
    [SerializeField] protected Image _cardImage;
    [SerializeField] protected TextMeshProUGUI _cardName;
    [SerializeField] protected TextMeshProUGUI _cardDescription;

    protected void OnValidate()
    {
        Assert.IsNotNull(_cardName);
        Assert.IsNotNull(_cardDescription);
        Assert.IsNotNull(_cardSymbol);
        Assert.IsNotNull(_cardImage);
    }

    protected virtual void DisplayCard(CardController card)
    {
        _cardName.text = card.CardName != "" ? card.CardName : _cardName.text;
        _cardDescription.text = card.CardDescription != "" ? card.CardDescription : _cardDescription.text;
        _cardImage.sprite = card.CardSprite != null ? card.CardSprite : _cardImage.sprite;
        _cardSymbol.sprite = card.CardName != null ? card.CardSymbol : _cardSymbol.sprite;
    }
}
