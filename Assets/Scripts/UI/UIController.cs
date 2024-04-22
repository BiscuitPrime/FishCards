using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


[RequireComponent(typeof(AudioSource))]
public class UIController : MonoBehaviour
{
    #region SINGLETON DESIGN PATTERN
    public static UIController Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(this);
        _audioSource = GetComponent<AudioSource>();
    }
    #endregion

    #region SERIALIZED PARAMETERS
    [Header("Game UI Elements")]
    [SerializeField] private GameObject _ingameUI;
    [SerializeField] private GameObject _escButton;
    [SerializeField] private GameObject _displayDeck;
    [SerializeField] private GameObject _cardReader;
    [SerializeField] private GameObject _returnToMainMenuScreen;

    [Header("Main Menu Elements")]
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private TextMeshProUGUI _mainMenuVersionText;
    [SerializeField] private GameObject _mainMenuQuitButton;

    [Header("Settings Menu Elements")]
    [SerializeField] private GameObject _settingsMenu;

    [Header("Achievements Menu Elements")]
    [SerializeField] private GameObject _achievementsMenu;

    [Header("EncounterSuccess Menu Elements")]
    [SerializeField] private GameObject _encounterSuccessMenu;

    [Header("Pick A Card Menu Elements")]
    [SerializeField] private GameObject _pickACardMenu;
    [SerializeField] private DisplayCardController _displayCard1;
    [SerializeField] private DisplayCardController _displayCard2;
    [SerializeField] private DisplayCardController _displayCard3;

    [Header("End Menu Elements")]
    [SerializeField] private GameObject _endMenu;
    [SerializeField] private GameObject _endMenuQuitButton;
    [SerializeField] private TextMeshProUGUI _endMenuText;

    [Header("Loading Screen Elements")]
    [SerializeField] private GameObject _loadingMenu;

    [Header("Scene Management")]
    [SerializeField] private int _startSceneBuildIndex=1;

    [Header("Buttons Audio")]
    [SerializeField] private AudioClip _buttonClickAudioClip;

    [Header("Confirmation Window")]
    [SerializeField] private GameObject _confirmationWindow;

    [Header("Informative Menu Elements")]
    [SerializeField] private GameObject _informativeMenu;

    #endregion

    private AudioSource _audioSource;

    private void Start()
    {
        EnableMainMenu();
    }

    #region MENU FUNCTIONS
    public void EnableMainMenu()
    {
        _mainMenu.SetActive(true);
        _endMenu.SetActive(false);
        _loadingMenu.SetActive(false);
        _pickACardMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _achievementsMenu.SetActive(false);
        _confirmationWindow.SetActive(false);
        _ingameUI.SetActive(false);
        _encounterSuccessMenu.SetActive(false);
        _informativeMenu.SetActive(false);
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            _mainMenuQuitButton.SetActive(false);
        }
        _mainMenuVersionText.text = "Version "+Application.version;
    }
    public void EnableSettingsMenu()
    {
        _mainMenu.SetActive(false);
        _loadingMenu.SetActive(false);
        _endMenu.SetActive(false);
        _settingsMenu.SetActive(true);
        _achievementsMenu.SetActive(false);
        _pickACardMenu.SetActive(false);
        _ingameUI.SetActive(false);
        _encounterSuccessMenu.SetActive(false);
        _informativeMenu.SetActive(false);
    }
    public void EnableEndMenu()
    {
        _mainMenu.SetActive(false);
        _endMenu.SetActive(true);
        _loadingMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _achievementsMenu.SetActive(false);
        _pickACardMenu.SetActive(false);
        _ingameUI.SetActive(false);
        _encounterSuccessMenu.SetActive(false);
        _informativeMenu.SetActive(false);
        DisplayEndScore();
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            _endMenuQuitButton.SetActive(false);
        }
    }
    public void EnableLoadingScreen()
    {
        _mainMenu.SetActive(false);
        _endMenu.SetActive(false);
        _loadingMenu.SetActive(true);
        _settingsMenu.SetActive(false);
        _achievementsMenu.SetActive(false);
        _pickACardMenu.SetActive(false);
        _ingameUI.SetActive(false);
        _encounterSuccessMenu.SetActive(false);
        _informativeMenu.SetActive(false);
    }
    public void EnablePickACardMenu()
    {
        _mainMenu.SetActive(false);
        _endMenu.SetActive(false);
        _loadingMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _achievementsMenu.SetActive(false);
        _pickACardMenu.SetActive(true);
        _ingameUI.SetActive(false);
        _encounterSuccessMenu.SetActive(false);
        _informativeMenu.SetActive(false);
    }
    public void EnableInGameUI()
    {
        _mainMenu.SetActive(false);
        _endMenu.SetActive(false);
        _loadingMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _achievementsMenu.SetActive(false);
        _pickACardMenu.SetActive(false);
        _ingameUI.SetActive(true);
        _encounterSuccessMenu.SetActive(false);
        _displayDeck.SetActive(false);
        _cardReader.SetActive(false);
        _returnToMainMenuScreen.SetActive(false);
        _informativeMenu.SetActive(false);
    }
    public void EnableAchievementsMenu()
    {
        _mainMenu.SetActive(false);
        _endMenu.SetActive(false);
        _loadingMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _achievementsMenu.SetActive(true);
        _pickACardMenu.SetActive(false);
        _ingameUI.SetActive(false);
        _encounterSuccessMenu.SetActive(false);
        _informativeMenu.SetActive(false);
    }
    public void EnableEncounterSuccessMenu()
    {
        _mainMenu.SetActive(false);
        _endMenu.SetActive(false);
        _loadingMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _achievementsMenu.SetActive(false);
        _pickACardMenu.SetActive(false);
        _ingameUI.SetActive(false);
        _encounterSuccessMenu.SetActive(true);
        _informativeMenu.SetActive(false);
    }
    public void EnableInformativeMenu()
    {
        _mainMenu.SetActive(false);
        _endMenu.SetActive(false);
        _loadingMenu.SetActive(false);
        _settingsMenu.SetActive(false);
        _achievementsMenu.SetActive(false);
        _pickACardMenu.SetActive(false);
        _ingameUI.SetActive(false);
        _encounterSuccessMenu.SetActive(false);
        _informativeMenu.SetActive(true);
    }
    #endregion

    #region BUTTON FUNCTIONS
    private void PlayAudioButtonClick()
    {
        _audioSource.clip = _buttonClickAudioClip;
        _audioSource.Play();
    }

    public void OnReloadGameButtonClicked()
    {
        PlayAudioButtonClick();
        LevelLoaderController.Instance.LoadLevel(0);
        EnableMainMenu();
    }
    public void OnPickACardMenuButtonClicked()
    {
        PlayAudioButtonClick();
        EnablePickACardMenu();
    }
    public void OnStartGameButtonClicked()
    {
        PlayAudioButtonClick();
        LevelLoaderController.Instance.LoadLevel(_startSceneBuildIndex);
    }
    public void OnMainMenuButtonClicked()
    {
        PlayAudioButtonClick();
        EnableMainMenu();
    }
    public void OnEndMenuButtonClicked()
    {
        PlayAudioButtonClick();
        EnableEndMenu();
    }
    public void OnSettingsButtonClicked()
    {
        PlayAudioButtonClick();
        EnableSettingsMenu();
    }
    public void OnAchievementsButtonClicked()
    {
        PlayAudioButtonClick();
        EnableAchievementsMenu();
    }
    public void OnQuitButtonClicked()
    {
        PlayAudioButtonClick();
        Application.Quit();
    }
    public void OnEncounterSuccessContinueButtonClicked()
    {
        PlayAudioButtonClick();
        EnablePickACardMenu();
        GameManager.Instance.TriggerPickACard();
    }
    public void OnResetProgressionButtonClicked()
    {
        PlayAudioButtonClick();
        _confirmationWindow.SetActive(true);
    }
    public void OnResetProgressionValidationButtonClicked()
    {
        PlayAudioButtonClick();
        _confirmationWindow.SetActive(false);
    }
    public void OnCancelResetProgressionButtonClicked()
    {
        PlayAudioButtonClick();
        _confirmationWindow.SetActive(false);
    }
    public void OnInformativeMenuButtonClicked()
    {
        PlayAudioButtonClick();
        EnableInformativeMenu();
    }
    public void OnHideReturnToMainMenuScreenClicked()
    {
        DisableReturnToMainScreenMenu();
    }
    #endregion

    #region INGAME UI FUNCTIONS
    /// <summary>
    /// Function that will hide the display deck.
    /// </summary>
    public void DisableDisplayDeck()
    {
        _escButton.SetActive(true);
        _displayDeck.SetActive(false);
    }
    /// <summary>
    /// Function that will enable the display deck (for example, if player wants to check their deck during the game).
    /// </summary>
    public void EnableDisplayDeck()
    {
        _escButton.SetActive(false);
        _displayDeck.SetActive(true);
    }

    public void EnableCardReader(CardController card)
    {
        _cardReader.SetActive(true);
        _cardReader.GetComponent<DisplayCardController>().AttributeCardData(card);
    }
    public void DisableCardReader()
    {
        if(_cardReader != null)
        {
            _cardReader.SetActive(false);
        }
    }
    public void EnableReturnToMainScreenMenu()
    {
        _returnToMainMenuScreen.SetActive(true);
    }
    public void DisableReturnToMainScreenMenu()
    {
        _returnToMainMenuScreen.SetActive(false);
    }
    /// <summary>
    /// Function that will request the call to the Return to main menu screen, and we will display it or not depending on its cur state
    /// </summary>
    public void RequestReturnToMainMenuScreen()
    {
        if (_returnToMainMenuScreen.activeSelf)
        {
            DisableReturnToMainScreenMenu();
        }
        else
        {
            EnableReturnToMainScreenMenu();
        }
    }
    #endregion

    #region PICK A CARD FUNCTIONS
    /// <summary>
    /// Function that will assign the selected prize cards to the correct display cards
    /// </summary>
    /// <param name="card"></param>
    public void AttributePrizeCards(CardController[] card)
    {
        if (card == null)
        {
            EnableInGameUI();
            TurnEventsHandler.Instance.EncounterEvent.Invoke(new EncounterEventArg() { State = ENCOUNTER_EVENT_STATE.ENCOUNTER_START });
            return;
        }

        //TODO : FOR NOW, we assume that the prize will ALWAYS be 3 randomly selected cards. In a future update (lol), maybe redo that part with more modularity in mind, similar to the spawn/load card assets done in HandController
        if (card.Length < 1)
        {
            Debug.Log("[UI CONTROLLER] : Less than 1 cards were given to the pick a card menu. RED ALERT.");
            EnableInGameUI();
            TurnEventsHandler.Instance.EncounterEvent.Invoke(new EncounterEventArg() { State = ENCOUNTER_EVENT_STATE.ENCOUNTER_START });
            return;
        }
        Debug.Log("Attributing prize cards");
        if(card.Length >= 3)
        {
            _displayCard3.AttributeCardData(card[2]);
        }
        else
        {
            _displayCard3.gameObject.SetActive(false);
        }
        if (card.Length >= 2)
        {
            _displayCard2.AttributeCardData(card[1]);
        }
        else
        {
            _displayCard2.gameObject.SetActive(false);
        }
        _displayCard1.AttributeCardData(card[0]);
    }
    #endregion

    #region END MENU FUNCTIONS
    private void DisplayEndScore()
    {
        //_endMenuText.text = "You successfully flirted with : "+GameManager.Instance.GetScore().ToString()+" tanks !";
    }
    #endregion
}

