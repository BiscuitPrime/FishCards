using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Script that will contain and handle an actor's values : HP, DEF and AGI
/// HP  : the health of the actor : without it, they die and the encounter (or game) finishes
/// DEF : defense of the actor    : defensive layer around the player that will interact with the PIER value of attack cards, REGENS between rounds
/// AGI : agility of the actor    : indicates whether an attack lands or not by interacting with the TRACK value of attack cards
/// </summary>
public class ActorValuesController : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] protected ActorValuesData _valuesInitData;
    [SerializeField] protected int _hp;
    [SerializeField] protected int _def;
    [SerializeField] protected int _agi;

    protected void OnValidate()
    {
        Assert.IsNotNull(_valuesInitData);
    }

    protected void Awake()
    {
        _hp = _valuesInitData.HP;
        _def = _valuesInitData.DEF;
        _agi = _valuesInitData.AGI;
    }

    #region VALUES INCREASE/DECREASE FUNCTIONS
    public void RegenDefenseValue()
    {
        _def = _valuesInitData.DEF;
    }
    public void IncreaseHPValue(int value)
    {
        _hp += value;
    }
    public void IncreaseDEFValue(int value)
    {
        _def += value;
    }
    public void IncreaseAGIValue(int value)
    {
        _agi += value;
    }
    public void DecreaseHPValue(int value)
    {
        _hp -= value;
    }
    public void DecreaseDEFValue(int value)
    {
        _def -= value;
    }
    public void DecreaseAGIValue(int value)
    {
        _agi -= value;
    }
    #endregion

    /// <summary>
    /// Function that will handle the attack and its analysis (whether it lands, the damage dealt etc...)
    /// </summary>
    /// <param name="atk">ATK value of the card</param>
    /// <param name="pier">PIER value of the card</param>
    /// <param name="track">TRACK value of the card</param>
    public void AttemptToTakeDamage(int atk, int pier, int track)
    {
        if (track>=2*_agi) //100% chance to land
        {
            AttackLandingCalculations(atk, pier);
        }
        else if (track>=_agi) //70% chance to land
        {
            if (Random.Range(1, 100) >= 30)
            {
                AttackLandingCalculations(atk, pier);
            }
        }
        else if (track>=((_agi/2)>=1? (_agi / 2) : 1)) //20% to land
        {
            if (Random.Range(1, 100) >= 80)
            {
                AttackLandingCalculations(atk, pier);
            }
        }
        else
        {
            return;
        }
    }



    /// <summary>
    /// Function that will calculate the effect of a landing attack (the attack has already landed per agility calculations
    /// </summary>
    /// <param name="atk">ATK value of the attack card</param>
    /// <param name="pier">PIER value of the attack card</param>
    protected void AttackLandingCalculations(int atk, int pier)
    {
        if (pier >= _def)
        {
            _hp = _hp - atk;
        }
        else
        {
            if(_def - atk >= 0)
            {
                _def = _def - atk;
            }
            else
            {
                _def = 0;
                _hp = _hp - (atk - _def);
            }
        }
        if (_hp <= 0)
        {
            Death();
        }
    }

    /// <summary>
    /// Function that will start the Death process of the actor
    /// </summary>
    protected virtual void Death()
    {
        Debug.Log("Actor " + gameObject.name + " is dead !");
    }
}
