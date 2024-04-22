using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Static class that will handle the storage of values in Player Prefs
/// </summary>
public static class PlayerPrefsHandler
{
    /// <summary>
    /// Deletes a specific Player Pref if it exists
    /// </summary>
    /// <param name="playerPrefName">Requested Player Pref</param>
    public static void DeletePlayerPref(string playerPrefName)
    {
        if (PlayerPrefs.HasKey(playerPrefName))
        {
            PlayerPrefs.DeleteKey(playerPrefName);
        }
    }

    /// <summary>
    /// Deletes all player prefs ! DANGEROUS, BEWARE !
    /// </summary>
    public static void DeleteAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
    }

    /// <summary>
    /// Updates or creates a player pref entry with the associated string value
    /// </summary>
    /// <param name="playerPrefName">Player pref requested</param>
    /// <param name="value">New or updated value</param>
    public static void ChangeStringPlayerPref(string playerPrefName, string value)
    {
        PlayerPrefs.SetString(playerPrefName, value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Updates or creates a player pref entry with the associated float value
    /// </summary>
    /// <param name="playerPrefName">Player pref requested</param>
    /// <param name="value">New or updated value</param>
    public static void ChangeFloatPlayerPref(string playerPrefName, float value)
    {
        PlayerPrefs.SetFloat(playerPrefName, value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Updates or creates a player pref entry with the associated int value
    /// </summary>
    /// <param name="playerPrefName">Player pref requested</param>
    /// <param name="value">New or updated value</param>
    public static void ChangeIntPlayerPrefs(string playerPrefName, int value)
    {
        Debug.Log("Changed player pref : " + playerPrefName + " to value : " + value);
        PlayerPrefs.SetInt(playerPrefName, value);
        PlayerPrefs.Save();
    }

    /// <summary>
    /// Returns the associated string value saved in the player prefs.
    /// </summary>
    /// <param name="playerPrefName">Player pref requested</param>
    /// <returns>string value if saved, null otherwise</returns>
    public static string ObtainStringPlayerPref(string playerPrefName)
    {
        if (PlayerPrefs.HasKey(playerPrefName))
        {
            return PlayerPrefs.GetString(playerPrefName);
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// Returns the associated int value saved in the player prefs.
    /// </summary>
    /// <param name="playerPrefName">Player pref requested</param>
    /// <returns>int value if saved, -1 otherwise</returns>
    public static int ObtainIntPlayerPref(string playerPrefName)
    {
        if (PlayerPrefs.HasKey(playerPrefName))
        {
            return PlayerPrefs.GetInt(playerPrefName);
        }
        else
        {
            return -1;
        }
    }

    /// <summary>
    /// Returns the associated float value saved in the player prefs.
    /// </summary>
    /// <param name="playerPrefName">Player pref requested</param>
    /// <returns>Float value if saved, -1 otherwise</returns>
    public static float ObtainFloatPlayerPref(string playerPrefName)
    {
        if (PlayerPrefs.HasKey(playerPrefName))
        {
            return PlayerPrefs.GetFloat(playerPrefName);
        }
        else
        {
            return -1;
        }
    }
}
