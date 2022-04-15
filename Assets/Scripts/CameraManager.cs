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
        distance /= (2.0f * Mathf.Tan(0.5f * _3DCamera.fieldOfView * Mathf.Deg2Rad));

        _3DCamera.transform.position = center - _3DCamera.transform.forward.normalized * distance;
    }
}
