using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script of the factory that will create buffs.
/// </summary>
public static class BuffFactory
{
    #region CREATE ACTOR BUFF FUNCTIONS
    /// <summary>
    /// Function that will create a buff to a targeted actor. If actor already has buffs, it will simply add a new buff to the already existing container.
    /// </summary>
    /// <param name="target">Target of the buff</param>
    /// <param name="name">Name of the buff object</param>
    /// <param name="hp">HP value of the buff</param>
    /// <param name="def">DEF value of the buff</param>
    /// <param name="agi">AGI value of the buff</param>
    /// <param name="turnCounter">Turn counter value of the buff</param>
    public static void BuffActor(GameObject target, string name, int hp, int def, int agi, int turnCounter)
    {
        if (target.GetComponent<BuffContainer>() == null)
        {
            target.AddComponent<BuffContainer>();
        }
        target.GetComponent<BuffContainer>().AddBuff(CreateActorBuff(name,hp,def,agi,turnCounter));
    }

    /// <summary>
    /// Internal function that will create, and init, actor buff objects
    /// </summary>
    /// <returns></returns>
    private static ActorBuffObject CreateActorBuff(string name, int hp, int def, int agi, int turnCounter)
    {
        ActorBuffObject buff = ScriptableObject.CreateInstance<ActorBuffObject>();
        buff.DefineValues(name,hp,def,agi,turnCounter);
        return buff;
    }
    #endregion
}
