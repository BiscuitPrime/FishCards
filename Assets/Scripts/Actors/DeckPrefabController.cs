using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script used by the PREFAB of the deck used by the player.
/// Only serves to trigger the FIRST encounter upon being clicked on.
/// TODO : NO LONGER SERVES ANY PURPOSES => SHOULD BE REMOVED
/// </summary>
[RequireComponent(typeof(Collider2D))]
public class DeckPrefabController : MonoBehaviour
{
    private Collider2D _collider;
    private void Awake()
    {
        _collider = GetComponent<Collider2D>();
        _collider.enabled = true;
    }

    private void OnMouseDown()
    {
        _collider.enabled = false;
       // GameManager.Instance.StartGame();
    }
}
