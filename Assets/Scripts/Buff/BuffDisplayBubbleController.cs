using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// Script that will control the visual display of the buff in the form of a small bubble with the three values of the buff within it
/// </summary>
public class BuffDisplayBubbleController : MonoBehaviour
{
    #region VARIABLES
    [Header("Buff Bubbles")]
    [SerializeField] private GameObject _hpBubble;
    [SerializeField] private TextMeshProUGUI _hpBubbleText;
    [SerializeField] private GameObject _defBubble;
    [SerializeField] private TextMeshProUGUI _defBubbleText;
    [SerializeField] private GameObject _agiBubble;
    [SerializeField] private TextMeshProUGUI _agiBubbleText;
    #endregion

    private void Awake()
    {
        Assert.IsNotNull(_hpBubble);
        Assert.IsNotNull(_hpBubbleText);
        Assert.IsNotNull(_defBubble);
        Assert.IsNotNull(_defBubbleText);
        Assert.IsNotNull(_agiBubble);
        Assert.IsNotNull(_agiBubbleText);
    }

    /// <summary>
    /// Function that will update the Buff's display bubble, based on the inputted values of the buff
    /// </summary>
    /// <param name="hp">HP value</param>
    /// <param name="def">DEF value</param>
    /// <param name="agi">AGI value</param>
    public void SetDisplayBubbleValues(int hp, int def, int agi)
    {
        if (hp <= 0)
        {
            _hpBubble.SetActive(false);
        }
        if(def <= 0)
        {
            _defBubble.SetActive(false);
        }
        if(agi <= 0)
        {
            _agiBubble.SetActive(false);
        }
        _hpBubbleText.text = hp.ToString();
        _defBubbleText.text = def.ToString();
        _agiBubbleText.text = agi.ToString();
    }
}
