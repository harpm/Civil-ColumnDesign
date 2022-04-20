using System.Collections.Generic;
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
    private GameObject _hConnectionObj;

    [SerializeField]
    private GameObject _lConnectionObj;

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

    private Line _selectedLine;

    private LineType _selectedLineType;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectLine(Line line)
    {
        _selectedLine = line;
        if (line._curAxis == Line.Axis.Z || line._curAxis == Line.Axis.Nz)
        {
            _selectedLineType = LineType.Column;
            DisplayColumnInspector(line.Length, line.AliveForces, line.DeadForces);
        }
        else
        {
            _selectedLineType = LineType.Beam;
            DisplayBeamInspector(line.Length, line.Inertia, line.HigherConnection,
                line.LowerConnection, line.IsOnGround);
        }

    }

    public void DeselectLine()
    {
        Apply();
        DisplayOffInspector();
    }

    private void DisplayColumnInspector(float length, float aliveForce, float deadForce)
    {
        Content.SetActive(true);
        _forces.SetActive(true);

        _title.text = _selectedLineType.ToString();
        

        _length.text = length + " m";
        _aliveForceInp.text = aliveForce.ToString();
        _deadForceInp.text = deadForce.ToString();
    }

    private void DisplayBeamInspector(float length, float inertia, Line.ConnectionType hConnType,
        Line.ConnectionType lConnType, bool isOnGround)
    {
        Content.SetActive(true);
        _inertia.SetActive(true);
        _hConnectionObj.SetActive(true);
        _lConnectionObj.SetActive(true);

        _title.text = _selectedLineType.ToString();

        _length.text = length + " m";
        _inertiaInp.text = inertia.ToString();
        _hConnection.value = (int)hConnType;
        _lConnection.value = (int)lConnType;

        _hConnection.ClearOptions();
        _lConnection.ClearOptions();

        var options = new List<TMP_Dropdown.OptionData>();
        options.Add(new TMP_Dropdown.OptionData("Fixed"));
        options.Add(new TMP_Dropdown.OptionData("Pinned"));
        
        if (!isOnGround)
        {
            options.Add(new TMP_Dropdown.OptionData("Console"));
            options.Add(new TMP_Dropdown.OptionData("Roller Support"));
        }
        
        _hConnection.AddOptions(options);
        _lConnection.AddOptions(options);

    }

    private void DisplayOffInspector()
    {
        _forces.SetActive(false);
        _inertia.SetActive(false);
        _hConnectionObj.SetActive(false);
        _lConnectionObj.SetActive(false);
        Content.SetActive(false);
    }

    private void Apply()
    {
        if (_selectedLineType == LineType.Column)
        {
            _selectedLine.AliveForces = float.Parse(_aliveForceInp.text);
            _selectedLine.DeadForces = float.Parse(_deadForceInp.text);
        }
        else
        {
            _selectedLine.Inertia = float.Parse(_inertiaInp.text);
            _selectedLine.HigherConnection = (Line.ConnectionType)_hConnection.value;
            _selectedLine.LowerConnection = (Line.ConnectionType)_lConnection.value;
        }
    }

    private enum LineType
    {
        Column,
        Beam
    }
}