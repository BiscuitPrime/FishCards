using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGameManager : MonoBehaviour
{
    [SerializeField] private bool StartGame;

    private void Update()
    {
        if (StartGame)
        {
            StartGame = false;
            GameManager.Instance.StartGame();
        }
    }
}
