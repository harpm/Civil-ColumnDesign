using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoint2D : MonoBehaviour
{
    public GridPoint MainInstance;

    public int X;
    public int Y;

    public float SpacingX;
    public float SpacingY;

    public GridPoint2D PreviousX;
    public GridPoint2D PreviousY;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AdjustPosition()
    {
        float x = 0, y = 0;
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

        transform.position = new Vector3(x, y);
    }
}
