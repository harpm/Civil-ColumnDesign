using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoint3D : MonoBehaviour
{
    public GridPoint MainInstance;

    public GridPoint3D PreviousX;
    public GridPoint3D PreviousY;
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
            x = PreviousX.transform.position.x;
            if (MainInstance.X > PreviousX.MainInstance.X)
                x += MainInstance.SpaceX;
        }

        if (PreviousY != null)
        {
            y = PreviousY.transform.position.z;
            if (MainInstance.Y > PreviousY.MainInstance.Y)
                y += MainInstance.SpaceY;
        }

        if (PreviousS != null)
        {
            s = PreviousS.transform.position.y;
            if (MainInstance.S > PreviousS.MainInstance.S)
                s += MainInstance.SpaceS;
        }


        transform.position = new Vector3(x, s, y);
    }
}
