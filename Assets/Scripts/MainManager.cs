using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    private MainWindow _mainWindow;

    [SerializeField]
    private CreateGridWindow _createGridWindow;

    [SerializeField]
    private CustomXY _customXY;

    [SerializeField]
    private CustomStories _customStories;

    [Header("Core Scripts")]
    [SerializeField]
    private CameraManager _cameraManager;

    [SerializeField]
    public GridBuilder GridBuilder;

    public static MainManager Instance;

    // Start is called before the first frame update
    void Start()
    {
        var mms = FindObjectsOfType<MainManager>();
        if (mms.Length > 1)
        {
            Debug.LogError("There should be only one main manager script in the scene!");
            DestroyImmediate(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
