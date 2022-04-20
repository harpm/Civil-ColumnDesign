using UnityEngine;

public class Line : MonoBehaviour
{
    private GridPoint _firstPoint;
    private GridPoint _endPoint;

    public bool Valid { get; private set; }

    public GridPoint FirstPoint
    {
        get => _firstPoint;

        set => _firstPoint = value;
    }

    public GridPoint EndPoint
    {
        get => _endPoint;

        set
        {
            Valid = false;
            if (value.X == FirstPoint.X && value.Y == FirstPoint.Y && value.S != FirstPoint.S)
                Valid = true;
            else if (value.X == FirstPoint.X && value.Y != FirstPoint.Y && value.S == FirstPoint.S)
                Valid = true;
            else if (value.X != FirstPoint.X && value.Y == FirstPoint.Y && value.S == FirstPoint.S)
                Valid = true;

            if (Valid)
                _endPoint = value;
        }
    }

    public SteelLine Instance3D;
    public SteelLine2D Instance2D;

    public Axis _curAxis { get; private set; }

    public float Length => 
        (_firstPoint.Instance3D.transform.position - _endPoint.Instance3D.transform.position).magnitude;

    public float AliveForces = 0.0f;
    public float DeadForces = 0.0f;

    public float Inertia = 0.0f;

    public ConnectionType HigherConnection;
    public ConnectionType LowerConnection;

    public bool IsOnGround = false;


    public void SetAxis()
    {
        if (FirstPoint != null && EndPoint != null)
        {
            if (EndPoint.X == FirstPoint.X && EndPoint.Y == FirstPoint.Y && EndPoint.S != FirstPoint.S)
            {
                _curAxis = EndPoint.S > FirstPoint.S ? Axis.Z : Axis.Nz;

                if (_firstPoint.S > _endPoint.S)
                {
                    var temp = _firstPoint;
                    _firstPoint = _endPoint;
                    _endPoint = temp;

                    Instance3D.transform.position = _firstPoint.Instance3D.transform.position;

                    if (Instance2D != null)
                        Instance2D.transform.position = _firstPoint.Instance2D.transform.position;

                    RedrawLines();
                }
            }
            else if (EndPoint.X == FirstPoint.X && EndPoint.Y != FirstPoint.Y && EndPoint.S == FirstPoint.S)
            {
                _curAxis = EndPoint.Y > FirstPoint.Y ? Axis.Y : Axis.Ny;

                if (_firstPoint.Y > _endPoint.Y)
                {
                    var temp = _firstPoint;
                    _firstPoint = _endPoint;
                    _endPoint = temp;

                    Instance3D.transform.position = _firstPoint.Instance3D.transform.position;

                    if (Instance2D != null)
                        Instance2D.transform.position = _firstPoint.Instance2D.transform.position;

                    RedrawLines();
                }
            }
            else if (EndPoint.X != FirstPoint.X && EndPoint.Y == FirstPoint.Y && EndPoint.S == FirstPoint.S)
            {
                _curAxis = EndPoint.X > FirstPoint.X ? Axis.X : Axis.Nx;

                if (_firstPoint.X > _endPoint.X)
                {
                    var temp = _firstPoint;
                    _firstPoint = _endPoint;
                    _endPoint = temp;

                    Instance3D.transform.position = _firstPoint.Instance3D.transform.position;

                    if (Instance2D != null)
                        Instance2D.transform.position = _firstPoint.Instance2D.transform.position;
                    RedrawLines();
                }
            }
            else
                _curAxis = Axis.Invalid;
        }
        else
            _curAxis = Axis.Invalid;

        if (FirstPoint.S == 1 && EndPoint.S == 1)
            IsOnGround = true;
        else
            IsOnGround = false;
    }

    public void FinalizeDrawing()
    {
        if (Instance2D != null)
        {
            Instance2D.MainInstance = this;
            Instance2D.AddCollider();
        }

        Instance3D.MainInstance = this;
        Instance3D.AddCollider();

        FirstPoint.IsEmpty = false;
        EndPoint.IsEmpty = false;
    }

    public void Select()
    {
        if (Instance2D != null)
            Instance2D.Select();

        Instance3D.Select();
    }

    public void Deselect()
    {
        if (Instance2D != null)
            Instance2D.Deselect();

        Instance3D.Deselect();

    }

    private void RedrawLines()
    {
        if (Instance2D != null)
        {
            Instance2D.Renderer.SetPosition(0, FirstPoint.Instance2D.transform.position);
            Instance2D.Renderer.SetPosition(1, EndPoint.Instance2D.transform.position);
        }

        Instance3D.Renderer.SetPosition(0, FirstPoint.Instance3D.transform.position);
        Instance3D.Renderer.SetPosition(1, EndPoint.Instance3D.transform.position);
    }

    // Utilities

    public static Axis GetAxis(GridPoint f, GridPoint e)
    {

        if (e.X == f.X && e.Y == f.Y && e.S != f.S)
            return e.Y > f.Y ? Axis.Y : Axis.Ny;
        else if (e.X == f.X && e.Y != f.Y && e.S == f.S)
            return e.S > f.S ? Axis.Z : Axis.Nz;
        else if (e.X != f.X && e.Y == f.Y && e.S == f.S)
            return e.X > f.X ? Axis.X : Axis.Nx;
        else
            return Axis.Invalid;

    }

    public enum ConnectionType
    {
        FixedConnection,
        PinConnection,
        ConsoleConnection,
        RollerSupportConnection
    }

    public enum Axis
    {
        Invalid,
        X,
        Y,
        Z,
        Nx,
        Ny,
        Nz
    }
}
