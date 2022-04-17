using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoint3D : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer _renderer;

    [SerializeField]
    private Material _mainMaterial;

    [SerializeField]
    private Material _hoveringMaterial;

    [HideInInspector]
    public GridPoint MainInstance;

    [HideInInspector]
    public GridPoint3D PreviousX;
    [HideInInspector]
    public GridPoint3D PreviousY;
    [HideInInspector]
    public GridPoint3D PreviousS;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustPositions()
    {
        float x = 0, y = 0, s = 0;

        if (PreviousX != null)
        {
            x = PreviousX.transform.localPosition.x;
            if (MainInstance.X > PreviousX.MainInstance.X)
                x += MainInstance.SpaceX;
        }

        if (PreviousY != null)
        {
            y = PreviousY.transform.localPosition.z;
            if (MainInstance.Y > PreviousY.MainInstance.Y)
                y += MainInstance.SpaceY;
        }

        if (PreviousS != null)
        {
            s = PreviousS.transform.localPosition.y;
            if (MainInstance.S > PreviousS.MainInstance.S)
                s += MainInstance.SpaceS;
        }


        transform.localPosition = new Vector3(x, s, y);
    }

    public void Hover()
    {
        _renderer.material = _hoveringMaterial;
    }

    public void EndHover()
    {
        _renderer.material = _mainMaterial;
    }
}
