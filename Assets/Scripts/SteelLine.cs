using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SteelLine : MonoBehaviour
{
    [SerializeField]
    public LineRenderer Renderer;

    private Axis _curAxis;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }



    private enum Axis
    {
        X, Y, Z
    }
}
