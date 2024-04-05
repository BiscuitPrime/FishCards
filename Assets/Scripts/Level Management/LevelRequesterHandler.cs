using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script exists only to transmit to the level loader the requests to load the next level.
/// The reason is to allow these requests to come from ANIMATION calls (that require the component to be present on the gameobject of the animator, so it can't be levelloader itself)
/// </summary>
public class LevelRequesterController : MonoBehaviour
{
    public int NextLevel=0;

    public void RequestNextLevel() => LevelLoaderController.Instance.LoadLevel(NextLevel);
}
