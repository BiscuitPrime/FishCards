using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBuffCreator : MonoBehaviour
{
    [SerializeField] private GameObject _buffTarget;
    [SerializeField] private ActorBuffObject _buff;

    [SerializeField] private bool CreateBuff;

    private void Update()
    {
        if (CreateBuff)
        {
            CreateBuff = false;
            var container = _buffTarget.AddComponent<BuffContainer>();
            ActorBuffObject buff = ScriptableObject.CreateInstance<ActorBuffObject>();
            buff.DefineValues("Buff", _buff.HP, _buff.DEF, _buff.AGI);
            container.AddBuff(buff);
        }
    }
}
