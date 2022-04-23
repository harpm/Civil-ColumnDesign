using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class MouseManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField]
    [Range(0.5f, 5.0f)]
    private float _mouseSensitivity = 1.0f;

    [Header("References")]
    [SerializeField]
    [Range(0.01f, 1.0f)]
    private float _smoothTime = 0.3f;

    [SerializeField]
    private Camera _3DCamera;

    [SerializeField]
    private Camera _2DCamera;

    [SerializeField]
    private RawImage _3DRenderScreen;

    [SerializeField]
    private RawImage _2DRenderScreen;

    [SerializeField]
    private Transform _mainLineParentTransform;

    [SerializeField]
    private Transform _3DLineParentTransform;

    [SerializeField]
    private Transform _2DLineParentTransform;


    [Header("Prefabs")]
    [SerializeField]
    private Line _linePrefab;

    [SerializeField]
    private SteelLine _linePrefab3D;

    [SerializeField]
    private SteelLine2D _linePrefab2D;

    [HideInInspector]
    public Vector3 CurrentRotation;

    public Command CurrentCommand = Command.None;
    private DrawMode _currentDrawMode = DrawMode.None;

    [HideInInspector]
    public float RotateX;
    [HideInInspector]
    public float RotateY;

    private bool _rightClick;

    private Vector3 _rotVelocity = Vector3.zero;

    private GridPoint3D _hoveringPoint3D;
    private GridPoint2D _hoveringPoint2D;

    public List<Line> Lines = new List<Line>();

    public Line SelectedLine = null;

    private Line _drawingLine;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            CancelInProgressCmd();

        HandleRotation(KeyCode.Mouse1);
        HandleZoom();
        HandleHover();
        HandleCommand();
    }

    private void HandleCommand()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            var mPose = Input.mousePosition;
            switch (CurrentCommand)
            {
                case Command.None:
                    {
                        break;
                    }
                case Command.DrawLineFirstPoint:
                    {
                        if (_hoveringPoint3D != null)
                        {
                            _currentDrawMode = DrawMode.Frame3D;
                            _drawingLine = Instantiate(_linePrefab, _mainLineParentTransform);

                            _drawingLine.FirstPoint = _hoveringPoint3D.MainInstance;

                            StartDrawing();
                            CurrentCommand = Command.DrawLineEndPoint;
                        }
                        else if (_hoveringPoint2D != null)
                        {
                            _currentDrawMode = DrawMode.Frame2D;
                            _drawingLine = Instantiate(_linePrefab, _mainLineParentTransform);

                            _drawingLine.FirstPoint = _hoveringPoint2D.MainInstance;

                            StartDrawing();
                            CurrentCommand = Command.DrawLineEndPoint;
                        }
                        break;
                    }
                case Command.DrawLineEndPoint:
                    {
                        if (_drawingLine == null)
                            break;

                        if (_currentDrawMode == DrawMode.Frame3D && _hoveringPoint3D != null)
                        {
                            if (Lines.Any(l =>
                                l.FirstPoint == _drawingLine.FirstPoint &&
                                l._curAxis == Line.GetAxis(_drawingLine.FirstPoint, _hoveringPoint3D.MainInstance)))
                            {
                                MainManager.Instance.MainWindow.StatusMessage("Cannot draw on another line", MainWindow.MessageType.Error);
                                break;
                            }

                            _drawingLine.EndPoint = _hoveringPoint3D.MainInstance;
                            if (_drawingLine.Valid)
                            {
                                EndDrawing();
                                CurrentCommand = Command.None;
                                _currentDrawMode = DrawMode.None;
                                Lines.Add(_drawingLine);
                                _drawingLine.SetAxis();
                            }
                        }
                        else if (_currentDrawMode == DrawMode.Frame2D && _hoveringPoint2D != null)
                        {
                            if (Lines.Any(l =>
                                l.FirstPoint == _drawingLine.FirstPoint &&
                                l._curAxis == Line.GetAxis(_drawingLine.FirstPoint, _hoveringPoint2D.MainInstance)))
                            {
                                MainManager.Instance.MainWindow.StatusMessage("Cannot draw on another line", MainWindow.MessageType.Error);
                                break;
                            }

                            _drawingLine.EndPoint = _hoveringPoint2D.MainInstance;
                            if (_drawingLine.Valid)
                            {
                                EndDrawing();
                                CurrentCommand = Command.None;
                                _currentDrawMode = DrawMode.None;
                                Lines.Add(_drawingLine);
                                _drawingLine.SetAxis();
                            }
                            else
                                MainManager.Instance.MainWindow.StatusMessage("Invalid Line", MainWindow.MessageType.Error);
                        }

                        break;
                    }
            }
        }
    }

    private void StartDrawing()
    {
        if (_drawingLine.FirstPoint == null)
            return;

        switch (_currentDrawMode)
        {
            case DrawMode.Frame3D:
                {
                    _drawingLine.Instance3D = Instantiate(_linePrefab3D, _3DLineParentTransform);
                    _drawingLine.Instance3D.transform.position = _hoveringPoint3D.transform.position;
                    _drawingLine.Instance3D.Renderer.positionCount = 2;

                    _drawingLine.Instance3D.Renderer.SetPosition(0,
                        _drawingLine.FirstPoint.Instance3D.transform.position);
                    _drawingLine.Instance3D.Renderer.SetPosition(1,
                        _drawingLine.FirstPoint.Instance3D.transform.position);
                    break;
                }
            case DrawMode.Frame2D:
                {
                    _drawingLine.Instance2D = Instantiate(_linePrefab2D, _2DLineParentTransform);
                    _drawingLine.Instance2D.transform.position = _hoveringPoint2D.transform.position;

                    _drawingLine.Instance2D.Renderer.positionCount = 2;

                    _drawingLine.Instance2D.Renderer.SetPosition(0,
                        _drawingLine.FirstPoint.Instance2D.transform.position);
                    _drawingLine.Instance2D.Renderer.SetPosition(1,
                        _drawingLine.FirstPoint.Instance2D.transform.position);
                    break;
                }
        }
    }

    private void PreviewDrawing()
    {
        switch (_currentDrawMode)
        {
            case DrawMode.Frame3D:
                {
                    _drawingLine.Instance3D.Renderer.SetPosition(1,
                        _hoveringPoint3D.transform.position);
                    break;
                }
            case DrawMode.Frame2D:
                {
                    _drawingLine.Instance2D.Renderer.SetPosition(1,
                        _hoveringPoint2D.transform.position);
                    break;
                }
        }
    }

    private void EndDrawing()
    {
        if (_drawingLine.EndPoint == null)
            return;

        switch (_currentDrawMode)
        {
            case DrawMode.Frame3D:
                {
                    _drawingLine.Instance3D.Renderer.SetPosition(1,
                        _drawingLine.EndPoint.Instance3D.transform.position);

                    Sync2DInstance();
                    _drawingLine.FinalizeDrawing();

                    break;
                }
            case DrawMode.Frame2D:
                {
                    _drawingLine.Instance2D.Renderer.SetPosition(1,
                        _drawingLine.EndPoint.Instance2D.transform.position);
                    Sync3DInstance();
                    _drawingLine.FinalizeDrawing();
                    break;
                }
        }
    }

    private void Sync3DInstance()
    {
        _drawingLine.Instance3D = Instantiate(_linePrefab3D, _3DLineParentTransform);
        _drawingLine.Instance3D.transform.position = _drawingLine.FirstPoint.Instance3D.transform.position;

        _drawingLine.Instance3D.Renderer.positionCount = 2;

        _drawingLine.Instance3D.Renderer.SetPosition(0,
            _drawingLine.FirstPoint.Instance3D.transform.position);
        _drawingLine.Instance3D.Renderer.SetPosition(1,
            _drawingLine.EndPoint.Instance3D.transform.position);
    }

    private void Sync2DInstance()
    {
        if (_drawingLine.FirstPoint.Instance2D == null
            || _drawingLine.EndPoint.Instance2D == null)
            return;

        _drawingLine.Instance2D = Instantiate(_linePrefab2D, _2DLineParentTransform);
        _drawingLine.Instance2D.transform.position = _drawingLine.FirstPoint.Instance2D.transform.position;

        _drawingLine.Instance2D.Renderer.positionCount = 2;

        _drawingLine.Instance2D.Renderer.SetPosition(0,
            _drawingLine.FirstPoint.Instance2D.transform.position);
        _drawingLine.Instance2D.Renderer.SetPosition(1,
            _drawingLine.EndPoint.Instance2D.transform.position);

    }

    private void HandleHover()
    {
        var mPose = Input.mousePosition;
        if (TryHitRenderer(mPose, _3DRenderScreen, out Vector2 localPose))
        {
            if (TryHitPoint(localPose, _3DCamera, out GridPoint3D point))
            {
                _hoveringPoint3D = point;
                _hoveringPoint3D.Hover();
                if (CurrentCommand == Command.DrawLineEndPoint && _currentDrawMode == DrawMode.Frame3D)
                    PreviewDrawing();
            }
            else
            {
                if (_hoveringPoint3D != null)
                {
                    _hoveringPoint3D.EndHover();
                    _hoveringPoint3D = null;
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (SelectedLine != null)
                    DeselectLine();
                else if (TryHitLine(localPose, _3DCamera, out SteelLine sl))
                {
                    SelectLine(sl.MainInstance);
                }
            }
        }
        else if (TryHitRenderer(mPose, _2DRenderScreen, out Vector2 localPose2D))
        {
            if (TryHitPoint(localPose2D, _2DCamera, out GridPoint2D point))
            {
                _hoveringPoint2D = point;
                _hoveringPoint2D.Hover();
                if (CurrentCommand == Command.DrawLineEndPoint && _currentDrawMode == DrawMode.Frame2D)
                    PreviewDrawing();
            }
            else
            {
                if (_hoveringPoint2D != null)
                {
                    _hoveringPoint2D.EndHover();
                    _hoveringPoint2D = null;
                }
            }

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (SelectedLine != null)
                    DeselectLine();
                else if (TryHitLine(localPose2D, _2DCamera, out SteelLine2D sl))
                {
                    SelectLine(sl.MainInstance);
                }
            }

        }
        else
        {
            if (_hoveringPoint3D != null)
            {
                _hoveringPoint3D.EndHover();
                _hoveringPoint3D = null;
            }

            if (_hoveringPoint2D != null)
            {
                _hoveringPoint2D.EndHover();
                _hoveringPoint2D = null;
            }
        }
    }

    private void HandleZoom()
    {
        var mPose = Input.mousePosition;
        if (TryHitRenderer(mPose, _3DRenderScreen, out _))
        {
            var delta = Input.mouseScrollDelta.y;
            _3DCamera.transform.position += delta * _3DCamera.transform.forward;
        }
    }

    private void HandleRotation(KeyCode kc)
    {
        var mPose = Input.mousePosition;

        if (Input.GetKeyDown(kc))
        {
            if (TryHitRenderer(mPose, _3DRenderScreen, out _))
                _rightClick = true;
        }
        else if (Input.GetKey(kc))
        {
            if (_rightClick)
                RotateByMouse();
        }
        else if (Input.GetKeyUp(kc))
        {
            _rightClick = false;
        }
        else
        {
            RotateByMouse();
        }
    }

    private void RotateByMouse()
    {
        float axisX = 0;
        float axisY = 0;

        if (_rightClick)
        {
            axisX = Input.GetAxis("Mouse X");
            axisY = -Input.GetAxis("Mouse Y");
        }
        else if (_rotVelocity == Vector3.zero)
            return;


        RotateX += axisY * _mouseSensitivity;
        RotateY += axisX * _mouseSensitivity;

        RotateX = Mathf.Clamp(RotateX, -45, +45);
        var nextRotation = new Vector2(RotateX, RotateY);

        CurrentRotation = Vector3.SmoothDamp(CurrentRotation, nextRotation, ref _rotVelocity, _smoothTime);
        _3DCamera.transform.localEulerAngles = CurrentRotation;

        var center = MainManager.Instance.GridBuilder.GetCenter3D();
        _3DCamera.transform.position =
            center - (_3DCamera.transform.position - center).magnitude * _3DCamera.transform.forward;
    }

    private bool TryHitRenderer(Vector3 pose, RawImage renderer, out Vector2 localPose)
    {
        bool res = false;
        localPose = -Vector3.one;

        var ray = Camera.main.ScreenPointToRay(pose);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null && hit.transform == renderer.transform)
            {
                res = true;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(renderer.rectTransform, pose, Camera.main,
                    out localPose);

                localPose = new Vector2(localPose.x / renderer.texture.width, localPose.y / renderer.texture.height);
            }
        }

        return res;
    }

    private bool TryHitLine(Vector3 localPose, Camera camera, out SteelLine line)
    {
        bool res = false;
        line = null;

        localPose = new Vector3(localPose.x * camera.pixelWidth, localPose.y * camera.pixelHeight);
        var ray = camera.ScreenPointToRay(localPose);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null && hit.collider.TryGetComponent(out line))
                res = true;
        }


        return res;
    }

    private bool TryHitLine(Vector3 localPose, Camera camera, out SteelLine2D line)
    {
        bool res = false;
        line = null;

        localPose = new Vector3(localPose.x * camera.pixelWidth, localPose.y * camera.pixelHeight);
        var ray = camera.ScreenPointToRay(localPose);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null && hit.collider.TryGetComponent(out line))
                res = true;
        }


        return res;
    }

    private bool TryHitPoint(Vector3 localPose, Camera camera, out GridPoint3D point)
    {
        bool res = false;
        point = null;

        localPose = new Vector3(localPose.x * camera.pixelWidth, localPose.y * camera.pixelHeight);
        var ray = camera.ScreenPointToRay(localPose);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider != null && hit.collider.TryGetComponent(out point))
                res = true;
        }

        return res;
    }

    private bool TryHitPoint(Vector3 localPose, Camera camera, out GridPoint2D point)
    {
        bool res = false;
        point = null;

        localPose = new Vector3(localPose.x * camera.pixelWidth, localPose.y * camera.pixelHeight);
        var ray = camera.ScreenPointToRay(localPose);
        if (Physics.Raycast(ray, out RaycastHit hit))
            if (hit.collider != null && hit.collider.TryGetComponent(out point))
                res = true;

        return res;
    }

    private void SelectLine(Line line)
    {
        SelectedLine = line;
        MainManager.Instance.InspectorWindow.SelectLine(line);
        SelectedLine.Select();

        MainManager.Instance.MainWindow.StatusMessage("Steel Line Selected!", MainWindow.MessageType.Info);
    }

    private void DeselectLine()
    {
        SelectedLine.Deselect();
        MainManager.Instance.InspectorWindow.DeselectLine();
        SelectedLine = null;
        MainManager.Instance.MainWindow.StatusMessage("Steel Line Deselected!", MainWindow.MessageType.Info);
    }

    public enum Command
    {
        None = 0,
        DrawLineFirstPoint = 1,
        DrawLineEndPoint = 2,
        PutLine = 3,
        PutLineAll = 2
    }

    private enum DrawMode
    {
        None,
        Frame3D,
        Frame2D
    }

    public void CancelInProgressCmd()
    {
        switch (CurrentCommand)
        {
            case Command.DrawLineFirstPoint:
                {
                    break;
                }

            case Command.DrawLineEndPoint:
                {
                    if (_currentDrawMode == DrawMode.Frame3D)
                    {
                        Destroy(_drawingLine.Instance3D.gameObject);
                    }
                    else if (_currentDrawMode == DrawMode.Frame2D)
                    {
                        Destroy(_drawingLine.Instance2D.gameObject);
                    }

                    Destroy(_drawingLine.gameObject);
                    _drawingLine = null;
                    CurrentCommand = Command.None;
                    _currentDrawMode = DrawMode.None;

                    break;
                }
        }
    }

    public void Reset()
    {
        CancelInProgressCmd();
        foreach (var t in Lines)
        {
            if (t.Instance2D != null)
                Destroy(t.Instance2D.gameObject);

            Destroy(t.Instance3D.gameObject);
        }

        Lines = new List<Line>();
    }

    public void DeleteSteelLine(Line line)
    {
        Lines.Remove(line);

        if (line.Instance2D != null)
        {
            Destroy(line.Instance2D.gameObject);
        }

        Destroy(line.Instance3D.gameObject);
        Destroy(line.gameObject);

        if (!Lines.Exists(l => (l.FirstPoint == line.EndPoint || l.EndPoint == line.EndPoint) &&
                               (l._curAxis == Line.Axis.Nz || l._curAxis == Line.Axis.Z)))
        {
            line.EndPoint.HasColumn = false;
        }
    }

    public void ClearSteelLines()
    {
        var count = Lines.Count;
        for (int i = 0; i < count; i++)
        {
            DeleteSteelLine(Lines[i]);
        }

        Lines = new List<Line>();
    }
}
