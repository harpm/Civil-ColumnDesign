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

    public float Length;
}
