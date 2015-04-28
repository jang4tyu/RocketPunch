using UnityEngine;
using System.Collections;

public class GameLogState : StateBase
{
    GameObject GameLogPanel;

    public override void OnEnter(System.Action callback)
    {
        base.OnEnter(callback);

        GameLogPanel = UIManager.instance.SetPanel(ePanel.GameLogPanel);
        GameLogPanel.GetComponent<GameLogPanel>().Init();
    }

    public override void OnExit(System.Action callback)
    {
        base.OnExit(callback);
    }
}
