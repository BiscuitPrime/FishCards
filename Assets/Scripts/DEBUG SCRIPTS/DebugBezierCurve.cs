using Fish.Utils;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DebugBezierCurve : MonoBehaviour
{
    [SerializeField] private int _maxNumPoints;
    [SerializeField] private Transform _startPoint, _endPoint, p1, p2;
    private LineRenderer _lineRenderer;

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
        _lineRenderer.positionCount = _maxNumPoints;
    }

    private void Update()
    {
        for(int i = 0; i < _maxNumPoints; i++)
        {
            _lineRenderer.SetPosition(i, BezierCurveHandler.GetPointOnBezierCurve(_startPoint.position,p1.position,p2.position,_endPoint.position,(float)i/_maxNumPoints));
        }
    }
}
