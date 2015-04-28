using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : Singleton<GameManager>
{
    GameObject CurGame;

    public void SetGame(eGame Game)
    {
        CurGame = GameObject.Instantiate(Resources.Load("Prefabs/Object/Manager/GameManager") as GameObject) as GameObject;

        switch(Game)
        {
            case eGame.BASEBALL:
                Baseball baseball = CurGame.AddComponent<Baseball>();
                baseball.Init();
                break;

            case eGame.DIALSAFE:
                DialSafe dialsafe = CurGame.AddComponent<DialSafe>();
                dialsafe.Init();
                break;
        }
    }

    public void PlayBaseball(List<int> NumList, BaseballCallback callback)
    {
        CurGame.GetComponent<Baseball>().SetPlay(NumList, callback);
    }
}