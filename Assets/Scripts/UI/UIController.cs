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
    [SerializeField] private GameObject _displayDeck;

    [Header("Main Menu Elements")]
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private TextMeshProUGUI _mainMenuVersionText;
    [SerializeField] private GameObject _mainMenuQuitButton;

    [Header("Settings Menu Elements")]
    [SerializeField] private GameObject _settingsMenu;

    [Header("Achievements Menu Elements")]
    [SerializeField] private GameObject _achievementsMenu;

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
        _ingameUI.SetActive(false);
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
        _displayDeck.SetActive(false);
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
    #endregion

    #region INGAME UI FUNCTIONS
    /// <summary>
    /// Function that will hide the display deck.
    /// </summary>
    public void HideDisplayDeck()
    {
        _displayDeck.SetActive(false);
    }
    /// <summary>
    /// Function that will enable the display deck (for example, if player wants to check their deck during the game).
    /// </summary>
    public void EnableDisplayDeck()
    {
        _displayDeck.SetActive(true);
    }
    #endregion

    #region PICK A CARD FUNCTIONS
    /// <summary>
    /// Function that will grant the 
    /// </summary>
    /// <param name="card"></param>
    public void AttributePrizeCards(CardController[] card)
    {
        //TODO : FOR NOW, we assume that the prize will ALWAYS be 3 randomly selected cards. In a future update (lol), maybe redo that part with more modularity in mind, similar to the spawn/load card assets done in HandController
        if (card.Length != 3)
        {
            Debug.LogError("[UI CONTROLLER] : More or less than 3 cards were given to the pick a card menu. RED ALERT.");
            return;
        }
        Debug.Log("Attributing prize cards : " + card[0]+" & " + card[1]+" & " + card[2]);
        _displayCard1.AttributeCardData(card[0]);
        _displayCard2.AttributeCardData(card[1]);
        _displayCard3.AttributeCardData(card[2]);
    }
    #endregion

    #region END MENU FUNCTIONS
    private void DisplayEndScore()
    {
        //_endMenuText.text = "You successfully flirted with : "+GameManager.Instance.GetScore().ToString()+" tanks !";
    }
    #endregion
}

