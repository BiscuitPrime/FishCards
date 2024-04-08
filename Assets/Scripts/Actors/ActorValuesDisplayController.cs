using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class ActorValuesDisplayController : MonoBehaviour
{
    [Header("Actor Values Elements")]
    [SerializeField] private GameObject _hpBubble;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private GameObject _defBubble;
    [SerializeField] private TextMeshProUGUI _defText;
    [SerializeField] private GameObject _agiBubble;
    [SerializeField] private TextMeshProUGUI _agiText;

    private ActorValuesController _controller;

    private void OnValidate()
    {
        Assert.IsNotNull(_hpBubble);
        Assert.IsNotNull(_hpText);
        Assert.IsNotNull(_defBubble);
        Assert.IsNotNull(_defText);
        Assert.IsNotNull(_agiBubble);
        Assert.IsNotNull(_agiText);

        _controller = GetComponent<ActorValuesController>();
    }

    private void Update()
    {
        _hpText.text = _controller.GetHP().ToString();
        _defText.text = _controller.GetDEF().ToString();
        _agiText.text = _controller.GetAGI().ToString();
    }
}
