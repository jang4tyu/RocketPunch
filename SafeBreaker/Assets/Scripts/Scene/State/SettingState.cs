using UnityEngine;
using System.Collections;

public class SettingState : StateBase
{
    GameObject SettingPanel;

    public override void OnEnter(System.Action callback)
    {
        base.OnEnter(callback);

        SettingPanel = UIManager.instance.SetPanel(ePanel.SettingPanel);
        SettingPanel.GetComponent<SettingPanel>().Init();
    }

    public override void OnExit(System.Action callback)
    {
        base.OnExit(callback);
    }
}