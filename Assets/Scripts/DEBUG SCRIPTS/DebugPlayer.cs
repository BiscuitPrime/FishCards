using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DeckController))]
[RequireComponent(typeof(HandController))]
public class DebugPlayer : MonoBehaviour
{
    private HandController _handController;
    private DeckController _deckController;

    [SerializeField] private bool CreateDeck;
    [SerializeField] private bool CreateHand;
    [SerializeField] private bool AddCardToHand;
    [SerializeField] private bool TriggerEndOfPlay;

    [SerializeField] private List<CardController> _availableCards;

    private void Awake()
    {
        _handController = GetComponent<HandController>();
        _deckController = GetComponent<DeckController>();
    }
    private void Update()
    {
        if (CreateDeck)
        {
            _deckController.ConstructDeck(_availableCards);
            CreateDeck = false;
        }
        if(CreateHand)
        {
            _handController.DrawCards();
            CreateHand = false;
        }
        if(AddCardToHand)
        {
            _handController.AddNewCard();
            AddCardToHand = false;
        }
        if(TriggerEndOfPlay)
        {
            PlayController.Instance.TriggerEndOfPlay();
            TriggerEndOfPlay = false;
        }
    }
}
