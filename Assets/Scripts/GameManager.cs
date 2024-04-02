using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that will control the entire game
/// </summary>
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

}
