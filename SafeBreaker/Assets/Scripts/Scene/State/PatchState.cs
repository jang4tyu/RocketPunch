using UnityEngine;
using System.Collections;

public class PatchState : StateBase
{
    GameObject PatchPanel;

    public override void OnEnter(System.Action callback)
    {
        base.OnEnter(callback);

        Application.LoadLevel("2.PatchScene");
    }

    public override void OnExit(System.Action callback)
    {
        base.OnExit(callback);
    }

    void OnLevelWasLoaded(int level)
    {
        if (Application.loadedLevelName == "2.PatchScene")
        {
            PatchPanel = UIManager.instance.SetPanel(ePanel.PatchPanel);
            //PatchPanel.GetComponent<PatchPanel>().Next();
        }
    }
}