using UnityEngine;
using UnityEngine.Events;

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
    private Transform _3DRenderScreen;

    [SerializeField]
    private Transform _2DRenderScreen;

    public Vector3 CurrentRotation;

    public float RotateX;
    public float RotateY;

    private bool _rightClick;

    private Vector3 _rotVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseEvents(_3DRenderScreen, KeyCode.Mouse1, ref _rightClick, RotateByMouse);
    }

    private void HandleMouseEvents(Transform screen, KeyCode kc, ref bool isMoving, UnityAction action)
    {
        var mPose = Input.mousePosition;

        if (Input.GetKeyDown(kc))
        {
            if (TryHitRenderer(mPose, screen, out Vector3 localPose))
                isMoving = true;
        }
        else if (Input.GetKey(kc))
        {
            if (isMoving)
                action();
        }
        else if (Input.GetKeyUp(kc))
        {
            isMoving = false;
        }
    }

    private void RotateByMouse()
    {
        var axisX = Input.GetAxis("Mouse X");
        var axisY = -Input.GetAxis("Mouse Y");

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

    private bool TryHitRenderer(Vector3 pose, Transform renderer, out Vector3 localPose)
    {
        bool res = false;
        localPose = -Vector3.one;

        var ray = Camera.main.ScreenPointToRay(pose);
        var hit = Physics2D.Raycast(ray.origin, ray.direction);
        if (hit.collider != null && hit.transform == renderer)
        {
            res = true;
            localPose = renderer.InverseTransformPoint(pose);
        }

        return res;
    }
}
