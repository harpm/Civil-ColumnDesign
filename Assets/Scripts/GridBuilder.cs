using System.Linq;
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
        GenerateGrid(3, 3, 3, new float[] { 10, 10, 10 }, new float[] { 10, 10, 10 }, new float[] { 8, 8, 8 });
    }

    // Update is called once per frame
    void Update()
    {

    }

    #region 3D Grid

    public void GenerateGrid(int x, int y, int s, float[] xSpaces, float[] ySpaces, float[] sSpaces)
    {
        ClearGrid();

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
                        point.Instance3D.PreviousY = GridData[i][j - 1][k].Instance3D;

                    if (k > 0)
                        point.Instance3D.PreviousS = GridData[i][j][k - 1].Instance3D;


                    point.Instance3D.AdjustPositions();
                }
            }
        }

        DrawGridLines3D();
        ShowSliceS(GridData[0][0].Length - 1);
        MainManager.Instance.CameraManager.TargetOnGrid3D();
    }


    private void DrawGridLines3D()
    {
        for (int i = 0; i < GridData.Length; i++)
        {
            for (int j = 0; j < GridData[i].Length; j++)
            {
                for (int k = 0; k < GridData[i][j].Length; k++)
                {
                    var point = GridData[i][j][k];
                    if (i > 0)
                        DrawGridLine(GridData[i - 1][j][k].Instance3D.transform.position,
                            point.Instance3D.transform.position);

                    if (j > 0)
                        DrawGridLine(GridData[i][j - 1][k].Instance3D.transform.position,
                            point.Instance3D.transform.position);

                    if (k > 0)
                        DrawGridLine(GridData[i][j][k - 1].Instance3D.transform.position,
                            point.Instance3D.transform.position);

                }
            }
        }
    }

    private void DrawGridLine(Vector3 pose1, Vector3 pose2)
    {
        var line = Instantiate(_gridLinePrefab, Parent3DGrid);
        line.transform.localPosition = pose1;
        line.positionCount = 2;
        line.SetPosition(0, pose1);
        line.SetPosition(1, pose2);
    }

    public Vector3 GetCenter3D()
    {
        return Parent3DGrid.TransformPoint(GridData.Last().Last().Last().Instance3D.transform.localPosition / 2);
    }

    public Vector3 GetSize3D()
    {
        return GridData.Last().Last().Last().Instance3D.transform.localPosition;
    }

    #endregion

    #region 2D Grid

    private void ShowSliceX(int x)
    {
        for (int i = 0; i < GridData[x].Length; i++)
        {
            for (int j = 0; j < GridData[x][i].Length; j++)
            {
                var point = GridData[x][i][j].Instance2D = Instantiate(_gridPointPrefab2D, Parent2DGrid);

                point.X = i + 1;
                point.Y = j + 1;
                point.SpacingX = point.MainInstance.SpaceY;
                point.SpacingY = point.MainInstance.SpaceS;

                if (i > 0)
                    point.PreviousX = GridData[x][i - 1][j].Instance2D;

                if (j > 0)
                    point.PreviousY = GridData[x][i][j - 1].Instance2D;

                point.AdjustPosition();

                if (i > 0)
                    DrawSingleLine2D(point.PreviousX.transform.localPosition, point.transform.localPosition);

                if (j > 0)
                    DrawSingleLine2D(point.PreviousY.transform.localPosition, point.transform.localPosition);
            }
        }
    }

    private void ShowSliceY(int y)
    {
        for (int i = 0; i < GridData.Length; i++)
        {
            for (int j = 0; j < GridData[i][y].Length; j++)
            {
                var point = GridData[i][y][j].Instance2D = Instantiate(_gridPointPrefab2D, Parent2DGrid);

                point.X = i + 1;
                point.Y = j + 1;
                point.SpacingX = point.MainInstance.SpaceX;
                point.SpacingY = point.MainInstance.SpaceS;

                if (i > 0)
                    point.PreviousX = GridData[i - 1][y][j].Instance2D;

                if (j > 0)
                    point.PreviousY = GridData[i][y][j - 1].Instance2D;

                point.AdjustPosition();

                if (i > 0)
                    DrawSingleLine2D(point.PreviousX.transform.localPosition, point.transform.localPosition);

                if (j > 0)
                    DrawSingleLine2D(point.PreviousY.transform.localPosition, point.transform.localPosition);
            }
        }
    }

    private void ShowSliceS(int s)
    {
        for (int i = 0; i < GridData.Length; i++)
        {
            for (int j = 0; j < GridData[i].Length; j++)
            {
                var point = GridData[i][j][s].Instance2D = Instantiate(_gridPointPrefab2D, Parent2DGrid);

                point.X = i + 1;
                point.Y = j + 1;
                point.SpacingX = point.MainInstance.SpaceY;
                point.SpacingY = point.MainInstance.SpaceS;

                if (i > 0)
                    point.PreviousX = GridData[i - 1][j][s].Instance2D;

                if (j > 0)
                    point.PreviousY = GridData[i][j - 1][s].Instance2D;

                point.AdjustPosition();

                if (i > 0)
                    DrawSingleLine2D(point.PreviousX.transform.localPosition, point.transform.localPosition);

                if (j > 0)
                    DrawSingleLine2D(point.PreviousY.transform.localPosition, point.transform.localPosition);
            }
        }
    }

    private void DrawSingleLine2D(Vector3 pose1, Vector3 pose2)
    {
        var line = Instantiate(_gridLinePrefab, Parent2DGrid);
        line.transform.localPosition = pose1;
        line.positionCount = 2;
        line.SetPosition(0, pose1);
        line.SetPosition(1, pose2);
    }

    #endregion


    private void ClearGrid()
    {
        int count = Parent3DGrid.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(Parent3DGrid.GetChild(i).gameObject);
        }

        count = Parent2DGrid.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(Parent2DGrid.GetChild(i).gameObject);
        }

        GridData = default;
    }
}
