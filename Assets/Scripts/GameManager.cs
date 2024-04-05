using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Script that will control the entire game, but especially the turn and encounter systems.
/// </summary>
[RequireComponent(typeof(TurnEventsHandler))]
public class GameManager : MonoBehaviour
{
    #region SINGLETON DESIGN PATTERN
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(this);
    }
    #endregion

    #region VARIABLES
    [Header("Player")]
    private GameObject _playerPrefab;
    private ActorController _playerController;

    [Header("Opponent")]
    private GameObject _opponentPrefab;
    private ActorController _opponentController;

    [Header("Play Variables")]
    private ActorController _curActivePlayer;
    #endregion

    private void Start()
    {
        _playerPrefab = GameObject.FindGameObjectWithTag("Player");
        _playerController = _playerPrefab.GetComponent<ActorController>();
        _opponentPrefab = GameObject.FindGameObjectWithTag("Opponent");
        _opponentController = _opponentPrefab.GetComponent<ActorController>();
        Assert.IsNotNull(_playerController);
        Assert.IsNotNull(_opponentController);

        TurnEventsHandler.Instance.PlayEvent.AddListener(OnPlayEventReceived);
        TurnEventsHandler.Instance.TurnEvent.AddListener(OnTurnEventReceived);
    }

    private void OnDestroy()
    {
        TurnEventsHandler.Instance.PlayEvent?.RemoveListener(OnPlayEventReceived);
        TurnEventsHandler.Instance.TurnEvent?.RemoveListener(OnTurnEventReceived);
    }

    /// <summary>
    /// Function that will start the game, and the turns logic, at the beginning of the game.
    /// It will start the initial turn, and like all turns, will assign the first play holder to be the player.
    /// </summary>
    public void StartGame()
    {
        TriggerStartEncounter();
    }


    /// <summary>
    /// Called when the event TurnEvent is received.
    /// END : should have been sent EITHER by this script OR by the death of an actor.
    /// </summary>
    public void OnTurnEventReceived(TURN_EVENT_STATE state)
    {
        if(state == TURN_EVENT_STATE.TURN_END)
        {
            Debug.Log("[GAME MANAGER] : TURN HAS ENDED");
            _curActivePlayer = null;
            //trigger the display of the pick a card => it will be this component, upon its resolution, that triggers the new turn to start.
        }
        else //if the turn is starting, we trigger its start
        {
            Debug.Log("[GAME MANAGER] : TURN BEGINS");
            TriggerStartTurn();
        }
    }

    /// <summary>
    /// Function triggered upon receiving the Play event.
    /// END : will trigger the end of the turn IF it was the opponent's play
    /// </summary>
    /// <param name="arg"></param>
    public void OnPlayEventReceived(PlayEventArg arg)
    {
        Debug.Log("[GAME MANAGER] : Play Event received with values : Holder : "+arg.Holder.ToString()+" and State : "+arg.State.ToString());
        if(arg.State==PLAY_EVENT_STATE.PLAY_END)
        {
            if(arg.Holder==PLAY_HOLDER_TYPE.OPPONENT && _curActivePlayer==_opponentController) //if the play that ended was the opponent's, then the turn is over. We also test if the opponent is the last active, as IF there as been a death, the event has already been received and updated the curactive
            {
                TurnEventsHandler.Instance.TurnEvent.Invoke(TURN_EVENT_STATE.TURN_END);
            }
            else
            {
                _curActivePlayer = _opponentController;
                TriggerStartPlay();
            }
        }
    }

    /// <summary>
    /// Function that will trigger the start of an encounter.
    /// An encounter against an opponent is defined by a series of turns until one of the actors die.
    /// At the start of an encounter, the player regens their DEF, all their hand cards are added to the bottom of their decks and the decks are then shuffled.
    /// </summary>
    private void TriggerStartEncounter()
    {
        TurnEventsHandler.Instance.TurnEvent.Invoke(TURN_EVENT_STATE.TURN_START);
    }

    /// <summary>
    /// Function that will trigger the start of a turn.
    /// Each turn is composed of two plays : the player's play, then the opponent's play.
    /// If during said plays there is no trigger to end the turn, then we begin a new turn (this function)
    /// </summary>
    public void TriggerStartTurn()
    {
        _curActivePlayer = _playerController;
        TriggerStartPlay();
    }


    private void TriggerStartPlay()
    {
        PlayController.Instance.ResetPlay();
        PlayController.Instance.AssignPlayerAndOpponent(_curActivePlayer.gameObject,_curActivePlayer==_opponentController?_playerController.gameObject:_opponentController.gameObject);
    }

    /// <summary>
    /// Function called at the beginning of a turn, sends an event to all actors to draw their cards.
    /// </summary>
    private void TriggerActorsDraw()
    {
        
    }
}
