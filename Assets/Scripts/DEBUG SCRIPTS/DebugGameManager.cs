using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGameManager : MonoBehaviour
{
    [SerializeField] private bool StartGame;
    [SerializeField] private bool TriggerNewTurn;

    private void Update()
    {
        if (StartGame)
        {
            StartGame = false;
            GameManager.Instance.StartGame();
        }
        if (TriggerNewTurn)
        {
            TriggerNewTurn = false;
            TurnEventsHandler.Instance.TurnEvent.Invoke(TURN_EVENT_STATE.TURN_START);
        }
    }
}
