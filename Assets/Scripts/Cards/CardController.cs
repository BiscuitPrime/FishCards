using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basis for all available cards
/// </summary>
public abstract class CardController : ScriptableObject
{
    #region VARIABLES
    [field: Header("Card Visual Elements")]
    [field: SerializeField, TextArea(1, 10)] public string CardName { get; private set; }
    [field: SerializeField] public Sprite CardSprite { get; private set; }
    [field:SerializeField] public Sprite CardBGSprite { get; private set; }
    [field:SerializeField, TextArea(3, 10)] public string CardDescription { get; private set; }
    #endregion

    #region CARD LOGIC
    /// <summary>
    /// Function called by other game elements that will trigger the effect of the card WHEN it is activated (not when played OR drawn).
    /// Since the Card Controller is a script obj, we pass in parameter the associated prefab to apply on it the various visual effects.
    /// </summary>
    public abstract void ActivateCardEffect(GameObject cardPrefab);

    #endregion
}
