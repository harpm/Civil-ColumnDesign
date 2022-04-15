using System;
using System.Collections;
using System.Collections.Generic;
using Michsky.UI.ModernUIPack;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CreateGridWindow : MonoBehaviour
{
    [SerializeField]
    private CustomXY _customXYWindow;

    [SerializeField]
    private CustomStories  _customStoriesWindow;

    [SerializeField]
    private TMP_InputField _xNoInput;

    [SerializeField]
    private TMP_InputField _yNoInput;

    [SerializeField]
    private TMP_InputField _sNoInput;

    [SerializeField]
    private TMP_InputField _xSpaceInput;

    [SerializeField]
    private TMP_InputField _ySpaceInput;

    [SerializeField]
    private TMP_InputField _sSpaceInput;

    [SerializeField]
    private SwitchManager _customXYSwitch;

    [SerializeField]
    private SwitchManager _customStoriesSwitch;

    [SerializeField]
    private Button _customXYBtn;
    
    [SerializeField]
    private Button _customSBtn;

    [SerializeField]
    private GameObject _errorWindow;

    [SerializeField]
    private TextMeshProUGUI _errorText;


    private int _xNo;
    private int _yNo;
    private int _sNo;

    private List<float> _xSpaces;
    private List<float> _ySpaces;
    private List<float> _sSpaces;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CustomXYOn()
    {
        _xNoInput.interactable = false;
        _yNoInput.interactable = false;
        _xSpaceInput.interactable = false;
        _ySpaceInput.interactable = false;
        _customXYBtn.interactable = true;
    }

    public void CustomXYOff()
    {
        _xNoInput.interactable = true;
        _yNoInput.interactable = true;
        _xSpaceInput.interactable = true;
        _ySpaceInput.interactable = true;
        _customXYBtn.interactable = false;

    }

    public void CustomSOn()
    {
        _sNoInput.interactable = false;
        _sSpaceInput.interactable = false;
        _customSBtn.interactable = true;
    }

    public void CustomSOff()
    {
        _sNoInput.interactable = true;
        _sSpaceInput.interactable = true;
        _customSBtn.interactable = false;
    }

    public void OnCustomXY()
    {
        _customXYWindow.enabled = true;
        enabled = false;
    }

    public void CreateGrids()
    {
        if (!_customXYSwitch.isOn)
        {
            if (_xNoInput.text == string.Empty)
            {
                ShowError("No x should have numeric value");
                return;
            }

            if (_yNoInput.text == string.Empty)
            {
                ShowError("No y Should have numeric value");
                return;
            }

            if (_xSpaceInput.text == string.Empty)
            {
                ShowError("No y Should have numeric value");
                return;
            }

            if (_ySpaceInput.text == string.Empty)
            {
                ShowError("No y Should have numeric value");
                return;
            }

            _xNo = Int32.Parse(_xNoInput.text);
            _yNo = Int32.Parse(_yNoInput.text);
            
            var defaultXSpace = float.Parse(_xSpaceInput.text);
            _xSpaces.ForEach(f => f = defaultXSpace);

            var defaultYSpace = float.Parse(_ySpaceInput.text);
            _ySpaces.ForEach(f => f = defaultYSpace);
        }

        if (!_customStoriesSwitch.isOn)
        {
            if (_sNoInput.text == string.Empty)
            {
                ShowError("No y Should have numeric value");
                return;
            }

            if (_sSpaceInput.text == string.Empty)
            {
                ShowError("No y Should have numeric value");
                return;
            }


            _sNo = Int32.Parse(_sNoInput.text);

            var defaultSSpace = float.Parse(_sSpaceInput.text);
            _sSpaces.ForEach(f => f = defaultSSpace);
        }


        MainManager.Instance.GridBuilder.GenerateGrid(_xNo, _yNo, _sNo, _xSpaces.ToArray(), _ySpaces.ToArray(), _sSpaces.ToArray());
        Close();
    }

    private void ShowError(string message)
    {
        _errorWindow.SetActive(true);
        _errorText.text = message;
    }

    public void DismissError()
    {
        _errorWindow.SetActive(false);
    }

    public void Close()
    {
        ResetValues();
        gameObject.SetActive(false);
    }

    private void ResetValues()
    {
        _xNoInput.text = string.Empty;
        _yNoInput.text = string.Empty;
        _sNoInput.text = string.Empty;
        _xSpaceInput.text = string.Empty;
        _ySpaceInput.text = string.Empty;
        _sSpaceInput.text = string.Empty;
        _customXYSwitch.isOn = false;
        _customStoriesSwitch.isOn = false;
    }
}
