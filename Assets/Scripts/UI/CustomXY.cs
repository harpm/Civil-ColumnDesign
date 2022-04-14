using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CustomXY : MonoBehaviour
{
    [SerializeField]
    private CustomXY CustomSYWindow;

    [SerializeField]
    private TMP_InputField _xNo;

    [SerializeField]
    private TMP_InputField _yNo;

    [SerializeField]
    private TMP_InputField _xSpace;

    [SerializeField]
    private TMP_InputField _ySpace;

    [SerializeField]
    private Button _customXYBtn;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CustomOn()
    {
        _xNo.interactable = false;
        _yNo.interactable = false;
        _xSpace.interactable = false;
        _ySpace.interactable = false;
        _customXYBtn.interactable = true;
    }

    public void CustomOff()
    {
        _xNo.interactable = true;
        _yNo.interactable = true;
        _xSpace.interactable = true;
        _ySpace.interactable = true;
        _customXYBtn.interactable = false;

    }

    public void OnCustomXY()
    {
        CustomSYWindow.enabled = true;
        enabled = false;
    }
}
