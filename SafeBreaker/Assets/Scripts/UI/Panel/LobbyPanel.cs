using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LobbyPanel : PanelBase 
{
    List<GameObject> RankItemList = new List<GameObject>();

    GameObject ScrollView;
    GameObject UIGrid;
    GameObject RankItem;
    GameObject GameLogBtn;
    GameObject SettingBtn;
    GameObject AttackBtn;
    GameObject NoticeBtn;
    GameObject TargetName;
    GameObject TargetMoney;
    GameObject AttackCost;
    GameObject AttackMoney;

	// Use this for initialization
	void Awake () {
        ScrollView = GameObject.Find("ScrollView");
        UIGrid = GameObject.Find("UIGrid");
        GameLogBtn = GameObject.Find("GameLogBtn");
        SettingBtn = GameObject.Find("SettingBtn");
        AttackBtn = GameObject.Find("AttackBtn");
        NoticeBtn = GameObject.Find("NoticeBtn");

        TargetName = GameObject.Find("TargetName");
        TargetMoney = GameObject.Find("TargetMoney");
        AttackCost = GameObject.Find("AttackCost");
        AttackMoney = GameObject.Find("AttackMoney");

        RankItem = Resources.Load("Prefabs/UI/Lobby/RankItem") as GameObject;

        UIEventListener.Get(GameLogBtn).onClick = OnClickGameLog;
        UIEventListener.Get(SettingBtn).onClick = OnClickSetting;
        UIEventListener.Get(AttackBtn).onClick = OnClickAttack;
        UIEventListener.Get(NoticeBtn).onClick = OnClickNotice;
	}

    public void Init()
    {
        foreach (var obj in RankItemList)
            Destroy(obj);

        RankItemList.Clear();

        DataManager.instance.ResetRank();
        WebSender.instance.P_USER_LIST(recv =>
        {
            Dictionary<int, UserInfo> RankList = DataManager.instance.GetRankList();
            if (RankList == null)
                return;

            foreach (var user in RankList)
            {
                UserInfo info = user.Value;
                GameObject RankUser = GameObject.Instantiate(RankItem);
                RankUser.transform.SetParent(UIGrid.transform);
                RankUser.transform.localPosition = Vector3.zero;
                RankUser.transform.localScale = Vector3.one;
                RankUser.GetComponent<RankItem>().SetInfo(user.Value, user.Key);
                if (user.Value.user_id == DataManager.instance.GetID())
                {
                    RankUser.GetComponent<RankItem>().SetMyItem();
                    DataManager.instance.SetCoin(user.Value.coin);
                }

                RankItemList.Add(RankUser);
            }

            UIGrid.GetComponent<UIGrid>().Reposition();
            ScrollView.GetComponent<UIScrollView>().ResetPosition();

            SetTargetInfo();
        });

        if (UIManager.instance.isSafeFull)
        {
            UIManager.instance.CreateNotifyPopup(eNotify.SAFE_FULL);
            UIManager.instance.isSafeFull = false;
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetTargetInfo()
    {
        UserInfo target = DataManager.instance.GetTarget();
        if (target != null && target.user_id != 0)
        {
            TargetName.GetComponent<UILabel>().text = "공격 대상: " + target.name;
            TargetMoney.GetComponent<UILabel>().text = string.Format("보유 금액: {0}", UtilManager.GetCoinString(target.coin));
            ulong Money = target.coin * LowData.master.DataInfoList[0].rewardrate / 100;
            AttackMoney.GetComponent<UILabel>().text = string.Format("약탈 금액: {0}", UtilManager.GetCoinString(Money));
            ulong Cost = target.coin * LowData.master.DataInfoList[0].attackcoin / 100;
            if (LowData.master.DataInfoList[0].attackcoinlimit < Cost)
                Cost = LowData.master.DataInfoList[0].attackcoinlimit;
            AttackCost.GetComponent<UILabel>().text = string.Format("비용: {0}", UtilManager.GetCoinString(Cost));
            
        }

        else
        {
            TargetName.GetComponent<UILabel>().text = "공격 대상: 선택 없음";
            TargetMoney.GetComponent<UILabel>().text = "보유 금액: 정보 없음";
            AttackMoney.GetComponent<UILabel>().text = "약탈 금액: 정보 없음";
            AttackCost.GetComponent<UILabel>().text = "비용: 0";
        }
    }

    void OnClickStart(GameObject GO)
    {
        //SceneManager.instance.SelectGame = eGame.BASEBALL;
        //Next();
        //UIManager.instance.CreateYNPopup("500 금액을 소모하여 금고를\n\n터시겠습니까?", eYNPopup.ATTACK);
    }

    void OnClickGameLog(GameObject GO)
    {
        SceneManager.instance.ActionEvent(eAction.GAMELOG);
    }

    void OnClickSetting(GameObject GO)
    {
        SceneManager.instance.ActionEvent(eAction.SETTING);
    }

    void OnClickAttack(GameObject GO)
    {
        UserInfo target = DataManager.instance.GetTarget();
        if (target == null || target.user_id == 0)
        {
            UIManager.instance.CreateOKPopup((int)eError.ERR_GAME_NOTARGET);
            return;
        }

        ulong Cost = target.coin * LowData.master.DataInfoList[0].attackcoin / 100;
        if (LowData.master.DataInfoList[0].attackcoinlimit < Cost)
            Cost = LowData.master.DataInfoList[0].attackcoinlimit;

        UIManager.instance.CreateYNPopup(string.Format("{0} 금액을 소모하여 금고를\n\n터시겠습니까?", UtilManager.GetCoinString(Cost)), eYNPopup.ATTACK);
    }

    void OnClickNotice(GameObject GO)
    {
        GameObject obj = GameObject.Instantiate(Resources.Load("Prefabs/UI/NoticePopupPanel") as GameObject) as GameObject;
        obj.transform.SetParent(UIManager.instance.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<NoticePopupPanel>().Init();
    }
}
