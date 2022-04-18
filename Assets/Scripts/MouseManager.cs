using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MouseManager : MonoBehaviour
{
    [SerializeField]
    [Range(0.5f, 5.0f)]
    private float _mouseSensitivity = 1.0f;

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

    public Vector3 CurrentRotation;

    public float RotateX;
    public float RotateY;

    private bool _rightClick;

    private Vector3 _rotVelocity = Vector3.zero;

    private GridPoint3D _hoveringPoint3D;
    private GridPoint2D _hoveringPoint2D;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleRotation(KeyCode.Mouse1);
        HandleZoom();
        HandleHover();
    }

    private void HandleHover()
    {
        var mPose = Input.mousePosition;
        print("M Pose: " + mPose);
        if (TryHitRenderer(mPose, _3DRenderScreen, out Vector2 localPose))
        {
            print("Local Pose: " + localPose);
            if (TryHitPoint(localPose, _3DCamera, out GridPoint3D point))
            {
                _hoveringPoint3D = point;
                _hoveringPoint3D.Hover();
            }
            else
            {
                if (_hoveringPoint3D != null)
                {
                    _hoveringPoint3D.EndHover();
                    _hoveringPoint3D = null;
                }
            }
        }
        else if (TryHitRenderer(mPose, _2DRenderScreen, out Vector2 localPose2D))
        {
            print("Local pose2D: " + localPose2D);
            if (TryHitPoint(localPose2D, _2DCamera, out GridPoint2D point))
            {
                _hoveringPoint2D = point;
                _hoveringPoint2D.Hover();
            }
            else
            {
                if (_hoveringPoint2D != null)
                {
                    _hoveringPoint2D.EndHover();
                    _hoveringPoint2D = null;
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

    public enum Command
    {
        DrawLine,
        PutLine,
        PutLineAll
    }
}
