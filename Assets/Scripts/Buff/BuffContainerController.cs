using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Script that defines a buff container.
/// The buff container will be created and added as a component to any gameobject when a buff is applied to said gameObject, and will contain a list of all the active buffs to said gameobject.
/// It will also keep track of the turns, and update the turn counters of the buffs accordingly.
/// IF a buff's counter reaches 0 => buff is removed and the associated script. obj. is deleted (Note that the buff script. obj. are CREATED (and not instantiated) by the buff cards)
/// It is also the buff cards that will ADD this component (if not already present) to the targeted card/actor
/// </summary>
public class BuffContainer : MonoBehaviour
{
    [Header("Buff list : ")]
    [SerializeField] protected List<BuffObject> _buffs;

    protected virtual void Awake()
    {
        _buffs = new List<BuffObject>();
    }

    protected virtual void Start()
    {
        TurnEventsHandler.Instance.TurnEvent.AddListener(OnTurnEventReceived);
        TurnEventsHandler.Instance.EncounterEvent.AddListener(OnEncounterEventReceived);
    }

    protected virtual void OnDestroy()
    {
        TurnEventsHandler.Instance.TurnEvent?.RemoveListener(OnTurnEventReceived);
        TurnEventsHandler.Instance.EncounterEvent?.RemoveListener(OnEncounterEventReceived);
    }

    #region EXTERNAL HANDLING OF BUFFS FUNCTIONS
    /// <summary>
    /// Function called by external scripts (usually buff cards) that will add a buff to the targeted object.
    /// </summary>
    /// <param name="buff">Buff added to the associated gameObject</param>
    public virtual void AddBuff(BuffObject buff)
    {
        _buffs.Add(buff);
    }

    /// <summary>
    /// script that will return all the currently held buffs.
    /// </summary>
    /// <returns>List of the buffs</returns>
    public virtual List<BuffObject> ObtainListOfBuffs()
    {
        return _buffs;
    }

    /// <summary>
    /// Function that will remove all buffs currently applied to the associated object.
    /// </summary>
    public virtual void RemoveAllBuffs()
    {
        _buffs.Clear();
    }
    #endregion

    #region EVENT RECEIVER FUNCTIONS
    /// <summary>
    /// Called when the turn event is received.
    /// TURN END : will update the buffs turn counter.
    /// </summary>
    /// <param name="arg">TURN_EVENT_STATE</param>
    protected virtual void OnTurnEventReceived(TURN_EVENT_STATE arg)
    {
        if(arg == TURN_EVENT_STATE.TURN_END) //we update the buff's turn counter ONLY when the turn ends
        {
            UpdateBuffsTurnCounter();
            UpdateBuffsList();
        }
    }

    /// <summary>
    /// When an encounter finishes or starts, all buffs are cleared
    /// </summary>
    /// <param name="arg0"></param>
    protected virtual void OnEncounterEventReceived(EncounterEventArg arg0)
    {
        RemoveAllBuffs();
    }
    #endregion

    #region UPDATE BUFFS LIST FUNCTIONS
    /// <summary>
    /// Updates the turn counter of the contained buffs objects
    /// </summary>
    protected virtual void UpdateBuffsTurnCounter()
    {
        foreach(var buff in _buffs)
        {
            buff.UpdateTurnCounter();
        }
    }

    /// <summary>
    /// Updates the current list of buff objects
    /// </summary>
    protected virtual void UpdateBuffsList()
    {
        List<BuffObject> tmp = new List<BuffObject>();
        foreach (var buff in _buffs)
        {
            if (buff.TurnCounter >= 1)
            {
                tmp.Add(buff);
            }
        }
        _buffs.Clear();
        _buffs.AddRange(tmp);
    }

    /// <summary>
    /// Function that will replace the buff list with another buff list
    /// </summary>
    /// <param name="buffs">List of buffs</param>
    public virtual void SetBuffsList(List<BuffObject> buffs)
    {
        _buffs.Clear();
        _buffs.AddRange(buffs);
    }
    #endregion
}
