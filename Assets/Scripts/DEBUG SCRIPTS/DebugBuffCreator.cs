using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugBuffCreator : MonoBehaviour
{
    [SerializeField] private GameObject _buffTarget;
    [SerializeField] private string _buffName;
    [SerializeField] private int _hp = 100,_def = 100,_agi = 100,_turnCounter = 1;

    [SerializeField] private bool CreateBuff;

    private void Update()
    {
        if (CreateBuff)
        {
            CreateBuff = false;
            BuffFactory.BuffActor(_buffTarget, _buffName, _hp,_def,_agi,_turnCounter);
        }
    }
}
