using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private Camera _3DCamera;

    [SerializeField]
    private Camera _2DCamera;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TargetOnGrid3D()
    {
        var center = MainManager.Instance.GridBuilder.GetCenter3D();
        var size = MainManager.Instance.GridBuilder.GetSize3D();

        float distance = Mathf.Max(size.x, size.y, size.z);
        distance /= 2.0f * Mathf.Tan(0.5f * _3DCamera.fieldOfView * Mathf.Deg2Rad);

        _3DCamera.transform.position = new Vector3(center.x + size.x, center.y + size.y, center.z - _3DCamera.transform.forward.normalized.z * distance * 2);
        _3DCamera.transform.LookAt(center);

        MainManager.Instance.MouseManager.CurrentRotation = _3DCamera.transform.localEulerAngles;
        MainManager.Instance.MouseManager.RotateX = _3DCamera.transform.localEulerAngles.x;
        MainManager.Instance.MouseManager.RotateY = _3DCamera.transform.localEulerAngles.y;
    }
}
