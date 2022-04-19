using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ViewWindow : MonoBehaviour
{
    [SerializeField]
    private Toggle _xToggle;

    [SerializeField]
    private Toggle _yToggle;

    [SerializeField]
    private Toggle _sToggle;

    [SerializeField]
    private TMP_Dropdown _xDropdown;

    [SerializeField]
    private TMP_Dropdown _yDropdown;

    [SerializeField]
    private TMP_Dropdown _sDropdown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetValues()
    {
        _xDropdown.ClearOptions();
        _yDropdown.ClearOptions();
        _sDropdown.ClearOptions();

        var xOptions = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < MainManager.Instance.GridBuilder.NumberX; i++)
        {
            TMP_Dropdown.OptionData od = new TMP_Dropdown.OptionData();
            od.text = "X-" + (i + 1);
            xOptions.Add(od);
        }
        _xDropdown.AddOptions(xOptions);

        var yOptions = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < MainManager.Instance.GridBuilder.NumberY; i++)
        {
            var od = new TMP_Dropdown.OptionData();
            od.text = "Y-" + (i + 1);
            yOptions.Add(od);
        }
        _yDropdown.AddOptions(yOptions);

        var sOptions = new List<TMP_Dropdown.OptionData>();
        for (int i = 0; i < MainManager.Instance.GridBuilder.NumberS; i++)
        {
            var od = new TMP_Dropdown.OptionData();
            od.text = "Story-" + (i + 1);
            sOptions.Add(od);
        }
        _sDropdown.AddOptions(sOptions);
    }

    public void OnToggleX(bool value)
    {
        _xDropdown.interactable = value;

        if (value)
        {
            _yDropdown.interactable = false;
            _sDropdown.interactable = false;
        }
    }

    public void OnToggleY(bool value)
    {
        _yDropdown.interactable = value;
        if (value)
        {
            _xDropdown.interactable = false;
            _sDropdown.interactable = false;
        }
    }

    public void OnToggleS(bool value)
    {
        _sDropdown.interactable = value;
        if (value)
        {
            _xDropdown.interactable = false;
            _yDropdown.interactable = false;
        }
    }

    public void Show()
    {
        if (_xToggle.isOn)
            MainManager.Instance.GridBuilder.ShowSliceX(_xDropdown.value);
        else if (_yToggle.isOn)
            MainManager.Instance.GridBuilder.ShowSliceY(_yDropdown.value);
        else if (_sToggle.isOn)
            MainManager.Instance.GridBuilder.ShowSliceS(_sDropdown.value);

        Close();
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
