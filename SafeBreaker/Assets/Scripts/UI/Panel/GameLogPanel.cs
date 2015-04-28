using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogPanel : PanelBase 
{
    GameObject ScrollView;
    GameObject UIGrid;
    GameObject GameLogItem;

    GameObject DefenceBtn;
    GameObject AttackBtn;
    GameObject BackBtn;
    GameObject SettingBtn;

    GameObject Desc;

    string NoDefence = "아직 내 금고 털기를 시도한 사람이 없습니다.";
    string NoAttack = "아직 금고 털기를 시도 하지 않았습니다.";
	// Use this for initialization
	void Awake () 
    {
        ScrollView = GameObject.Find("ScrollView");
        UIGrid = GameObject.Find("UIGrid");

        DefenceBtn = GameObject.Find("DefenceBtn");
        AttackBtn = GameObject.Find("AttackBtn");
        BackBtn = GameObject.Find("BackBtn");
        SettingBtn = GameObject.Find("SettingBtn");

        Desc = GameObject.Find("Desc");

        UIEventListener.Get(BackBtn).onClick = OnClickBack;
        UIEventListener.Get(DefenceBtn).onClick = OnClickDefence;
        UIEventListener.Get(AttackBtn).onClick = OnClickAttack;
        UIEventListener.Get(SettingBtn).onClick = OnClickSetting;

        GameLogItem = Resources.Load("Prefabs/UI/Lobby/GameLogItem") as GameObject;
	}
	
    public void Init()
    {
        DataManager.instance.ResetGameLog();
        WebSender.instance.P_GAME_LOG(recv =>
        {
            OnClickDefence(null);
        });
    }

    void OnClickBack(GameObject GO)
    {
        ClearGridLog();
        SceneManager.instance.ActionEvent(eAction.PREV);
    }

    void OnClickSetting(GameObject GO)
    {
        ClearGridLog();
        SceneManager.instance.ActionEvent(eAction.NEXT);
    }

    void SetDesc(string desc)
    {
        Desc.GetComponent<UILabel>().text = desc;
    }

    void ClearGridLog()
    {
        foreach (var obj in UIGrid.GetComponent<UIGrid>().GetChildList())
        {
            UIGrid.GetComponent<UIGrid>().RemoveChild(obj.transform);
            DestroyImmediate(obj.gameObject);
        }
    }

    void OnClickDefence(GameObject GO)
    {
        SetDesc("");

        AttackBtn.GetComponent<UISprite>().color = Color.white;
        DefenceBtn.GetComponent<UISprite>().color = Color.yellow;

        ClearGridLog();

        List<GameLogInfo> LogList = DataManager.instance.GetLogList();
        if (LogList == null)
            return;

        foreach (var log in LogList)
        {
            if (log.target == DataManager.instance.GetID())
            {
                GameObject GameLog = GameObject.Instantiate(GameLogItem);
                GameLog.transform.SetParent(UIGrid.transform);
                GameLog.transform.localPosition = Vector3.zero;
                GameLog.transform.localScale = Vector3.one;
                GameLog.GetComponent<GameLogItem>().SetInfo(log, false);
                GameLog.GetComponent<GameLogItem>().SetMyDefence();

                UIGrid.GetComponent<UIGrid>().AddChild(GameLog.transform);
                UIGrid.GetComponent<UIGrid>().Reposition();
            }
        }

        if (UIGrid.GetComponent<UIGrid>().GetChildList().Count == 0)
            SetDesc(NoDefence);

        ScrollView.GetComponent<UIScrollView>().ResetPosition();
    }

    void OnClickAttack(GameObject GO)
    {
        SetDesc("");

        AttackBtn.GetComponent<UISprite>().color = Color.yellow;
        DefenceBtn.GetComponent<UISprite>().color = Color.white;

        ClearGridLog();

        List<GameLogInfo> LogList = DataManager.instance.GetLogList();
        if (LogList == null)
            return;

        foreach (var log in LogList)
        {
            if (log.attacker == DataManager.instance.GetID())
            {
                GameObject GameLog = GameObject.Instantiate(GameLogItem);
                GameLog.transform.SetParent(UIGrid.transform);
                GameLog.transform.localPosition = Vector3.zero;
                GameLog.transform.localScale = Vector3.one;
                GameLog.GetComponent<GameLogItem>().SetInfo(log, true);
                GameLog.GetComponent<GameLogItem>().SetMyAttack();

                UIGrid.GetComponent<UIGrid>().AddChild(GameLog.transform);
                UIGrid.GetComponent<UIGrid>().Reposition();
            }
        }

        if (UIGrid.GetComponent<UIGrid>().GetChildList().Count == 0)
            SetDesc(NoAttack);

        ScrollView.GetComponent<UIScrollView>().ResetPosition();
    }
}
