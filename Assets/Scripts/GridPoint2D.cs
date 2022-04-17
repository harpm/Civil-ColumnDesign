using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridPoint2D : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer _renderer;

    [HideInInspector]
    public GridPoint MainInstance;

    [HideInInspector]
    public int X;
    [HideInInspector]
    public int Y;

    [HideInInspector]
    public float SpacingX;
    [HideInInspector]
    public float SpacingY;

    [HideInInspector]
    public GridPoint2D PreviousX;
    [HideInInspector]
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
            if (MainInstance.X > PreviousX.X)
                x += MainInstance.SpaceX;
        }

        if (PreviousY != null)
        {
            y = PreviousY.transform.localPosition.y;
            if (MainInstance.Y > PreviousY.Y)
                y += MainInstance.SpaceY;
        }

        transform.localPosition = new Vector3(x, y);
    }

    public void Hover()
    {
        _renderer.color = Color.green;
    }

    public void EndHover()
    {
        _renderer.color = Color.white;
    }
}
