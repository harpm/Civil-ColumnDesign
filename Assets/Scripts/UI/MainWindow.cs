using TMPro;
using UnityEngine;

public class MainWindow : MonoBehaviour
{
    [SerializeField]
    private CreateGridWindow _createGridWindow;

    [SerializeField]
    private ViewWindow _changeViewWindow;

    [SerializeField]
    private GameObject _errorWindow;

    [SerializeField]
    private TextMeshProUGUI _errorMessage;

    [SerializeField]
    private TextMeshProUGUI _statusText;

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

    public void OpenChangeViewWindow()
    {
        _changeViewWindow.gameObject.SetActive(true);
        _createGridWindow.Close();
        _changeViewWindow.SetValues();
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

    public void StatusMessage(string message, MessageType type)
    {
        _statusText.text = message;
        switch (type)
        {
            case MessageType.Info:
                _statusText.color = Color.blue;
                break;

            case MessageType.Successful:
                _statusText.color = Color.green;
                break;

            case MessageType.Error:
                _statusText.color = Color.red;
                break;

            case MessageType.Data:
                _statusText.color = Color.black;
                break;

        }
    }

    public enum MessageType
    {
        Info,
        Successful,
        Error,
        Data
    }
    
}
