using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SettingPanel : PanelBase 
{
    GameObject BackBtn;
    GameObject GameLogBtn;
    GameObject LockBtn;

    GameObject SafeUpBtn;
    GameObject LockUpBtn;

    GameObject WheelManager;

    GameObject SafeCost;
    GameObject SafeLv;
    GameObject MaxCoin;

    GameObject LockCost;
    GameObject LockLv;
    GameObject MaxCount;
    GameObject LimitTime;
    GameObject LimitValue;

    GameObject SafeInfo;
    GameObject LockInfo;

    //3D
    GameObject SafeBox;

	// Use this for initialization
	void Awake () 
    {
        BackBtn = GameObject.Find("BackBtn");
        GameLogBtn = GameObject.Find("GameLogBtn");
        LockBtn = GameObject.Find("LockBtn");
        SafeUpBtn = GameObject.Find("SafeUpBtn");
        LockUpBtn = GameObject.Find("LockUpBtn");

        WheelManager = GameObject.Find("WheelManager");

        SafeCost = GameObject.Find("SafeCost");
        SafeLv = GameObject.Find("SafeLv");
        MaxCoin = GameObject.Find("MaxCoin");

        LockCost = GameObject.Find("LockCost");
        LockLv = GameObject.Find("LockLv");
        MaxCount = GameObject.Find("MaxCount");
        LimitTime = GameObject.Find("LimitTime");
        LimitValue = GameObject.Find("LimitValue");

        SafeInfo = GameObject.Find("SafeInfo");
        LockInfo = GameObject.Find("LockInfo");

        SafeBox = GameObject.Find("SafeBox");

        UIEventListener.Get(BackBtn).onClick = OnClickBack;
        UIEventListener.Get(GameLogBtn).onClick = OnClickGameLog;
        UIEventListener.Get(LockBtn).onClick = OnClickLock;
        UIEventListener.Get(SafeUpBtn).onClick = OnClickSafeUp;
        UIEventListener.Get(LockUpBtn).onClick = OnClickLockUp;
	}
	
    public void Init()
    {
        SafeBox.GetComponent<Animation>()["C4D Animation Take"].speed = 1;
        SafeBox.GetComponent<Animation>().Rewind();

        string password = DataManager.instance.GetPassword();
        for (int n = 0; n < 4; ++n)
            StartCoroutine(WheelManager.GetComponent<WheelManager>().SetNumber(n, int.Parse(password[n].ToString())));

        SafeInfo.GetComponent<TweenPosition>().ResetToBeginning();
        SafeInfo.GetComponent<TweenPosition>().PlayForward();

        LockInfo.GetComponent<TweenPosition>().ResetToBeginning();
        LockInfo.GetComponent<TweenPosition>().PlayForward();

        ResetLevelInfo();
    }

    void ResetLevelInfo()
    {
        byte SafeLevel = DataManager.instance.GetSafeLv();
        SafeLv.GetComponent<UILabel>().text = string.Format("금고 Lv {0:00}", SafeLevel);
        SafeCost.GetComponent<UILabel>().text = string.Format("{0}", UtilManager.GetCoinString(LowData.safeUpgrade.DataInfoDic[SafeLevel].cost));
        MaxCoin.GetComponent<UILabel>().text = string.Format("최대 보유액 {0}", UtilManager.GetCoinString(LowData.safeUpgrade.DataInfoDic[SafeLevel].maxcoin));

        byte LockLevel = DataManager.instance.GetLockLv();
        LockLv.GetComponent<UILabel>().text = string.Format("자물쇠 Lv {0:00}", LockLevel);
        LockCost.GetComponent<UILabel>().text = string.Format("{0}", UtilManager.GetCoinString(LowData.lockUpgrade.DataInfoDic[LockLevel].cost));
        MaxCount.GetComponent<UILabel>().text = string.Format("자물쇠 비밀번호 {0}자리", LowData.lockUpgrade.DataInfoDic[LockLevel].maxcount);
        LimitTime.GetComponent<UILabel>().text = string.Format("자물쇠 해제시간 {0:00}초", LowData.lockUpgrade.DataInfoDic[LockLevel].limittime);
        LimitValue.GetComponent<UILabel>().text = string.Format("자물쇠 해제회수 {0:00}회", LowData.lockUpgrade.DataInfoDic[LockLevel].limitvalue);
    }

    public void ChangePW()
    {
        List<int> NumberList = WheelManager.GetComponent<WheelManager>().GetNumList();
        if (NumberList != null)
        {
            int numIndex = 0;
            foreach (var num in NumberList)
            {
                int num2Index = 0;
                foreach (var num2 in NumberList)
                {
                    if (num == num2 && numIndex != num2Index)
                    {
                        WheelManager.GetComponent<WheelManager>().SetResult(numIndex, Color.red);
                        WheelManager.GetComponent<WheelManager>().SetResult(num2Index, Color.red);
                        iTween.ShakePosition(SafeBox, new Vector3(0.03f, 0.03f, 0.03f), 0.3f);

                        UIManager.instance.CreateOKPopup((int)eError.ERR_GAME_SAMEKEY);

                        return;
                    }

                    num2Index++;
                }

                numIndex++;
            }

            for (int n = 0; n < 4; ++n)
                WheelManager.GetComponent<WheelManager>().SetResult(n, Color.green);

            string numString = string.Empty;
            foreach (var num in NumberList)
                numString += num.ToString();

            if( numString == DataManager.instance.GetPassword())
            {
                UIManager.instance.CreateOKPopup((int)eError.ERR_USER_SAME_PW);
                return;
            }

            WebSender.instance.P_USER_SETPW(numString, recv =>
            {
                SafeBox.GetComponent<Animation>()["C4D Animation Take"].speed = -1.5f;
                SafeBox.GetComponent<Animation>()["C4D Animation Take"].time = SafeBox.GetComponent<Animation>()["C4D Animation Take"].length - 0.5f;
                SafeBox.GetComponent<Animation>().Play();
            });
        }
    }

    void OnClickBack(GameObject GO)
    {
        SceneManager.instance.ActionEvent(eAction.PREV);
    }

    void OnClickGameLog(GameObject GO)
    {
        SceneManager.instance.ActionEvent(eAction.NEXT);
    }


    void OnClickLock(GameObject GO)
    {
        int Cost = LowData.master.DataInfoList[0].passwordcoin;
        UIManager.instance.CreateYNPopup(Cost + " 금액을 소모하여 비밀번호를\n\n바꾸시겠습니까?", eYNPopup.CHANGE_PW);
    }

    void OnClickSafeUp(GameObject GO)
    {
        byte SafeLv = DataManager.instance.GetSafeLv();
        if (LowData.safeUpgrade.DataInfoDic.Count <= SafeLv)
        {
            UIManager.instance.CreateOKPopup((int)eError.ERR_USER_MAXUPGRADE);
            return;
        }

        if (DataManager.instance.GetCoin() < LowData.safeUpgrade.DataInfoDic[SafeLv].cost)
        {
            UIManager.instance.CreateOKPopup((int)eError.ERR_USER_NOTENOUGH_COIN);
            return;
        }

        WebSender.instance.P_USER_UPGRADE((byte)eUpgrade.SAFE_LV, recv =>
        {
            UIManager.instance.CreateNotifyPopup(LowData.safeUpgrade.DataInfoDic[SafeLv].desc);
            ResetLevelInfo();
        });
    }

    void OnClickLockUp(GameObject GO)
    {
        byte LockLv = DataManager.instance.GetLockLv();
        if (LowData.lockUpgrade.DataInfoDic.Count <= LockLv)
        {
            UIManager.instance.CreateOKPopup((int)eError.ERR_USER_MAXUPGRADE);
            return;
        }

        if (DataManager.instance.GetCoin() < LowData.lockUpgrade.DataInfoDic[LockLv].cost)
        {
            UIManager.instance.CreateOKPopup((int)eError.ERR_USER_NOTENOUGH_COIN);
            return;
        }

        WebSender.instance.P_USER_UPGRADE((byte)eUpgrade.LOCK_LV, recv =>
        {
            UIManager.instance.CreateNotifyPopup(LowData.lockUpgrade.DataInfoDic[LockLv].desc);
            ResetLevelInfo();
        });
    }
}
