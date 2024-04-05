using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugGameManager : MonoBehaviour
{
    [SerializeField] private bool StartGame;
    [SerializeField] private bool TriggerNewTurn;
    [SerializeField] private bool TriggerEndEncounter;

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
        if(TriggerEndEncounter)
        {
            TriggerEndEncounter = false;
            TurnEventsHandler.Instance.EncounterEvent.Invoke(new EncounterEventArg() { State=ENCOUNTER_EVENT_STATE.ENCOUNTER_END });
        }
    }
}
