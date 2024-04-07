using System;
using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private List<BuffObject> _buffs;

    private void Awake()
    {
        _buffs = new List<BuffObject>();
    }

    private void Start()
    {
        TurnEventsHandler.Instance.TurnEvent.AddListener(OnTurnEventReceived);
    }


    private void OnDestroy()
    {
        TurnEventsHandler.Instance.TurnEvent?.RemoveListener(OnTurnEventReceived);
    }

    #region EXTERNAL HANDLING OF BUFFS FUNCTIONS
    /// <summary>
    /// Function called by external scripts (usually buff cards) that will add a buff to the targeted object.
    /// </summary>
    /// <param name="buff">Buff added to the associated gameObject</param>
    public void AddBuff(BuffObject buff)
    {
        _buffs.Add(buff);
    }

    /// <summary>
    /// script that will return all the currently held buffs.
    /// </summary>
    /// <returns>List of the buffs</returns>
    public List<BuffObject> ObtainListOfBuffs()
    {
        return _buffs;
    }

    /// <summary>
    /// Function that will remove all buffs currently applied to the associated object.
    /// </summary>
    public void RemoveAllBuffs()
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
    private void OnTurnEventReceived(TURN_EVENT_STATE arg)
    {
        if(arg == TURN_EVENT_STATE.TURN_END) //we update the buff's turn counter ONLY when the turn ends
        {
            UpdateBuffsTurnCounter();
            UpdateBuffsList();
        }
    }
    #endregion

    #region UPDATE BUFFS LIST FUNCTIONS
    /// <summary>
    /// Updates the turn counter of the contained buffs objects
    /// </summary>
    private void UpdateBuffsTurnCounter()
    {
        foreach(var buff in _buffs)
        {
            buff.UpdateTurnCounter();
        }
    }

    /// <summary>
    /// Updates the current list of buff objects
    /// </summary>
    private void UpdateBuffsList()
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
    #endregion
}
