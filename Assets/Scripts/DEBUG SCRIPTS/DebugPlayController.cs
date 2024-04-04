using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayController))]
public class DebugPlayController : MonoBehaviour
{
    [SerializeField] public bool SetUpPlay;

    private void Update()
    {
        if (SetUpPlay)
        {
            PlayController.Instance.ResetPlay();
            PlayController.Instance.AssignPlayerAndOpponent(GameObject.FindGameObjectWithTag("Player"),GameObject.FindGameObjectWithTag("Opponent"));
            SetUpPlay = false;
        }
    }
}
