using Michsky.UI.ModernUIPack;
using UnityEngine;
using UnityEngine.UI;

public class CommandBtnManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrawLineBtnClicked()
    {
        MainManager.Instance.MouseManager.CancelInProgressCmd();
        MainManager.Instance.MouseManager.CurrentCommand = MouseManager.Command.DrawLineFirstPoint;
        MainManager.Instance.MainWindow.StatusMessage("Command draw line pressed!", MainWindow.MessageType.Info);
    }
}
