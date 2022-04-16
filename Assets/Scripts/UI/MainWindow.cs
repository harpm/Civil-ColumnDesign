using TMPro;
using UnityEngine;

public class MainWindow : MonoBehaviour
{
    [SerializeField]
    private CreateGridWindow _createGridWindow;

    [SerializeField]
    private GameObject _errorWindow;

    [SerializeField]
    private TextMeshProUGUI _errorMessage;

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

    public void ShowError(string message)
    {
        _errorWindow.gameObject.SetActive(true);
        _errorMessage.text = message;
    }

    public void DismissError()
    {
        _errorWindow.gameObject.SetActive(false);
    }
}
