using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoint
{
    [SerializeField]
    private GridPoint3D _gridPointPrefab3D;

    public int X;
    public float SpaceX;

    public int Y;
    public float SpaceY;

    public int S;
    public float SpaceS;

    public GridPoint3D Instance3D;
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
