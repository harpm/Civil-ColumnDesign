using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SteelLine : MonoBehaviour
{
    [SerializeField]
    public LineRenderer Renderer;

    [SerializeField]
    private CapsuleCollider _collider;

    private Line _mainInstance;

    public Line MainInstance
    {
        get => _mainInstance;
        set
        {
            FirstPointInstance = value.FirstPoint.Instance3D;
            EndPointInstance = value.EndPoint.Instance3D;
            _mainInstance = value;
        }
    }

    public GridPoint3D FirstPointInstance;
    public GridPoint3D EndPointInstance;

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

    private void SetAxis()
    {
        if (FirstPointInstance.MainInstance.X != EndPointInstance.MainInstance.X)
            _curAxis = Axis.X;
        else if (FirstPointInstance.MainInstance.Y != EndPointInstance.MainInstance.Y)
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
