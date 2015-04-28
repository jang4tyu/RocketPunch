using UnityEngine;
using System.Collections;

public class LobbyState : StateBase
{
    GameObject LobbyPanel;

    public override void OnEnter(System.Action callback)
    {
        base.OnEnter(callback);

        Application.LoadLevel("3.LobbyScene");
        DataManager.instance.SetTarget(null);
        UICamera.mainCamera.clearFlags = CameraClearFlags.Depth;
    }

    public override void OnExit(System.Action callback)
    {
        base.OnExit(callback);
    }

    void OnLevelWasLoaded(int level)
    {
        if (Application.loadedLevelName == "3.LobbyScene")
        {
            UIManager.instance.SetInfoPanel();
            LobbyPanel = UIManager.instance.SetPanel(ePanel.LobbyPanel);
            LobbyPanel.GetComponent<LobbyPanel>().Init();
        }
    }
}
