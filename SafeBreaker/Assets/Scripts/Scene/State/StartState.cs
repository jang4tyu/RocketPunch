using UnityEngine;
using System.Collections;

public class StartState : StateBase
{
    GameObject StartPanel;

    public override void OnEnter(System.Action callback)
    {
        base.OnEnter(callback);

        StartPanel = UIManager.instance.SetPanel(ePanel.StartPanel);
    }

    public override void OnExit(System.Action callback)
    {
        base.OnExit(callback);
    }
}