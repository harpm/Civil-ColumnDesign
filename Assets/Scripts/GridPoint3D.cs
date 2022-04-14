using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoint3D : MonoBehaviour
{
    public GridPoint MainInstance;

    private GridPoint3D _previousX;
    private GridPoint3D _previousY;
    private GridPoint3D _previousS;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GetPreviousGrids(GridPoint3D px, GridPoint3D py, GridPoint3D ps)
    {
        this._previousX = px;
        this._previousY = py;
        this._previousS = ps;
    }

    public void AdjustPositions()
    {
        float x = 0, y = 0, s = 0;

        if (_previousX != null)
        {
            x = _previousX.transform.position.x;
            if (MainInstance.X > _previousX.MainInstance.X)
                x += MainInstance.SpaceX;
        }

        if (_previousY != null)
        {
            y = _previousY.transform.position.z;
            if (MainInstance.Y > _previousY.MainInstance.Y)
                y += MainInstance.SpaceY;
        }

        if (_previousS != null)
        {
            s = _previousS.transform.position.y;
            if (MainInstance.S > _previousS.MainInstance.S)
                s += MainInstance.SpaceS;
        }


        transform.position = new Vector3(x, s, y);
    }
}
