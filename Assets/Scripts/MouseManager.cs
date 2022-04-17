using UnityEngine;
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
        // HandleRaycast();
    }

    private void HandleRaycast()
    {
        var mPose = Input.mousePosition;
        var ray = Camera.main.ScreenPointToRay(mPose);

        if (TryHitRenderer(ray, _3DRenderScreen, out Vector3 localPose))
        {
            if (TryHitPoint3D(localPose, _3DCamera, out RaycastHit hit))
            {
                if (hit.collider.TryGetComponent(out _hoveringPoint3D))
                {
                    _hoveringPoint3D.Hover();
                }
                else if (_hoveringPoint3D != null)
                        _hoveringPoint3D.EndHover();

            }
            else if (_hoveringPoint3D != null)
                _hoveringPoint3D.EndHover();
        }
        else if (_hoveringPoint3D != null)
            _hoveringPoint3D.EndHover();

        if (TryHitRenderer(ray, _2DRenderScreen, out localPose))
        {
            if (TryHitPoint2D(localPose, _2DCamera, out RaycastHit2D hit))
            {
                if (hit.collider.TryGetComponent(out _hoveringPoint2D))
                {
                    _hoveringPoint2D.Hover();
                }
                else if (_hoveringPoint2D != null)
                    _hoveringPoint2D.EndHover();
            }
            else if (_hoveringPoint3D != null)
                _hoveringPoint3D.EndHover();

        }
        else if(_hoveringPoint3D != null)
            _hoveringPoint3D.EndHover();
    }

    private void HandleZoom()
    {
        var mPose = Input.mousePosition;
        var ray = Camera.main.ScreenPointToRay(mPose);
        if (TryHitRenderer(ray, _3DRenderScreen, out Vector3 localPose))
        {
            var delta = Input.mouseScrollDelta.y;
            _3DCamera.transform.position += delta * _3DCamera.transform.forward;
        }
    }

    private void HandleRotation(KeyCode kc)
    {
        var mPose = Input.mousePosition;
        var ray = Camera.main.ScreenPointToRay(mPose);

        if (Input.GetKeyDown(kc))
        {
            if (TryHitRenderer(ray, _3DRenderScreen, out Vector3 localPose))
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

    private bool TryHitRenderer(Ray ray, RawImage renderer, out Vector3 localPose)
    {
        bool res = false;
        localPose = -Vector3.one;

        var hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider != null && hit.collider.GetComponent<RawImage>() == renderer)
        {
            print(hit.transform.name);
            res = true;
            localPose = renderer.transform.InverseTransformPoint(hit.point);
            localPose = new Vector3(localPose.x / renderer.mainTexture.width,
                localPose.y / renderer.mainTexture.height);
        }

        return res;
    }

    private bool TryHitPoint3D(Vector3 localPose, Camera camera, out RaycastHit hit)
    {
        localPose = new Vector3(localPose.x * camera.pixelWidth, localPose.y * camera.pixelHeight);

        bool res = false;

        hit = default;
        var ray = camera.ScreenPointToRay(localPose);
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
                res = true;
        }


        return res;
    }

    private bool TryHitPoint2D(Vector3 localPose, Camera camera, out RaycastHit2D hit)
    {
        localPose = new Vector3(localPose.x * camera.pixelWidth, localPose.y * camera.pixelHeight);

        bool res = false;

        var ray = camera.ScreenPointToRay(localPose);
        hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider != null)
            res = true;


        return res;
    }
}
