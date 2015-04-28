using UnityEngine;
using System.Collections;

public class NicknameState : StateBase
{
    GameObject NicknamePanel;

    public override void OnEnter(System.Action callback)
    {
        base.OnEnter(callback);

        NicknamePanel = UIManager.instance.SetPanel(ePanel.NicknamePanel);
        NicknamePanel.GetComponent<NicknamePanel>().Init();
    }

    public override void OnExit(System.Action callback)
    {
        base.OnExit(callback);
    }
}