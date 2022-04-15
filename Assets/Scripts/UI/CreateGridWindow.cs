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


    private int _xNo;
    private int _yNo;
    private int _sNo;

    private float[] _xSpaces;
    private float[] _ySpaces;
    private float[] _sSpaces;

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

    public void OnCustomXY()
    {
        _customXYWindow.enabled = true;
        enabled = false;
    }

    public void CreateGrids()
    {
        if (!_customXYSwitch.isOn)
        {

        }

        if (!_customStoriesSwitch.isOn)
        {

        }
        

        MainManager.Instance.GridBuilder.GenerateGrid(_xNo, _yNo, _sNo, _xSpaces, _ySpaces, _sSpaces);
    }
}
