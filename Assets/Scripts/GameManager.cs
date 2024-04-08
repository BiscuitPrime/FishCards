using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    
    [Header("Cards")]
    [SerializeField,Tooltip("The starting cards of the player, remains const during runtime.")] private CardsListData _playerInitCardsData;
    [SerializeField,Tooltip("All the cards available to the player during play.")] private CardsListData _availableCardsData;
    [SerializeField, Tooltip("The effective player cards. This data will be modified during play.")] private CardsListData _playerCardsData;

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
    private bool _encounterIsActive = false;
    #endregion

    private void OnValidate()
    {
        Assert.IsNotNull(_availableCardsData);
        Assert.IsNotNull(_playerInitCardsData);
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
        TurnEventsHandler.Instance.DeathEvent.AddListener(OnDeathEventReceived);
    }

    private void OnDestroy()
    {
        TurnEventsHandler.Instance.PlayEvent?.RemoveListener(OnPlayEventReceived);
        TurnEventsHandler.Instance.TurnEvent?.RemoveListener(OnTurnEventReceived);
        TurnEventsHandler.Instance.EncounterEvent?.RemoveListener(OnEncounterEventReceived);
        TurnEventsHandler.Instance.DeathEvent?.RemoveListener(OnDeathEventReceived);
    }

    public CardsListData GetPlayerInitCards()
    {
        return _playerInitCardsData;
    }
    public CardsListData GetPlayerCards()
    {
        return _playerCardsData;
    }
    public CardsListData GetAvailableCards()
    {
        return _availableCardsData;
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
    /// <summary>
    /// Called when an encounter event is received.
    /// START : starts the encounter
    /// END : increase encounter count and launchs the pick a card menu system
    /// </summary>
    /// <param name="arg"></param>
    public void OnEncounterEventReceived(EncounterEventArg arg)
    {
        if(arg.State==ENCOUNTER_EVENT_STATE.ENCOUNTER_START)
        {
            _encounterIsActive = true;
            TriggerStartEncounter();
        }
        else
        {
            _encounterIsActive = false;
            _encounterCount++;
            UIController.Instance.EnablePickACardMenu();
            UIController.Instance.AttributePrizeCards(SelectPickCards(GameValues.PICK_A_CARD_OPTIONS));
        }
    }

    /// <summary>
    /// Called when the event TurnEvent is received.
    /// END : should have been sent EITHER by this script OR by the death of an actor.
    /// </summary>
    public void OnTurnEventReceived(TURN_EVENT_STATE state)
    {
        if (!_encounterIsActive) { return; } //if the encounter has already concluded, we do not go any further.
        if (state == TURN_EVENT_STATE.TURN_END)
        {
            Debug.Log("[GAME MANAGER] : TURN HAS ENDED");
            _curActivePlayer = null;
            ResetReadiness();
            TurnEventsHandler.Instance.TurnEvent.Invoke(TURN_EVENT_STATE.TURN_START);
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
        if (!_encounterIsActive) { return; } //if the encounter has already concluded, we do not go any further.
        Debug.Log("[GAME MANAGER] : Play Event received with values : Holder : "+arg.Holder.ToString()+" and State : "+arg.State.ToString());
        if (arg.State==PLAY_EVENT_STATE.PLAY_END)
        {
            if(arg.Holder==HOLDER_TYPE.OPPONENT && _curActivePlayer==_opponentController) //if the play that ended was the opponent's, then the turn is over. We also test if the opponent is the last active, as IF there as been a death, the event has already been received and updated the curactive
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
    /// Called when either the player or opponent have died, launching the death event.
    /// PLAYER : the game ends, we display the end menu.
    /// OPPONENT : the encounter ends.
    /// </summary>
    /// <param name="type"></param>
    public void OnDeathEventReceived(HOLDER_TYPE type)
    {
        if (type == HOLDER_TYPE.PLAYER)
        {
            //Ends the game
            _encounterIsActive = false;
            UIController.Instance.EnableEndMenu();
        }
        else
        {
            //Ends the encounter
            TurnEventsHandler.Instance.EncounterEvent.Invoke(new EncounterEventArg() { State = ENCOUNTER_EVENT_STATE.ENCOUNTER_END });
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
        _playerController.GetDeckController().ConstructDeck(_playerCardsData.Cards);
        // THEN (and only THEN) :
        _curActivePlayer = _playerController;
        TurnEventsHandler.Instance.TurnEvent.Invoke(TURN_EVENT_STATE.TURN_START);
        //UIController.Instance.EnableInGameUI();
    }

    /// <summary>
    /// Function that will trigger the start of a turn.
    /// Each turn is composed of two plays : the player's play, then the opponent's play.
    /// If during said plays there is no trigger to end the turn, then we begin a new turn (this function)
    /// </summary>
    public void TriggerStartTurn()
    {
        Debug.Log("[GAME MANAGER] : STARTING TURN");
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
        TurnEventsHandler.Instance.PlayEvent.Invoke(new PlayEventArg() { Holder = _curActivePlayer==_opponentController ? HOLDER_TYPE.OPPONENT:HOLDER_TYPE.PLAYER, State = PLAY_EVENT_STATE.PLAY_BEGIN});
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
        if (_playerCardsData.Cards.Count == _availableCardsData.Cards.Count)
        {
            Debug.Log("[GAME MANAGER] : Player possesses all the available cards => pick a card will not be displayed");
            return null;
        }

        List<CardController> possibleCards = new List<CardController>();

        //First we create a list of all available cards EXCLUDING THE CARDS THE PLAYER ALREADY POSSESS :
        List<CardController> availableCards = new List<CardController>();
        foreach(var card in _availableCardsData.Cards)
        {
            if (!_playerCardsData.Cards.Contains(card))
            {
                availableCards.Add(card);
            }
        }

        //Then, we try to obtain num cards, each unique.
        //If that's impossible, we take as many unique cards (amongst the cards player DOESN'T ALREADY HAVE) as we can
        for (int i = 0; i < num; i++)
        {
            var selectedCard = availableCards[Random.Range(0, availableCards.Count)];
            while (possibleCards.Contains(selectedCard))
            {
                selectedCard = availableCards[Random.Range(0, availableCards.Count)];
            }
            possibleCards.Add(selectedCard);
            availableCards.Remove(selectedCard);
            if(availableCards.Count < 1)
            {
                break;
            }
            Debug.Log("[GAME MANAGER] : PRIZE CARD SELECTED : " + possibleCards[i].CardName);
        }
        return possibleCards.ToArray();
    }

    /// <summary>
    /// Function that will add a card to the player's cards
    /// </summary>
    /// <param name="card"></param>
    public void AddCardToPlayerCards(CardController card)
    {
        _playerCardsData.Cards.Add(card);
    }

    /// <summary>
    /// Function that will reset the player's cards to the player's init cards
    /// </summary>
    public void ResetPlayerCards()
    {
        _playerCardsData.Cards.Clear();
        _playerCardsData.SetCards(_playerInitCardsData.Cards);
    }
    #endregion

    public void UpdateReadiness(HOLDER_TYPE type)
    {
        if (type == HOLDER_TYPE.OPPONENT) { _opponentIsReady = true; }
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
