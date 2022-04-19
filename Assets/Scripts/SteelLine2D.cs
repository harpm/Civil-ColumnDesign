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

    [SerializeField]
    private Material _selectedMaterial;

    [SerializeField]
    private Material _mainMaterial;

    private Axis _curAxis;

    private Line _mainInstance;

    public Line MainInstance
    {
        get => _mainInstance;
        set => _mainInstance = value;
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

    public void Select()
    {
        Renderer.material = _selectedMaterial;
    }

    public void Deselect()
    {
        Renderer.material = _mainMaterial;
    }

    private void SetAxis()
    {
        if (MainInstance.FirstPoint.Instance2D.X != MainInstance.EndPoint.Instance2D.X)
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
