using System;
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
    private CustomStories _customStoriesWindow;

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


    private int _xNo = 3;
    private int _yNo = 3;
    private int _sNo = 3;

    private float[] _xSpaces = new float[] { 10.0f, 10.0f, 10.0f };
    private float[] _ySpaces = new float[] { 10.0f, 10.0f, 10.0f };
    private float[] _sSpaces = new float[] { 8.0f, 8.0f, 8.0f };

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

    public void SetCustomXY(int x, int y, float[] xSpaces, float[] ySpaces)
    {
        _xNo = x;
        _yNo = y;
        _xSpaces = xSpaces;
        _ySpaces = ySpaces;

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

    public void SetCustomS(int s, float[] sSpaces)
    {
        _sNo = s;
        _sSpaces = sSpaces;
    }

    public void BtnCustomXYClicked()
    {
        _customXYWindow.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    public void BtnCustomSClicked()
    {
        _customStoriesWindow.gameObject.SetActive(true);
        gameObject.SetActive(false);
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
            _xSpaces = new float[_xNo];
            for (int i = 0; i < _xNo; i++)
            {
                _xSpaces[i] = defaultXSpace;
            }

            var defaultYSpace = float.Parse(_ySpaceInput.text);
            _ySpaces = new float[_yNo];
            for (int i = 0; i < _yNo; i++)
            {
                _ySpaces[i] = defaultYSpace;
            }
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
            _sSpaces = new float[_sNo];
            for (int i = 0; i < _sNo; i++)
            {
                _sSpaces[i] = defaultSSpace;
            }
        }


        MainManager.Instance.GridBuilder.GenerateGrid(_xNo, _yNo, _sNo, _xSpaces, _ySpaces, _sSpaces);
        Close();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    private void ShowError(string message)
    {
        MainManager.Instance.MainWindow.ShowError(message);
    }
}
