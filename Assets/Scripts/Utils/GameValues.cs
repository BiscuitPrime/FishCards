using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This static class will contain all game-wide values
/// </summary>
public static class GameValues
{
    public static readonly int MAX_DIFFICULTY_RATING = 3; //the max difficulty rating : above this number, the same pool of opponents (with a rating equal to this number) will be used for all further encounters.
    public static readonly int PICK_A_CARD_OPTIONS = 3;

    public static readonly int PERCENTAGE_TRACK_OVER_AGI = 30;
    public static readonly int PERCENTAGE_TRACK_OVER_HALF_AGI = 80;

}
