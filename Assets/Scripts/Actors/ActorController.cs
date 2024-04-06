using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Basic controller script for the actors (player and opponent)
/// </summary>
[RequireComponent(typeof(DeckController))]
[RequireComponent(typeof(HandController))]
public class ActorController : MonoBehaviour
{
    private DeckController _deckController;
    private HandController _handController;
    private void Awake()
    {
        _deckController = GetComponent<DeckController>();
        _handController = GetComponent<HandController>();
    }

    public DeckController GetDeckController() { return _deckController; }
    public HandController GetHandController() { return _handController; }
}
