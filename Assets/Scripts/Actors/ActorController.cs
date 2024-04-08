using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Basic controller script for the actors (player and opponent)
/// </summary>
[RequireComponent(typeof(DeckController))]
[RequireComponent(typeof(HandController))]
public class ActorController : MonoBehaviour
{
    [SerializeField] private Transform _vfxSpawnPoint;
    private DeckController _deckController;
    private HandController _handController;
    private void Awake()
    {
        _deckController = GetComponent<DeckController>();
        _handController = GetComponent<HandController>();
        Assert.IsNotNull(_vfxSpawnPoint);
    }

    public DeckController GetDeckController() { return _deckController; }
    public HandController GetHandController() { return _handController; }
    public Transform GetVFXSpawnPoint() { return _vfxSpawnPoint; }
}
