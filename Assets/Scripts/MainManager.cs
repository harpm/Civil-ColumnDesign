using System;
using UnityEngine;

public class MainManager : MonoBehaviour
{
    [Header("UI")]
    [SerializeField]
    public MainWindow MainWindow;

    [SerializeField]
    public CreateGridWindow CreateGridWindow;

    [SerializeField]
    public InspectorManager InspectorWindow;

    [SerializeField]
    private CustomXY _customXY;

    [SerializeField]
    private CustomStories _customStories;

    [Header("Core Scripts")]
    [SerializeField]
    public CameraManager CameraManager;

    [SerializeField]
    public GridBuilder GridBuilder;

    [SerializeField]
    public MouseManager MouseManager;

    [Header("Calculations")]
    [SerializeField]
    public CoreCalculator CoreCalculator;

    public static MainManager Instance;

    void Awake()
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

    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = 0;

        try
        {
            Application.targetFrameRate = -1;
        }
        catch
        {
            // ignored
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
