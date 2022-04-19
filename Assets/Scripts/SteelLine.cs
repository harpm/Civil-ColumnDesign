using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SteelLine : MonoBehaviour
{
    [SerializeField]
    public LineRenderer Renderer;

    [SerializeField]
    private CapsuleCollider _collider;

    [SerializeField]
    private Material _selectedMaterial;

    [SerializeField]
    private Material _mainMaterial;

    private Line _mainInstance;

    public Line MainInstance
    {
        get => _mainInstance;
        set => _mainInstance = value;
    }

    private Axis _curAxis;

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
        _collider.direction = (int) _curAxis;
        _collider.height = MainInstance.Length;
        
        if (_curAxis == Axis.X)
            _collider.center = Vector3.zero + new Vector3(MainInstance.Length / 2, 0, 0);
        else if (_curAxis == Axis.Y)
            _collider.center = Vector3.zero + new Vector3(0, MainInstance.Length / 2, 0);
        else
            _collider.center = Vector3.zero + new Vector3(0, 0, MainInstance.Length / 2);
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
        if (MainInstance.FirstPoint.X != MainInstance.EndPoint.X)
            _curAxis = Axis.X;
        else if (MainInstance.FirstPoint.Y != MainInstance.EndPoint.Y)
            _curAxis = Axis.Z;
        else
            _curAxis = Axis.Y;
    }

    private enum Axis
    {
        X = 0,
        Y = 1,
        Z = 2
    }
}
