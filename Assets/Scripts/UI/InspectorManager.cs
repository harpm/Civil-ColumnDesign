using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private GameObject _forcesAD;

    [SerializeField]
    private GameObject _forcesU;

    [SerializeField]
    private Toggle _forceADToggle;

    [SerializeField]
    private Toggle _forceUToggle;

    [SerializeField]
    private GameObject _hConnectionObj;

    [SerializeField]
    private GameObject _lConnectionObj;

    [SerializeField]
    private GameObject _xBraceObj;

    [SerializeField]
    private GameObject _yBraceObj;

    [SerializeField]
    private GameObject _fyObj;

    [SerializeField]
    private GameObject _colSuppObj;

    [SerializeField]
    private TMP_Dropdown _fyDropdown;

    [SerializeField]
    private TMP_InputField _aliveForceInp;

    [SerializeField]
    private TMP_InputField _deadForceInp;

    [SerializeField]
    private TMP_InputField _ultimateForceInp;

    [SerializeField]
    private TMP_InputField _inertiaInp;

    [SerializeField]
    private TMP_Dropdown _hConnection;

    [SerializeField]
    private TMP_Dropdown _lConnection;

    [SerializeField]
    private TMP_Dropdown _xBrace;

    [SerializeField]
    private TMP_Dropdown _yBrace;

    [SerializeField]
    private TMP_Dropdown _columnSupport;

    [SerializeField]
    private GameObject _runComponents;

    [SerializeField]
    private TMP_Dropdown _outputOptions;

    [SerializeField]
    private Button _deleteBtn;

    [SerializeField]
    private GameObject LoadingModal;
    
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
        _deleteBtn.interactable = true;

        if (line._curAxis == Line.Axis.Z || line._curAxis == Line.Axis.Nz)
        {
            _selectedLineType = LineType.Column;
            DisplayColumnInspector(line);
        }
        else
        {
            _selectedLineType = LineType.Beam;
            DisplayBeamInspector(line);
        }

    }

    public void DeselectLine()
    {
        Apply();
        DisplayOffInspector();
        _deleteBtn.interactable = false;
    }

    private void DisplayColumnInspector(Line line)
    {
        Content.SetActive(true);
        _forcesAD.SetActive(true);
        _forcesU.SetActive(true);
        _fyObj.SetActive(true);
        _runComponents.SetActive(true);
        _xBraceObj.SetActive(true);
        _yBraceObj.SetActive(true);

        if (line.IsOnGround)
        {
            _colSuppObj.SetActive(true);
        }

        _title.text = _selectedLineType.ToString();

        _length.text = line.Length + " cm";
        _inertiaInp.text = line.Inertia.ToString();
        _xBrace.value = Convert.ToInt32(line.IsBracedX);
        _yBrace.value = Convert.ToInt32(line.IsBracedY);

        if (line.ForceAD)
        {
            _forceADToggle.isOn = true;
            _aliveForceInp.text = line.AliveForces.ToString();
            _deadForceInp.text = line.DeadForces.ToString();
        }
        else
        {
            _forceUToggle.isOn = true;
            _ultimateForceInp.text = line.UltimateForce.ToString();
        }


    }

    private void DisplayBeamInspector(Line line)
    {
        Content.SetActive(true);
        _hConnectionObj.SetActive(true);
        _lConnectionObj.SetActive(true);

        _title.text = _selectedLineType.ToString();

        _length.text = line.Length + " cm";
        _inertiaInp.text = line.Inertia.ToString();
        _hConnection.value = (int)line.HigherConnection;
        _lConnection.value = (int)line.LowerConnection;

    }

    private void DisplayOffInspector()
    {
        _forcesAD.SetActive(false);
        _forcesU.SetActive(false);
        _hConnectionObj.SetActive(false);
        _lConnectionObj.SetActive(false);
        _xBraceObj.SetActive(false);
        _yBraceObj.SetActive(false);
        _colSuppObj.SetActive(false);
        _fyObj.SetActive(false);
        _runComponents.SetActive(false);
        Content.SetActive(false);
    }

    /// <summary>
    /// Apply all the properties to the Selected Line Variable
    /// in order to calculate the selected line requested profile
    /// </summary>
    private void Apply()
    {
        if (_selectedLine == null)
            return;
        if (_selectedLineType == LineType.Column)
        {
            _selectedLine.IsBracedX = _xBrace.value != 0;
            _selectedLine.IsBracedY = _yBrace.value != 0;

            if (_selectedLine.IsOnGround)
            {
                _selectedLine.SuppType = (Line.SupportType)_columnSupport.value;
            }

            _selectedLine.ForceAD = _forceADToggle.isOn;
            _selectedLine.Fy = _fyDropdown.value == 0 ? 2400.0f : 3700.0f;

            if (_forceADToggle.isOn)
            {
                _selectedLine.AliveForces = float.Parse(_aliveForceInp.text);
                _selectedLine.DeadForces = float.Parse(_deadForceInp.text);
                _selectedLine.UltimateForce = 0;
            }
            else
            {
                _selectedLine.UltimateForce = float.Parse(_ultimateForceInp.text);
                _selectedLine.AliveForces = 0;
                _selectedLine.DeadForces = 0;
            }
        }
        else
        {
            _selectedLine.HigherConnection = (Line.ConnectionType)_hConnection.value;
            _selectedLine.LowerConnection = (Line.ConnectionType)_lConnection.value;
        }

        _selectedLine.Inertia = float.Parse(_inertiaInp.text);
    }

    public void DeleteLine()
    {
        MainManager.Instance.MouseManager.DeleteSteelLine(_selectedLine);
        DeselectLine();
    }

    public void Run()
    {
        StartCoroutine(RunCalculation());
    }

    private IEnumerator RunCalculation()
    {
        yield return new WaitForEndOfFrame();
        StartCalculating();
        yield return new WaitForEndOfFrame();

        Apply();
        ProfileCalcResult res = null;

        switch ((RunOption)_outputOptions.value)
        {
            case RunOption.IPB:
                {
                    yield return res = MainManager.Instance.CoreCalculator.EvaluateIpb(_selectedLine);
                    break;
                }
            case RunOption.ReinforcedIPB:
                {
                    yield return res = MainManager.Instance.CoreCalculator.EvaluateReinforcedIpb(_selectedLine);
                    break;
                }
            case RunOption.ReinforcedIPE:
                {
                    yield return res = MainManager.Instance.CoreCalculator.EvaluateReinforcedIpe(_selectedLine);
                    break;
                }
            case RunOption.DoubleIPEComplete:
                {
                    yield return res = MainManager.Instance.CoreCalculator.EvaluateDoubleIpeComplete(_selectedLine);
                    break;
                }
            case RunOption.DoubleIPEDiagonal:
                {
                    yield return res = MainManager.Instance.CoreCalculator.EvaluateDoubleIpeDiagonal(_selectedLine);
                    break;
                }
            case RunOption.DoubleIPECross:
                {
                    yield return res = MainManager.Instance.CoreCalculator.EvaluateDoubleIpeCross(_selectedLine);
                    break;
                }
            case RunOption.BoxHSSRectangular:
                {
                    yield return res = MainManager.Instance.CoreCalculator.EvaluateHssBoxRect(_selectedLine);
                    break;
                }
            case RunOption.BoxHSSSquare:
                {
                    yield return res = MainManager.Instance.CoreCalculator.EvaluateSquareHss(_selectedLine);
                    break;
                }
            case RunOption.RoundHSS:
                {
                    yield return res = MainManager.Instance.CoreCalculator.EvaluateRoundHss(_selectedLine);
                    break;

                }
            case RunOption.EvaluateDoubleIpeParallel:
                {
                    yield return res = MainManager.Instance.CoreCalculator.EvaluateDoubleIpeParallel(_selectedLine);
                    break;
                }
        }

        yield return new WaitForSeconds(1);
        yield return new WaitForEndOfFrame();
        StopCalculating();

        MainManager.Instance.MainWindow.ShowOutput(res);
    }


    private void StartCalculating()
    {
        LoadingModal.SetActive(true);
    }

    private void StopCalculating()
    {
        LoadingModal.SetActive(false);
    }

    public void ForceAdChanged(bool on)
    {
        _aliveForceInp.interactable = on;
        _deadForceInp.interactable = on;

        if (on)
        {
            _ultimateForceInp.interactable = false;
        }
    }

    public void ForceUChanged(bool on)
    {
        _ultimateForceInp.interactable = on;

        if (on)
        {
            _aliveForceInp.interactable = false;
            _deadForceInp.interactable = false;
        }
    }

    private enum RunOption
    {
        IPB,
        ReinforcedIPB,
        ReinforcedIPE,
        DoubleIPEComplete,
        DoubleIPEDiagonal,
        DoubleIPECross,
        BoxHSSRectangular,
        BoxHSSSquare,
        RoundHSS,
        EvaluateDoubleIpeParallel
    }

    private enum LineType
    {
        Column,
        Beam
    }
}