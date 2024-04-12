using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that will be used to read player's input during the game in runtime.
/// </summary>
public class InputManager : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            UIController.Instance.RequestReturnToMainMenuScreen();
        }
    }
}
