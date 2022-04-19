using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SteelLine2D : MonoBehaviour
{
    [SerializeField]
    public LineRenderer Renderer;

    [SerializeField]
    private CapsuleCollider _collider;

    public GridPoint2D FirstPointInstance;
    public GridPoint2D EndPointInstance;

    private Axis _curAxis;

    private Line _mainInstance;

    public Line MainInstance
    {
        get => _mainInstance;
        set
        {
            FirstPointInstance = value.FirstPoint.Instance2D;
            EndPointInstance = value.EndPoint.Instance2D;
            _mainInstance = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddCollider()
    {
        SetAxis();
        _collider.direction = (int)_curAxis;
        _collider.height = MainInstance.Length;

        if (_curAxis == Axis.X)
            _collider.center = Vector3.zero + new Vector3(MainInstance.Length / 2, 0, 0);
        else if (_curAxis == Axis.Y)
            _collider.center = Vector3.zero + new Vector3(0, MainInstance.Length / 2, 0);
    }

    private void SetAxis()
    {
        if (FirstPointInstance.X != EndPointInstance.X)
            _curAxis = Axis.X;
        else
            _curAxis = Axis.Y;
    }

    private enum Axis
    {
        X = 0,
        Y = 1
    }
}
