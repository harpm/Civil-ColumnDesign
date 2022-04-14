using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField]
    private GridPoint3D _gridPointPrefab3D;

    [SerializeField]
    private GridPoint2D _gridPointPrefab2D;

    [SerializeField]
    private LineRenderer _gridLinePrefab;

    [Header("References")]
    [SerializeField]
    public Transform Parent3DGrid;

    [SerializeField]
    public Transform Parent2DGrid;

    private GridPoint[][][] GridData;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    #region 3D Grid

    public void GenerateGrid(int x, int y, int s, float[] xSpaces, float[] ySpaces, float[] sSpaces)
    {
        GridData = new GridPoint[x][][];
        for (int i = 0; i < x; i++)
        {
            GridData[i] = new GridPoint[y][];
            for (int j = 0; j < y; j++)
            {
                GridData[i][j] = new GridPoint[s];
                for (int k = 0; k < s; k++)
                {
                    var point = GridData[i][j][k] =
                        new GridPoint(i + 1, j + 1, k + 1, xSpaces[i], ySpaces[j], sSpaces[k]);
                    point.Instance3D = Instantiate(_gridPointPrefab3D, Parent3DGrid);

                    if (i > 0)
                        point.Instance3D.PreviousX = GridData[i - 1][j][k].Instance3D;
                    if (j > 0)
                        point.Instance3D.PreviousY = GridData[i - 1][j - 1][k].Instance3D;
                    if (k > 0)
                        point.Instance3D.PreviousS = GridData[i][j][k - 1].Instance3D;

                    point.Instance3D.AdjustPositions();
                }
            }
        }
    }

    #endregion

    #region 2D Grid



    #endregion
}
