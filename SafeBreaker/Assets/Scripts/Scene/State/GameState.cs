using UnityEngine;
using System.Collections;

public class GameState : StateBase 
{
    GameObject GamePanel;

    public override void OnEnter(System.Action callback)
    {
        base.OnEnter(callback);

        Application.LoadLevel("4.GameScene");
    }

    public override void OnExit(System.Action callback)
    {
        base.OnExit(callback);

        UIManager.instance.ClearPanel();
    }

    void OnLevelWasLoaded(int level)
    {
        if (Application.loadedLevelName == "4.GameScene")
        {
            switch(SceneManager.instance.SelectGame)
            {
                case eGame.BASEBALL:
                    GamePanel = UIManager.instance.SetPanel(ePanel.BaseballPanel2);
                    GamePanel.GetComponent<BaseballPanel2>().Init();
                    GameManager.instance.SetGame(eGame.BASEBALL);
                    break;

                case eGame.DIALSAFE:
                    GamePanel = UIManager.instance.SetPanel(ePanel.DialSafePanel);
                    GamePanel.GetComponent<DialSafePanel>().Init();
                    GameManager.instance.SetGame(eGame.DIALSAFE);
                    break;
            }
        }
    }
}
