using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Script only used to select the next opponent based on the current encounter number AND its difficulty rating.
/// </summary>
public class OpponentSelectorController : MonoBehaviour
{
    [Header("Opponents")]
    [SerializeField] private List<OpponentData> _possibleOpponents;
    private Dictionary<int, List<OpponentData>> _opponentsRatingDict; //dictionnary that to each difficulty rating assigns all the possible opponents

    private void Awake()
    {
        CreateDict();
    }

    private void OnValidate()
    {
        Assert.IsNotNull( _possibleOpponents );
    }

    private void CreateDict()
    {
        _opponentsRatingDict = new Dictionary<int, List<OpponentData>>();
        for(int i=1; i < GameValues.MAX_DIFFICULTY_RATING+1; i++)
        {
            List<OpponentData> opponentPool = new List<OpponentData>();
            foreach (var opponent in _possibleOpponents)
            {
                if (opponent.DifficultyRating == i)
                {
                    opponentPool.Add(opponent);
                }
            }
            _opponentsRatingDict.Add(i, opponentPool);
        }
    }

    /// <summary>
    /// Function called by GameManager that will obtain the opponent data required for the next encounter based on its rating.
    /// </summary>
    /// <param name="rating">Difficulty rating of the encounter</param>
    /// <returns>An opponent data script obj</returns>
    public OpponentData ObtainOpponentData(int rating)
    {
        OpponentData opponentData;
        System.Random rand = new System.Random();
        if (rating>= GameValues.MAX_DIFFICULTY_RATING) //if the rating is above the max rating, we always pick the data within the max rating pool
        {
            Debug.Log("Searching oppo with rating : " + rating + " | size " + _opponentsRatingDict[GameValues.MAX_DIFFICULTY_RATING].Count);
            opponentData = _opponentsRatingDict[GameValues.MAX_DIFFICULTY_RATING].ElementAt(rand.Next(0, _opponentsRatingDict[GameValues.MAX_DIFFICULTY_RATING].Count));
        }
        else
        {
            opponentData = _opponentsRatingDict[rating].ElementAt(rand.Next(0, _opponentsRatingDict[rating].Count));
        }
        Debug.Log("[OPPONENT SELECTOR] : SELECTED OPPONENT : " + opponentData.Name);
        return opponentData;
    }
}
