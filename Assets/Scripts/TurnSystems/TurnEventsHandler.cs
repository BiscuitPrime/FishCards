using System.Collections;
using System.Collections.Generic;
using System.Xml.Xsl;
using UnityEngine;
using UnityEngine.Events;

public enum TURN_EVENT_STATE
{
    TURN_START,
    TURN_END
}

public enum PLAY_EVENT_STATE
{
    PLAY_BEGIN,
    PLAY_END
}

public enum HOLDER_TYPE
{
    PLAYER,
    OPPONENT
}

public enum ENCOUNTER_EVENT_STATE
{
    ENCOUNTER_START,
    ENCOUNTER_END
}

public struct PlayEventArg
{
    public HOLDER_TYPE Holder;
    public PLAY_EVENT_STATE State;
}

public struct EncounterEventArg
{
    public ENCOUNTER_EVENT_STATE State;
}

public class TurnEvent : UnityEvent<TURN_EVENT_STATE>
{

}

public class PlayEvent : UnityEvent<PlayEventArg> 
{

}

public class DeathEvent : UnityEvent<HOLDER_TYPE>
{

}

public class EncounterEvent : UnityEvent<EncounterEventArg>
{

}

/// <summary>
/// Script that will act as the event spawner and handler for Turn and Play type events.
/// </summary>
public class TurnEventsHandler : MonoBehaviour
{
    #region SINGLETON DESIGN PATTERN
    public static TurnEventsHandler Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        //DontDestroyOnLoad(this);
        TurnEvent = new TurnEvent();
        PlayEvent = new PlayEvent();
        DeathEvent = new DeathEvent();
        EncounterEvent = new EncounterEvent();
    }
    #endregion

    public EncounterEvent EncounterEvent;
    public TurnEvent TurnEvent;
    public PlayEvent PlayEvent;
    public DeathEvent DeathEvent;
}
