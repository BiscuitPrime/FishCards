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

    private BuffContainer _buffContainer;

    protected void OnValidate()
    {
        Assert.IsNotNull(_valuesInitData);
    }

    protected void Awake()
    {
        if (_valuesInitData != null)
        {
            AssignData();
        }
    }

    private void Start()
    {
        TurnEventsHandler.Instance.EncounterEvent.AddListener(OnEncounterEventReceived);
    }
    private void OnDestroy()
    {
        TurnEventsHandler.Instance.EncounterEvent?.RemoveListener(OnEncounterEventReceived);
    }

    #region DATA RELATED FUNCTIONS
    public void SetData(ActorValuesData data)
    {
        _valuesInitData = data;
        AssignData();
    }

    private void AssignData()
    {
        _hp = _valuesInitData.HP;
        _def = _valuesInitData.DEF;
        _agi = _valuesInitData.AGI;
    }
    #endregion

    /// <summary>
    /// Function called when the event Encounter has been received.
    /// START : At the start of the encounter, the player regens their entire def.
    /// </summary>
    /// <param name="arg0">Arg of the event</param>
    private void OnEncounterEventReceived(EncounterEventArg arg0)
    {
        if(arg0.State==ENCOUNTER_EVENT_STATE.ENCOUNTER_START && _valuesInitData!=null)
        {
            _def = _valuesInitData.DEF;
        }
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

    #region DAMAGE FUNCTIONS
    /// <summary>
    /// Function that will handle the attack and its analysis (whether it lands, the damage dealt etc...)
    /// </summary>
    /// <param name="atk">ATK value of the card</param>
    /// <param name="pier">PIER value of the card</param>
    /// <param name="track">TRACK value of the card</param>
    public void AttemptToTakeDamage(int atk, int pier, int track)
    {
        //we begin by totaling the buff values on agility that the actor has so that the agi/track calculations can be done :
        _buffContainer = GetComponent<BuffContainer>();
        int agi = _agi;
        if (_buffContainer != null)
        {
            var buffs = _buffContainer.ObtainListOfBuffs();
            foreach( var buff in buffs)
            {
                var tmp = buff as ActorBuffObject;
                if(tmp == null) { continue; }
                agi += tmp.AGI;
            }
        }

        if (track>=2*agi) //100% chance to land
        {
            AttackLandingCalculations(atk, pier);
        }
        else if (track>=agi) //70% chance to land
        {
            if (Random.Range(1, 100) >= 30)
            {
                AttackLandingCalculations(atk, pier);
            }
            else
            {
                Debug.Log("[ActorValuesController] : Attack failed to land");
                TextVFXSpawner.Instance.RequestSpawnVFX(gameObject.GetComponent<ActorController>().GetVFXSpawnPoint().transform.position, VFX_TYPE.MISS);
            }
        }
        else if (track>=((agi/2)>=1? (agi / 2) : 1)) //20% to land
        {
            if (Random.Range(1, 100) >= 80)
            {
                AttackLandingCalculations(atk, pier);
            }
            else
            {
                Debug.Log("[ActorValuesController] : Attack failed to land");
                TextVFXSpawner.Instance.RequestSpawnVFX(gameObject.GetComponent<ActorController>().GetVFXSpawnPoint().transform.position, VFX_TYPE.MISS);
            }
        }
        else
        {
            Debug.Log("[ActorValuesController] : Attack failed to land");
            TextVFXSpawner.Instance.RequestSpawnVFX(gameObject.GetComponent<ActorController>().GetVFXSpawnPoint().transform.position, VFX_TYPE.MISS);
            return;
        }
    }



    /// <summary>
    /// Function that will calculate the effect of a landing attack (the attack has already landed per agility calculations)
    /// </summary>
    /// <param name="atk">ATK value of the attack card</param>
    /// <param name="pier">PIER value of the attack card</param>
    protected void AttackLandingCalculations(int atk, int pier)
    {
        Debug.Log("[ActorValuesController] : Attack landed - calculating with defenses");

        //first, we start by summing up def with the buffs
        int def = _def;
        List<BuffObject> buffs;
        if (_buffContainer != null)
        {
            buffs = _buffContainer.ObtainListOfBuffs();
            foreach (var buff in buffs)
            {
                var tmp = buff as ActorBuffObject;
                if (tmp == null) { continue; }
                def += tmp.DEF;
            }
        }

        if (pier >= def) //DEF is too low : atk is fully taken by the player's HP
        {
            Debug.Log("[ACTOR VALUES CONTROLLER] : DEF is fully breached");
            //we calculate the remaining hp while first depleting the buffs (since they act like mini-hp bars outside of the hp bar itself).
            if (_buffContainer != null)
            {
                buffs = _buffContainer.ObtainListOfBuffs();
                foreach (var buff in buffs)
                {
                    var tmp = buff as ActorBuffObject;
                    if (tmp == null) { continue; }
                    if (atk <= tmp.HP)
                    {
                        tmp.TakeDamage(atk,0);
                        UpdateBuffValues(tmp, atk, 0);
                        //ENTIRE ATTACK HAS BEEN EATEN UP BY THE BUFF => atk is over
                        atk = 0;
                        break;
                    }
                    else
                    {
                        atk-=tmp.HP;
                        //tmp.TakeDamage(tmp.HP,0);
                        UpdateBuffValues(tmp, tmp.HP, 0);
                        //The buff is completely reduced, and the atk goes on to either the next buff OR the player's HP
                    }
                }
            }
            //At this point, the atk has went through all the buffs, so IF any atk remains, it goes to the HP bar :
            _hp = _hp - atk;
            TextVFXSpawner.Instance.RequestSpawnVFX(gameObject.GetComponent<ActorController>().GetVFXSpawnPoint().transform.position, VFX_TYPE.DAMAGE_RED);
        }
        else
        {
            if(def - atk >= 0)
            {
                Debug.Log("[ACTOR VALUES CONTROLLER] : DEF is partially depleted");
                //we calculate the remaining def while first depleting the buffs (since they act like mini-def bars outside of the def bar itself).
                if (_buffContainer != null)
                {
                    buffs = _buffContainer.ObtainListOfBuffs();
                    foreach (var buff in buffs)
                    {
                        var tmp = buff as ActorBuffObject;
                        if (tmp == null) { continue; }
                        if (atk <= tmp.DEF)
                        {
                            Debug.Log("[ACTOR VALUES CONTROLLER] : ATK entirely eaten up by a defensive buff");
                            //tmp.TakeDamage(0, atk);
                            UpdateBuffValues(tmp, 0, atk);
                            //ENTIRE ATTACK HAS BEEN EATEN UP BY THE BUFF => atk is over
                            atk = 0;
                            break;
                        }
                        else
                        {
                            Debug.Log("[ACTOR VALUES CONTROLLER] : buff failed to stop entirely the attack");
                            atk -= tmp.DEF;
                            //The buff is completely reduced, and the atk goes on to either the next buff OR the player's HP
                            //tmp.TakeDamage(0, tmp.DEF);
                            UpdateBuffValues(tmp, 0, tmp.DEF);
                        }
                    }
                }
                //At this point, the atk has went through all the buffs' def, so IF any atk remains, it goes to the DEF bar :
                _def = _def - atk;
                TextVFXSpawner.Instance.RequestSpawnVFX(gameObject.GetComponent<ActorController>().GetVFXSpawnPoint().transform.position, VFX_TYPE.DAMAGE_BLUE);
            }
            else
            {
                Debug.Log("[ACTOR VALUES CONTROLLER] : DEF is fully depleted");
                int remainingATK = atk - def;
                //we calculate the remaining hp while first depleting the buffs (since they act like mini-hp bars outside of the hp bar itself).
                if (_buffContainer != null)
                {
                    buffs = _buffContainer.ObtainListOfBuffs();
                    foreach (var buff in buffs)
                    {
                        var tmp = buff as ActorBuffObject;
                        if (tmp == null) { continue; }
                        if (remainingATK <= tmp.HP)
                        {
                            //tmp.TakeDamage(remainingATK, 0);
                            UpdateBuffValues(tmp, remainingATK, 0);
                            remainingATK = 0;
                            //ENTIRE ATTACK HAS BEEN EATEN UP BY THE BUFF => atk is over
                            break;
                        }
                        else
                        {
                            remainingATK -= tmp.HP;
                            //tmp.TakeDamage(tmp.HP, 0);
                            UpdateBuffValues(tmp,tmp.HP,0);
                            //The buff is completely reduced, and the atk goes on to either the next buff OR the player's HP
                        }
                    }
                }
                _hp = _hp - remainingATK;
                _def = 0;
                TextVFXSpawner.Instance.RequestSpawnVFX(gameObject.GetComponent<ActorController>().GetVFXSpawnPoint().transform.position, VFX_TYPE.DAMAGE_RED);
            }
        }

        //After the attack has been fully calculated, we remove the emptied buffs :
        if (_buffContainer != null)
        {
            buffs = _buffContainer.ObtainListOfBuffs();
            List<BuffObject> remainingBuffs = new List<BuffObject>();
            foreach (var buff in buffs)
            {
                var tmp = buff as ActorBuffObject;
                if (tmp == null) { continue; }
                if(tmp.HP!=0 || tmp.DEF != 0)
                {
                    remainingBuffs.Add(tmp);
                }
            }
            _buffContainer.SetBuffsList(remainingBuffs);
        }
        Debug.Log("[ACTOR VALUES CONTROLLER] : Damage calculations are complete with current actor values : HP= "+_hp+", DEF= "+_def+", AGI = "+_agi);

        if (_hp <= 0)
        {
            Death();
        }
    }

    /// <summary>
    /// Function that will update the buff values, useful for the player to update their buff bubble
    /// </summary>
    private void UpdateBuffValues(ActorBuffObject buffObj, int hp, int def)
    {
        if(_buffContainer.GetType() == typeof(PlayerBuffContainerController)) 
        {
            buffObj.TakeDamage(hp, def);
            var container = _buffContainer as PlayerBuffContainerController;
            container.UpdateBubbleDisplay(buffObj);
        }
    }

    /// <summary>
    /// Function that will start the Death process of the actor.
    /// </summary>
    protected virtual void Death()
    {
        Debug.Log("Actor " + gameObject.name + " is dead !");
        if (gameObject.GetComponent<ActorController>().GetHandController().GetHolderType() == HOLDER_TYPE.PLAYER)
        {
            TurnEventsHandler.Instance.DeathEvent.Invoke(HOLDER_TYPE.PLAYER);
        }
        else
        {
            TurnEventsHandler.Instance.DeathEvent.Invoke(HOLDER_TYPE.OPPONENT);
        }
    }
    #endregion
}
