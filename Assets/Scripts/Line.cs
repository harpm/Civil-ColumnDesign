using UnityEngine;

public class Line
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
    public SteelLine Instance2D;

    public Axis _curAxis { get; private set; }

    public float Length
    {
        get => (_firstPoint.Instance3D.transform.position - _endPoint.Instance3D.transform.position).magnitude;
    }

    public void SetAxis()
    {
        if (FirstPoint != null && EndPoint != null)
        {
            if (EndPoint.X == FirstPoint.X && EndPoint.Y == FirstPoint.Y && EndPoint.S != FirstPoint.S)
            {
                _curAxis = Axis.Y;
                if (_firstPoint.S > _endPoint.S)
                {
                    var temp = _firstPoint;
                    _firstPoint = _endPoint;
                    _endPoint = temp;
                }
            }
            else if (EndPoint.X == FirstPoint.X && EndPoint.Y != FirstPoint.Y && EndPoint.S == FirstPoint.S)
                _curAxis = Axis.Z;
            else if (EndPoint.X != FirstPoint.X && EndPoint.Y == FirstPoint.Y && EndPoint.S == FirstPoint.S)
                _curAxis = Axis.X;
        }
    }

    // Utilities

    public static Axis GetAxis(GridPoint f, GridPoint e)
    {

        if (e.X == f.X && e.Y == f.Y && e.S != f.S)
            return Axis.Y;
        else if (e.X == f.X && e.Y != f.Y && e.S == f.S)
            return Axis.Z;
        else if (e.X != f.X && e.Y == f.Y && e.S == f.S)
            return Axis.X;
        else
            return Axis.Invalid;

    }

    public enum Axis
    {
        Invalid,
        X,
        Y,
        Z
    }
}
