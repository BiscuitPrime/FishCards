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
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(AudioSource))]
public class CardPrefabController : CardElementController
{
    #region VARIABLES
    public CardController Card;

    [SerializeField] private Animator _anim;

    private HandController _hand;
    private Collider2D _cardPrefabCollider;
    private AudioSource _audioSource;
    #endregion

    private void Awake()
    {
        _anim = GetComponent<Animator>();
        _cardPrefabCollider = GetComponent<Collider2D>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        UIController.Instance.DisableCardReader();    
    }


    /// <summary>
    /// Function called when the card is spawned in the game.
    /// </summary>
    public void SpawnNewCard(CardController card, HandController hand)
    {
        Card = card;
        _hand = hand;
        DisplayCard(Card);
    }

    /// <summary>
    /// When the prefab is activated, we display the correct data.
    /// Since there will always be a limited number of cards in play, we should do a pool.
    /// </summary>
    public void OnEnable()
    {
        if(Card != null)
        {
            DisplayCard(Card);
        }
    }

    #region COLLIDER FUNCTIONS
    /// <summary>
    /// Function called by external scripts that will deactivate the card's collider, usually after being played.
    /// </summary>
    public void DeactivateCardCollider()
    {
        _cardPrefabCollider.enabled = false;
    }

    /// <summary>
    /// Function called by external scripts that will activate the card's collider, usually after being played.
    /// </summary>
    public void ActivateCardCollider()
    {
        _cardPrefabCollider.enabled = true;
    }

    /// <summary>
    /// When the card is clicked, it will tell the HandController.
    /// ONLY THE HANDCONTROLLER MOVES/DEACTIVATE THE CARD PREFAB. CARDPREFABCONTROLLER HAS NO SAY IN THIS.
    /// </summary>
    private void OnMouseDown()
    {
        Debug.Log("Card "+Card.CardName +" has been selected : player wants to play it");
        _audioSource.Play();
        AnimatePlay();
        DeactivateCardCollider();
        UIController.Instance.DisableCardReader();
        _hand.PlayCard(this.Card);
    }

    /// <summary>
    /// When the player starts to hover on the card, we enable the card reader.
    /// </summary>
    private void OnMouseOver()
    {
        UIController.Instance.EnableCardReader(this.Card);
    }

    /// <summary>
    /// When the player stops hovering on the card, we disable the card reader.
    /// </summary>
    private void OnMouseExit()
    {
        UIController.Instance.DisableCardReader();   
    }
    #endregion

    #region ANIMATIONS
    public void AnimateActivation()
    {
        _anim.Play("Activation");
    }
    public void AnimatePlay()
    {
        _anim.Play("Play");
    }
    #endregion
}
