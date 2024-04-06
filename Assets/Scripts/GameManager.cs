using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Script that will control the entire game, but especially the turn and encounter systems.
/// </summary>
[RequireComponent(typeof(TurnEventsHandler))]
[RequireComponent(typeof(OpponentSelectorController))]
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
    [SerializeField] private CardsListData _playerInitCardData;
    [SerializeField] private CardsListData _playerAvailableCardData;

    [Header("Opponent")]
    private GameObject _opponentPrefab;
    private ActorController _opponentController;

    [Header("Play Variables")]
    [SerializeField] private ActorController _curActivePlayer;

    [Header("Game Elements")]
    [SerializeField] private int _encounterCount=0;

    private OpponentSelectorController _opponentSelectorController;

    private bool _playerIsReady=false;
    private bool _opponentIsReady=false;
    #endregion

    private void OnValidate()
    {
        Assert.IsNotNull(_playerAvailableCardData);
        Assert.IsNotNull(_playerInitCardData);
    }

    private void Start()
    {
        _opponentSelectorController = GetComponent<OpponentSelectorController>();
        _playerPrefab = GameObject.FindGameObjectWithTag("Player");
        _playerController = _playerPrefab.GetComponent<ActorController>();
        _opponentPrefab = GameObject.FindGameObjectWithTag("Opponent");
        _opponentController = _opponentPrefab.GetComponent<ActorController>();
        Assert.IsNotNull(_playerController);
        Assert.IsNotNull(_opponentController);

        TurnEventsHandler.Instance.PlayEvent.AddListener(OnPlayEventReceived);
        TurnEventsHandler.Instance.TurnEvent.AddListener(OnTurnEventReceived);
        TurnEventsHandler.Instance.EncounterEvent.AddListener(OnEncounterEventReceived);
    }

    private void OnDestroy()
    {
        TurnEventsHandler.Instance.PlayEvent?.RemoveListener(OnPlayEventReceived);
        TurnEventsHandler.Instance.TurnEvent?.RemoveListener(OnTurnEventReceived);
        TurnEventsHandler.Instance.EncounterEvent?.RemoveListener(OnEncounterEventReceived);
    }

    /// <summary>
    /// Function that will start the game, and the turns logic, at the beginning of the game.
    /// It will start the initial turn, and like all turns, will assign the first play holder to be the player.
    /// </summary>
    public void StartGame()
    {
        TurnEventsHandler.Instance.EncounterEvent.Invoke(new EncounterEventArg() { State = ENCOUNTER_EVENT_STATE.ENCOUNTER_START });
    }

    #region EVENT FUNCTIONS
    public void OnEncounterEventReceived(EncounterEventArg arg)
    {
        if(arg.State==ENCOUNTER_EVENT_STATE.ENCOUNTER_START)
        {
            TriggerStartEncounter();
        }
        else
        {
            _encounterCount++;
            UIController.Instance.EnablePickACardMenu();
            UIController.Instance.AttributePrizeCards(SelectPickCards(3));
        }
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
            TurnEventsHandler.Instance.TurnEvent.Invoke(TURN_EVENT_STATE.TURN_START);
            ResetReadiness();
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
    #endregion

    #region TRIGGER FUNCTIONS
    /// <summary>
    /// Function that will trigger the start of an encounter.
    /// An encounter against an opponent is defined by a series of turns until one of the actors die.
    /// At the start of an encounter, the player regens their DEF, all their hand cards are added to the bottom of their decks and the decks are then shuffled.
    /// </summary>
    private void TriggerStartEncounter()
    {
        //HERE we do :
        // select the opponent based on rating
        OpponentData selectedOpponent = _opponentSelectorController.ObtainOpponentData(_encounterCount+1);
        Debug.Log("[GAME MANAGER] : Selected opponent : "+selectedOpponent.Name);
        //assign the opponent to the opponent prefab :
        _opponentPrefab.GetComponent<OpponentAIController>().SetOpponentData(selectedOpponent);
        // construct the decks :
        _opponentController.GetDeckController().ConstructDeck(selectedOpponent.InitCardDeck.Cards);
        _playerController.GetDeckController().ConstructDeck(_playerInitCardData.Cards);
        // THEN (and only THEN) :
        _curActivePlayer = _playerController;
        TurnEventsHandler.Instance.TurnEvent.Invoke(TURN_EVENT_STATE.TURN_START);
        UIController.Instance.EnableInGameUI();
    }

    /// <summary>
    /// Function that will trigger the start of a turn.
    /// Each turn is composed of two plays : the player's play, then the opponent's play.
    /// If during said plays there is no trigger to end the turn, then we begin a new turn (this function)
    /// </summary>
    public void TriggerStartTurn()
    {
        _curActivePlayer = _playerController;
        //TriggerStartPlay();
    }

    /// <summary>
    /// Function triggered either by the start of the turn OR another play, will start the play with the new actor.
    /// </summary>
    private void TriggerStartPlay()
    {
        PlayController.Instance.ResetPlay();
        PlayController.Instance.AssignPlayerAndOpponent(_curActivePlayer.gameObject,_curActivePlayer==_opponentController?_playerPrefab:_opponentPrefab);
        TurnEventsHandler.Instance.PlayEvent.Invoke(new PlayEventArg() { Holder = _curActivePlayer==_opponentController ? PLAY_HOLDER_TYPE.OPPONENT:PLAY_HOLDER_TYPE.PLAYER, State = PLAY_EVENT_STATE.PLAY_BEGIN});
    }
    #endregion

    #region PICK A CARD FUNCTIONS
    /// <summary>
    /// Function that will select an inputted number of cards amongst a list. The selected cards will be each unique from one another.
    /// As such, we can never request more cards than are currently present in the PlayerCardData element
    /// </summary>
    /// <param name="num">Number of cards picked</param>
    /// <returns>Selected cards</returns>
    private CardController[] SelectPickCards(int num)
    {
        List<CardController> cards = new List<CardController>();
        cards.Add(_playerAvailableCardData.Cards[Random.Range(0, _playerAvailableCardData.Cards.Count)]);
        for (int i = 1; i < num; i++)
        {
            var selectedCard = _playerAvailableCardData.Cards[Random.Range(0, _playerAvailableCardData.Cards.Count)];
            while (cards.Contains(selectedCard))
            {
                selectedCard = _playerAvailableCardData.Cards[Random.Range(0, _playerAvailableCardData.Cards.Count)];
            }
            cards.Add(selectedCard);
            Debug.Log("[GAME MANAGER] : PRIZE CARD SELECTED : " + cards[i].CardName);
        }
        return cards.ToArray();
    }
    #endregion

    public void UpdateReadiness(PLAY_HOLDER_TYPE type)
    {
        if (type == PLAY_HOLDER_TYPE.OPPONENT) { _opponentIsReady = true; }
        else { _playerIsReady = true; }

        if(_playerIsReady && _opponentIsReady)
        {
            Debug.Log("[GAME MANAGER] : BOTH PLAYERS ARE READY -> Starting first play");
            TriggerStartPlay();
        }
    }
    private void ResetReadiness()
    {
        _opponentIsReady = false;
        _playerIsReady = false;
    }
}
