using TMPro;
using UnityEngine;

public class InspectorManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    public GameObject Content;

    [SerializeField]
    private TextMeshProUGUI _title;

    [SerializeField]
    private TextMeshProUGUI _length;

    [SerializeField]
    private GameObject _forces;

    [SerializeField]
    private GameObject _inertia;

    [SerializeField]
    private TMP_InputField _aliveForceInp;

    [SerializeField]
    private TMP_InputField _deadForceInp;

    [SerializeField]
    private TMP_InputField _inertiaInp;

    [SerializeField]
    private TMP_Dropdown _hConnection;

    [SerializeField]
    private TMP_Dropdown _lConnection;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void DisplayColumnInspector(float length, float aliveForce, float deadForce, Line.ConnectionType hConnType,
        Line.ConnectionType lConnType)
    {
        Content.SetActive(true);
        _forces.SetActive(true);

        _title.text = "Column";
        

        _length.text = length + " m";
        _aliveForceInp.text = aliveForce.ToString();
        _deadForceInp.text = deadForce.ToString();
        _hConnection.value = (int) hConnType;
        _lConnection.value = (int) lConnType;
    }

    public void DisplayBeamInspector(float length, float inertia, Line.ConnectionType hConnType,
        Line.ConnectionType lConnType)
    {
        Content.SetActive(true);
        _inertia.SetActive(true);

        _title.text = "Beam";

        _length.text = length + " m";
        _inertiaInp.text = inertia.ToString();
        _hConnection.value = (int)hConnType;
        _lConnection.value = (int)lConnType;
    }

    public void DisplayOffInspector()
    {
        _forces.SetActive(false);
        _inertia.SetActive(false);
        Content.SetActive(false);
    }
}
