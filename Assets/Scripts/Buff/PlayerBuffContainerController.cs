using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script that defines a buff container applied only to the player.
/// The only addition is the fact that the buffs are to be displayed, unlike with other buff containers
/// </summary>
public class PlayerBuffContainerController : BuffContainer
{
    #region VARIABLES
    [Header("Buff Display : ")]
    [SerializeField] private GameObject _buffBubble;

    private Dictionary<BuffObject, GameObject> _bubblesDict;
    #endregion

    protected override void Awake()
    {
        _bubblesDict = new Dictionary<BuffObject, GameObject>();
    }

    protected override void UpdateBuffsList()
    {
        base.UpdateBuffsList();
        ClearAllBubbles();
        foreach (BuffObject buff in _buffs)
        {
            _bubblesDict.Add(buff, CreateNewBubble(buff as ActorBuffObject));
        }
    }

    public override void AddBuff(BuffObject buff)
    {
        base.AddBuff(buff);
        ClearAllBubbles();
        foreach (BuffObject tmp in _buffs)
        {
            _bubblesDict.Add(tmp, CreateNewBubble(tmp as ActorBuffObject));
        }
    }

    #region BUFF DISPLAY BUBBLES
    /// <summary>
    /// Function called by external scripts (ActorValuesController) that will update the visual indications of the buff bubble.
    /// </summary>
    /// <param name="buff">Buff to update</param>
    public void UpdateBubbleDisplay(ActorBuffObject buff)
    {
        if (_bubblesDict.ContainsKey(buff))
        {
            _bubblesDict[buff].GetComponent<BuffDisplayBubbleController>().SetDisplayBubbleValues(buff.HP, buff.DEF, buff.AGI);
        }
    }

    /// <summary>
    /// Creates a new bubble with the inputted buff
    /// </summary>
    /// <param name="buff"></param>
    /// <returns>Game object created in the scene</returns>
    private GameObject CreateNewBubble(ActorBuffObject buff)
    {
        GameObject bubble = Instantiate(_buffBubble);
        bubble.GetComponent<BuffDisplayBubbleController>().SetDisplayBubbleValues(buff.HP, buff.DEF, buff.AGI);
        return bubble;
    }

    /// <summary>
    /// Destroys all buff bubbles present in scene.
    /// </summary>
    private void ClearAllBubbles()
    {
        List<GameObject> objToDelete = new List<GameObject>(); 
        foreach(var buff in _bubblesDict.Keys)
        {
            GameObject tmp = _bubblesDict[buff];
            objToDelete.Add(tmp);
        }
        _bubblesDict.Clear();
        foreach (var obj in objToDelete)
        {
            Destroy(obj);
        }
    }
    #endregion
}
