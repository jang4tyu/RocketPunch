using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseballPanel2 : PanelBase {

    GameObject PlayBtn;
    GameObject Timer;
    GameObject WheelManager;
    GameObject LogManager;
    GameObject ResultLabel;
    GameObject TryCountLabel;
    GameObject AlertRes;

    GameObject Alert;
    
    //3D
    GameObject SafeBox;
    GameObject Handle;

    bool    bPlaying = false;
    bool    bAlert = false;
    float   MaxTime = 30.0f;
    float   CurTime = 0.0f;
    byte    TryCount = 0;

	// Use this for initialization
	void Awake () 
    {
        Timer = GameObject.Find("ProgressBar");
        PlayBtn = GameObject.Find("PlayBtn");
        WheelManager = GameObject.Find("WheelManager");
        LogManager = GameObject.Find("LogManager");
        ResultLabel = GameObject.Find("ResultLabel");
        TryCountLabel = GameObject.Find("TryCountLabel");
        AlertRes = Resources.Load("Prefabs/UI/Baseball/TimeAlert") as GameObject;

        SafeBox = GameObject.Find("SafeBox");
        Handle = GameObject.Find("Handle");

        SafeBox.GetComponent<Animation>().Stop();
        Handle.GetComponent<Animation>().Stop();

        UIEventListener.Get(PlayBtn).onClick = OnPlayBtn;
	}

    public void Init()
    {
        bPlaying = true;
        CurTime = 0.0f;

        LogManager.GetComponent<LogManager>().Init();
        WheelManager.GetComponent<WheelManager>().Init();

        UserInfo target = DataManager.instance.GetTarget();
        if (target == null)
            return;

        MaxTime = LowData.lockUpgrade.DataInfoDic[target.locklv].limittime;
        TryCount = (byte)(LowData.lockUpgrade.DataInfoDic[target.locklv].limitvalue + 1);

        SetTryCount();
    }

	// Update is called once per frame
	void Update () 
    {
        if (!bPlaying)
            return;

        if (CurTime < MaxTime)
        {
            CurTime += Time.deltaTime;
            Timer.GetComponent<UISlider>().value = (float)(1.0f - CurTime / MaxTime);
        }

        else
            FailGame();

        if (MaxTime - CurTime < 5.0f && !bAlert)
        {
            bAlert = true;
            SetAlert();
        }
	}

    void FailGame()
    {
        bPlaying = false;

        GameObject obj = GameObject.Instantiate(Resources.Load("Prefabs/UI/ResultPopupPanel") as GameObject) as GameObject;
        obj.transform.SetParent(UIManager.instance.transform);
        obj.transform.localPosition = Vector3.zero;
        obj.transform.localScale = Vector3.one;
        obj.GetComponent<ResultPopupPanel>().Fail();
    }
    
    void OnPlayBtn(GameObject GO)
    {
        if (!bPlaying)
            return;

        List<int> NumberList = GetNumList();

        int numIndex = 0;
        foreach(var num in NumberList)
        {
            int num2Index = 0;
            foreach(var num2 in NumberList)
            {
                if (num == num2 && numIndex != num2Index)
                {
                    WheelManager.GetComponent<WheelManager>().SetResult(numIndex, Color.red);
                    WheelManager.GetComponent<WheelManager>().SetResult(num2Index, Color.red);
                    iTween.ShakePosition(WheelManager, new Vector3(0.03f, 0.03f, 0.03f), 0.3f);

                    return;
                }
                num2Index++;
            }

            numIndex++;
        }

        iTween.ShakePosition(SafeBox, new Vector3(0.03f, 0.03f, 0.03f), 0.3f);

        GameManager.instance.PlayBaseball(NumberList, result =>
        {
            int strikecount = 0;
            int index = 0;
            List<PlayLogInfo> PlayLog = new List<PlayLogInfo>();
            foreach(var res in result)
            {
                Color ResColor = Color.white;
                switch(res)
                {
                    case eBaseballResult.OUT:
                        ResColor = Color.red;
                        break;
                    case eBaseballResult.BALL:
                        ResColor = Color.yellow;
                        break;
                    case eBaseballResult.STRIKE:
                        ResColor = Color.green;
                        strikecount++;
                        break;
                }

                PlayLogInfo log = new PlayLogInfo();
                log.num = NumberList[index];
                log.color = ResColor;
                PlayLog.Add(log);

                WheelManager.GetComponent<WheelManager>().SetResult(index, ResColor);
                ++index;
            }

            if (strikecount == 4)
            {
                Handle.GetComponent<Animation>().Play("Open");
                bPlaying = false;
                ClearGame();
            }

            else
                Handle.GetComponent<Animation>().Play("Lock");

            SetTryCount();
            LogManager.GetComponent<LogManager>().AddLog(PlayLog);
        });
    }

    void SetTryCount()
    {
        if (TryCount <= 0)
            return;

        TryCount--;
        TryCountLabel.GetComponent<UILabel>().text = TryCount.ToString();

        if (2 < TryCount)
            TryCountLabel.GetComponent<UILabel>().color = Color.green;
        else if(TryCount == 2)
            TryCountLabel.GetComponent<UILabel>().color = Color.yellow;
        else
            TryCountLabel.GetComponent<UILabel>().color = Color.red;

        if (TryCount == 0 && bPlaying)
            FailGame();
    }

    public List<int> GetNumList()
    {
        return WheelManager.GetComponent<WheelManager>().GetNumList();
    }

    public void ClearGame()
    {
        if (Alert != null)
            Destroy(Alert);

        SafeBox.GetComponent<Animation>().Play(PlayMode.StopAll);

        WebSender.instance.P_GAME_RESULT(DataManager.instance.GetGameID(), (byte)(TryCount - 1), recv =>
        {
            if (LowData.safeUpgrade.DataInfoDic[DataManager.instance.GetSafeLv()].maxcoin == DataManager.instance.GetCoin())
                UIManager.instance.isSafeFull = true;

            int AddCoin = (int)DataManager.instance.GetAddCoin();
            int BonusCoin = TryCount * LowData.master.DataInfoList[0].bonuscoin;
            if (AddCoin < BonusCoin)
                BonusCoin = 0;

            AddCoin -= BonusCoin;


            GameObject obj = GameObject.Instantiate(Resources.Load("Prefabs/UI/ResultPopupPanel") as GameObject) as GameObject;
            obj.transform.SetParent(UIManager.instance.transform);
            obj.transform.localPosition = Vector3.zero;
            obj.transform.localScale = Vector3.one;
            obj.GetComponent<ResultPopupPanel>().Success(AddCoin, BonusCoin);

            UIManager.instance.AddCoin(DataManager.instance.GetAddCoin());
        });
    }

    void SetAlert()
    {
        Alert = GameObject.Instantiate(AlertRes) as GameObject;
        Alert.transform.SetParent(transform.FindChild("Center"));
        Alert.transform.localPosition = Vector3.zero;
        Alert.transform.localScale = Vector3.one;
    }
}