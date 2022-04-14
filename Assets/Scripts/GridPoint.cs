
public class GridPoint
{
    public int X;
    public float SpaceX;

    public int Y;
    public float SpaceY;

    public int S;
    public float SpaceS;

    private GridPoint3D _instancePoint3D;

    public GridPoint3D Instance3D
    {
        get => _instancePoint3D;
        
        set
        {
            _instancePoint3D = value;
            value.MainInstance = this;
        }
    }

    public GridPoint2D Instance2D;

    public GridPoint(int x, int y, int s, float spaceX, float spaceY, float spaceS)
    {
        X = x;
        Y = y;
        S = s;
        SpaceX = spaceX;
        SpaceY = spaceY;
        SpaceS = spaceS;
    }
}
