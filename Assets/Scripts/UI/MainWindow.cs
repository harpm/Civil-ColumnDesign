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

    [SerializeField]
    private GameObject _outputWindow;

    [SerializeField]
    private TextMeshProUGUI _outputText;

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
        _errorWindow.SetActive(true);
        _errorMessage.text = message;
    }

    public void DismissError()
    {
        _errorWindow.SetActive(false);
    }

    public void ShowOutput(ProfileCalcResult res)
    {
        _outputWindow.SetActive(true);
        _outputText.text = res.Answer;
        
    }

    public void CloseOutput()
    {
        _outputWindow.gameObject.SetActive(false);
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

    public void ExitApplication()
    {
        Destroy(MainManager.Instance.MouseManager.gameObject);
        Destroy(MainManager.Instance.CameraManager.gameObject);
        Destroy(MainManager.Instance.GridBuilder.gameObject);
        Application.Quit(0);
    }

    public enum MessageType
    {
        Info,
        Successful,
        Error,
        Data
    }
    
}
