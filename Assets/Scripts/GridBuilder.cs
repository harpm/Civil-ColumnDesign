using System.Linq;
using TMPro;
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

    [SerializeField]
    private GameObject _xyzAxisPrefab;

    [SerializeField]
    private LabelFrame _labelFramePrefab;

    [SerializeField]
    private LabelFrame _spacingLabelPrefab;

    [Header("References")]
    [SerializeField]
    public Transform Parent3DGridPoints;

    [SerializeField]
    public Transform Parent3DGridLines;

    [SerializeField]
    public Transform Parent2DGridPoints;

    [SerializeField]
    public Transform Parent2DGridLines;

    [SerializeField]
    private TextMeshProUGUI _sliceTitle;

    private GridPoint[][][] GridData;
    private GridPoint2D[][] SliceData;

    public int NumberX = 0;
    public int NumberY = 0;
    public int NumberS = 0;

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
        NumberX = x;
        NumberY = y;
        NumberS = s;

        MainManager.Instance.MouseManager.Reset();
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
                    point.Instance3D = Instantiate(_gridPointPrefab3D, Parent3DGridPoints);

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

        // Place Axis prefab
        var pivot = Instantiate(_xyzAxisPrefab, Parent3DGridPoints);
        pivot.transform.localPosition = GridData[0][0][0].Instance3D.transform.localPosition;

        ShowSliceS(GridData[0][0].Length - 1);

        MainManager.Instance.CameraManager.TargetOnGrid3D();
        MainManager.Instance.CameraManager.TargetOnGrid2D();

        MainManager.Instance.MainWindow.StatusMessage("Grid Built successfully!", MainWindow.MessageType.Successful);
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
        var line = Instantiate(_gridLinePrefab, Parent3DGridLines);
        line.transform.localPosition = pose1;
        line.positionCount = 2;
        line.SetPosition(0, pose1);
        line.SetPosition(1, pose2);
    }

    public Vector3 GetCenter3D()
    {
        return Parent3DGridPoints.TransformPoint(GridData.Last().Last().Last().Instance3D.transform.localPosition / 2);
    }

    public Vector3 GetSize3D()
    {
        return GridData.Last().Last().Last().Instance3D.transform.localPosition;
    }

    #endregion

    #region 2D Grid

    public void ShowSliceX(int x)
    {
        ClearGrid2D();

        _sliceTitle.text = "X - " + (x + 1);
        SliceData = new GridPoint2D[GridData[x].Length][];
        for (int i = 0; i < GridData[x].Length; i++)
        {
            SliceData[i] = new GridPoint2D[GridData[x][i].Length];
            for (int j = 0; j < GridData[x][i].Length; j++)
            {
                var point = SliceData[i][j] = GridData[x][i][j].Instance2D = Instantiate(_gridPointPrefab2D, Parent2DGridPoints);

                point.X = i + 1;
                point.Y = j + 1;
                point.SpacingX = point.MainInstance.SpaceY;
                point.SpacingY = point.MainInstance.SpaceS;

                if (i > 0)
                    point.PreviousX = SliceData[i - 1][j];

                if (j > 0)
                    point.PreviousY = SliceData[i][j - 1];

                point.AdjustPosition();

                // Draw Lines

                if (i > 0)
                    DrawSingleLine2D(point.PreviousX.transform.position, point.transform.position);

                if (j > 0)
                    DrawSingleLine2D(point.PreviousY.transform.position, point.transform.position);

                // Draw Labels
                if (i == 0)
                {
                    var label = Instantiate(_labelFramePrefab, Parent2DGridPoints);
                    label.transform.localPosition = new Vector3(point.transform.localPosition.x - 3, point.transform.localPosition.y);
                    label.text.text = "S" + (j + 1);

                    if (j > 0)
                    {
                        var spacing = Instantiate(_spacingLabelPrefab, Parent2DGridPoints);
                        spacing.transform.localPosition = new Vector3(point.transform.localPosition.x - 2,
                            point.transform.localPosition.y - point.SpacingY / 2);
                        spacing.text.text = point.SpacingY + " m";
                    }
                }

                if (j == SliceData[i].Length - 1)
                {
                    var label = Instantiate(_labelFramePrefab, Parent2DGridPoints);
                    label.transform.localPosition = new Vector3(point.transform.localPosition.x, point.transform.localPosition.y + 3);
                    label.text.text = "Y" + (i + 1);

                    if (i > 0)
                    {
                        var spacing = Instantiate(_spacingLabelPrefab, Parent2DGridPoints);
                        spacing.transform.localPosition = new Vector3(point.transform.localPosition.x - point.SpacingX / 2,
                            point.transform.localPosition.y + 2);
                        spacing.text.text = point.SpacingX + " m";
                    }
                }

            }
        }
    }

    public void ShowSliceY(int y)
    {
        ClearGrid2D();
        _sliceTitle.text = "Y - " + (y + 1);

        SliceData = new GridPoint2D[GridData.Length][];
        for (int i = 0; i < GridData.Length; i++)
        {
            SliceData[i] = new GridPoint2D[GridData[i][y].Length];
            for (int j = 0; j < GridData[i][y].Length; j++)
            {
                var point = SliceData[i][j] = GridData[i][y][j].Instance2D = Instantiate(_gridPointPrefab2D, Parent2DGridPoints);

                point.X = i + 1;
                point.Y = j + 1;
                point.SpacingX = point.MainInstance.SpaceX;
                point.SpacingY = point.MainInstance.SpaceS;

                if (i > 0)
                    point.PreviousX = SliceData[i - 1][j];

                if (j > 0)
                    point.PreviousY = SliceData[i][j - 1];

                point.AdjustPosition();

                // Draw Lines

                if (i > 0)
                    DrawSingleLine2D(point.PreviousX.transform.position, point.transform.position);

                if (j > 0)
                    DrawSingleLine2D(point.PreviousY.transform.position, point.transform.position);

                // Draw Labels
                if (i == 0)
                {
                    var label = Instantiate(_labelFramePrefab, Parent2DGridPoints);
                    label.transform.localPosition = new Vector3(point.transform.localPosition.x - 3, point.transform.localPosition.y);
                    label.text.text = "S" + (j + 1);

                    if (j > 0)
                    {
                        var spacing = Instantiate(_spacingLabelPrefab, Parent2DGridPoints);
                        spacing.transform.localPosition = new Vector3(point.transform.localPosition.x - 2,
                            point.transform.localPosition.y - point.SpacingY / 2);
                        spacing.text.text = point.SpacingY + " m";
                    }
                }

                if (j == SliceData[i].Length - 1)
                {
                    var label = Instantiate(_labelFramePrefab, Parent2DGridPoints);
                    label.transform.localPosition = new Vector3(point.transform.localPosition.x, point.transform.localPosition.y + 3);
                    label.text.text = "X" + (i + 1);

                    if (i > 0)
                    {
                        var spacing = Instantiate(_spacingLabelPrefab, Parent2DGridPoints);
                        spacing.transform.localPosition = new Vector3(point.transform.localPosition.x - point.SpacingX / 2,
                            point.transform.localPosition.y + 2);
                        spacing.text.text = point.SpacingX + " m";
                    }
                }
            }
        }
    }

    public void ShowSliceS(int s)
    {
        ClearGrid2D();

        _sliceTitle.text = "Story - " + (s + 1);

        SliceData = new GridPoint2D[GridData.Length][];
        for (int i = 0; i < GridData.Length; i++)
        {
            SliceData[i] = new GridPoint2D[GridData[i].Length];
            for (int j = 0; j < GridData[i].Length; j++)
            {
                var point = SliceData[i][j] = GridData[i][j][s].Instance2D = Instantiate(_gridPointPrefab2D, Parent2DGridPoints);

                point.X = i + 1;
                point.Y = j + 1;
                point.SpacingX = point.MainInstance.SpaceX;
                point.SpacingY = point.MainInstance.SpaceY;

                if (i > 0)
                    point.PreviousX = SliceData[i - 1][j];

                if (j > 0)
                    point.PreviousY = SliceData[i][j - 1];

                point.AdjustPosition();

                // Draw Lines

                if (i > 0)
                    DrawSingleLine2D(point.PreviousX.transform.position, point.transform.position);

                if (j > 0)
                    DrawSingleLine2D(point.PreviousY.transform.position, point.transform.position);

                // Draw Labels
                if (i == 0)
                {
                    var label = Instantiate(_labelFramePrefab, Parent2DGridPoints);
                    label.transform.localPosition = new Vector3(point.transform.localPosition.x - 3, point.transform.localPosition.y);
                    label.text.text = "Y" + (j + 1);

                    if (j > 0)
                    {
                        var spacing = Instantiate(_spacingLabelPrefab, Parent2DGridPoints);
                        spacing.transform.localPosition = new Vector3(point.transform.localPosition.x - 2,
                            point.transform.localPosition.y - point.SpacingY / 2);
                        spacing.text.text = point.SpacingY + " m";
                    }
                }

                if (j == SliceData[i].Length - 1)
                {
                    var label = Instantiate(_labelFramePrefab, Parent2DGridPoints);
                    label.transform.localPosition = new Vector3(point.transform.localPosition.x, point.transform.localPosition.y + 3);
                    label.text.text = "X" + (i + 1);

                    if (i > 0)
                    {
                        var spacing = Instantiate(_spacingLabelPrefab, Parent2DGridPoints);
                        spacing.transform.localPosition = new Vector3(point.transform.localPosition.x - point.SpacingX / 2,
                            point.transform.localPosition.y + 2);
                        spacing.text.text = point.SpacingX + " m";
                    }
                }
            }
        }
    }

    private void DrawSingleLine2D(Vector3 pose1, Vector3 pose2)
    {
        var line = Instantiate(_gridLinePrefab, Parent2DGridLines);
        line.transform.localPosition = pose1;
        line.positionCount = 2;
        line.SetPosition(0, pose1);
        line.SetPosition(1, pose2);
    }

    public Vector3 GetCenter2D()
    {
        return Parent2DGridPoints.TransformPoint(SliceData.Last().Last().transform.localPosition / 2);
    }

    public Vector3 GetSize2D()
    {
        return SliceData.Last().Last().transform.localPosition;
    }

    public void ClearGrid2D()
    {
        // 2D
        var count = Parent2DGridPoints.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(Parent2DGridPoints.GetChild(i).gameObject);
        }

        count = Parent2DGridLines.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(Parent2DGridLines.GetChild(i).gameObject);
        }

        SliceData = default;
    }

    #endregion


    private void ClearGrid()
    {
        // 3D
        int count = Parent3DGridPoints.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(Parent3DGridPoints.GetChild(i).gameObject);
        }

        count = Parent3DGridLines.childCount;
        for (int i = 0; i < count; i++)
        {
            Destroy(Parent3DGridLines.GetChild(i).gameObject);
        }

        GridData = default;

        MainManager.Instance.MainWindow.StatusMessage("Cleared Grid!" , MainWindow.MessageType.Info);
    }
}
