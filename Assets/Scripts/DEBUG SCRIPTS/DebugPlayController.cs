using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayController))]
public class DebugPlayController : MonoBehaviour
{
    [SerializeField] public bool SetUpPlay;
    [SerializeField] private bool TriggerEndOfPlay;

    private void Update()
    {
        if (SetUpPlay)
        {
            PlayController.Instance.ResetPlay();
            PlayController.Instance.AssignPlayerAndOpponent(GameObject.FindGameObjectWithTag("Player"),GameObject.FindGameObjectWithTag("Opponent"));
            SetUpPlay = false;
        }
        if (TriggerEndOfPlay)
        {
            StartCoroutine(PlayController.Instance.TriggerEndOfPlay());
            TriggerEndOfPlay = false;
        }
    }
}
