using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainWindow : MonoBehaviour
{
    [SerializeField]
    private CreateGridWindow _createGridWindow;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OpenCreatGridWindow()
    {
        _createGridWindow.gameObject.SetActive(true);
    }
}
