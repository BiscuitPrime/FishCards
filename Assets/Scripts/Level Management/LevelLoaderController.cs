using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// This script, singleton and not destructible, will be the one that handles the loading and unloading of levels
/// </summary>
public class LevelLoaderController : MonoBehaviour
{
    #region SINGLETON DESIGN PATTERN
    public static LevelLoaderController Instance;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        DontDestroyOnLoad(this);
    }
    #endregion

    public void LoadLevel(int buildIndex)
    {
        StartCoroutine(LoadSceneAsynchronously(buildIndex));
    }

    private IEnumerator LoadSceneAsynchronously(int buildIndex)
    {
        AsyncOperation loadSceneAsyncOperation = SceneManager.LoadSceneAsync(buildIndex);
        loadSceneAsyncOperation.allowSceneActivation = false;
        UIController.Instance.EnableLoadingScreen();
        yield return new WaitForSeconds(0.2f);
        loadSceneAsyncOperation.allowSceneActivation = true;
        while (!loadSceneAsyncOperation.isDone)
        {
            float loadingProgress = Mathf.Clamp01(loadSceneAsyncOperation.progress / 0.9f);
            Debug.Log("Loading progress : " + loadingProgress);
            yield return null;
            UIController.Instance.EnableInGameUI();
            UIController.Instance.EnableDisplayDeck();
        }
    }
}
