using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will be the "brain" of the opponent, that will direct its internal logic.
/// Basically, when its play starts, the opponent will just randomly pick their cards for now and play them.
/// </summary>
[RequireComponent(typeof(HandController))]
public class OpponentAIController : MonoBehaviour
{
    [Header("Opponent Prefab Elements")]

    [SerializeField] private OpponentData _data;
    private HandController _handController;

    private void Awake()
    {
        _handController = GetComponent<HandController>();
    }

    private void Start()
    {
        TurnEventsHandler.Instance.PlayEvent.AddListener(OnPlayEventReceived);
    }

    private void OnDestroy()
    {
        TurnEventsHandler.Instance.PlayEvent?.RemoveListener(OnPlayEventReceived);
    }

    #region DATA RELATED FUNCTIONS
    public void SetOpponentData(OpponentData data)
    {
        _data = data;
        AssignOpponentDataElements();
    }
    public OpponentData GetOpponentData() { return _data; }

    /// <summary>
    /// Function that will assign the data elements to the opponent's prefab AND creates its associated deck.
    /// </summary>
    private void AssignOpponentDataElements()
    {
        GetComponent<DeckController>().ConstructDeck(_data.InitCardDeck.Cards);
        GetComponent<ActorValuesController>().SetData(_data.Values);
    }
    #endregion

    #region EVENT RECEIVER FUNCTIONS
    /// <summary>
    /// Function called when receiving the play event.
    /// PLAY START : If the event corresponds to the opponent (non-player), then it will attempt to play 4 random cards.
    /// </summary>
    /// <param name="arg0"></param>
    private void OnPlayEventReceived(PlayEventArg arg)
    {
        if(arg.Holder==_handController.GetHolderType() && arg.State == PLAY_EVENT_STATE.PLAY_BEGIN)
        {
            StartCoroutine(PlayRandomCards(_data.HandSize));
        }
    }
    #endregion

    private IEnumerator PlayRandomCards(int num)
    {
        for(int i = 0; i < num; i++)
        {
            yield return new WaitForSeconds(1f);
            _handController.PlayRandomCard();
        }
    }
}
