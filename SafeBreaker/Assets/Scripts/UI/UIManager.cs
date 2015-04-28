using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : Immortal<UIManager> 
{
    GameObject CurPanel = null;
    GameObject InfoPanel = null;
    GameObject PopupPanel = null;

    Dictionary<eError, string> ErrList = new Dictionary<eError, string>();
    Dictionary<ePanel, GameObject> PanelList = new Dictionary<ePanel, GameObject>();
    Dictionary<eNotify, string> NotifyList = new Dictionary<eNotify, string>();

    public bool isSafeFull = false;
    void Start()
    {
        ErrList.Add(eError.ERR_DB, "데이터베이스 에러!");
        ErrList.Add(eError.ERR_USER_NOTENOUGH_COIN, "보유금액이 부족합니다.");
        ErrList.Add(eError.ERR_GAME_LIMITTIME, "대상의 금고는 공격불가 상태입니다.\n\n(약탈 후 5분간)");
        ErrList.Add(eError.ERR_GAME_NOTARGET, "유저 목록에서 금고를 털\n\n대상을 선택 해 주세요.");
        ErrList.Add(eError.ERR_GAME_SAMEKEY, "같은 숫자를 2번 사용할 수 없습니다.\n\n다시 확인 해 주세요.");
        ErrList.Add(eError.ERR_USER_MAXUPGRADE, "더 이상 업그레이드 할 수 없습니다.");
        ErrList.Add(eError.ERR_USER_SAME_PW, "이전 비밀번호와 같은 번호입니다.");
        ErrList.Add(eError.ERR_NETWORK, "네트워크 접속이 원활하지 않습니다.\n\n확인 후 재시도 해주세요.");

        NotifyList.Add(eNotify.UPGRADE_SAFELV, "금고 레벨이 업그레이드 되었습니다.");
        NotifyList.Add(eNotify.UPGRADE_LOCKLV, "자물쇠 레벨이 업그레이드 되었습니다.");
        NotifyList.Add(eNotify.SAFE_FULL, "금고에서 보관할 수 있는\n한도가 초과 되었습니다.\n금고를 업그레이드 하지 않으면,\n획득한 금액을 잃게 됩니다.");
    }

    public void SetInfoPanel()
    {
        if (!InfoPanel)
            InfoPanel = GameObject.Instantiate(Resources.Load("Prefabs/UI/InfoPanel") as GameObject) as GameObject;
        else
            InfoPanel.GetComponent<InfoPanel>().SetInfo();
    }

    public GameObject SetPanel(ePanel name)
    {
        if (CurPanel != null)
            CurPanel.SetActive(false);

        if (PanelList.ContainsKey(name))
            CurPanel = PanelList[name];

        else
        {
            CurPanel = GameObject.Instantiate(Resources.Load("Prefabs/UI/" + name.ToString()) as GameObject) as GameObject;
            CurPanel.transform.SetParent(transform);
            CurPanel.transform.localScale = Vector3.one;
            PanelList.Add(name, CurPanel);
        }

        CurPanel.GetComponent<PanelBase>().PanelName = name;
        CurPanel.SetActive(true);

        return CurPanel;
    }

    public void ClearPanel()
    {
        PanelList.Remove(CurPanel.GetComponent<PanelBase>().PanelName);
        Destroy(CurPanel);
    }

    public void ResetInfo()
    {
        InfoPanel.GetComponent<InfoPanel>().SetInfo();
    }

    public void ResetTarget()
    {
        if (CurPanel == null)
            return;

        if(CurPanel.GetComponent<PanelBase>().PanelName == ePanel.LobbyPanel)
            CurPanel.GetComponent<LobbyPanel>().SetTargetInfo();
    }

    public void AddCoin(ulong coin)
    {
        InfoPanel.GetComponent<InfoPanel>().AddCoin(coin);
    }

    public void CreateOKPopup(int errno)
    {
        if (PopupPanel != null)
            Destroy(PopupPanel);

        PopupPanel = GameObject.Instantiate(Resources.Load("Prefabs/UI/OKPopupPanel") as GameObject) as GameObject;
        PopupPanel.GetComponent<OKPopupPanel>().Init(ErrList[(eError)errno]);
    }

    public void CreateNotifyPopup(eNotify type)
    {
        if (PopupPanel != null)
            Destroy(PopupPanel);

        PopupPanel = GameObject.Instantiate(Resources.Load("Prefabs/UI/OKPopupPanel") as GameObject) as GameObject;
        PopupPanel.GetComponent<OKPopupPanel>().Init(NotifyList[type]);
    }

    public void CreateNotifyPopup(string msg)
    {
        if (PopupPanel != null)
            Destroy(PopupPanel);

        PopupPanel = GameObject.Instantiate(Resources.Load("Prefabs/UI/OKPopupPanel") as GameObject) as GameObject;
        PopupPanel.GetComponent<OKPopupPanel>().Init(msg);
    }

    public void ChangePW()
    {
        if (CurPanel == null)
            return;

        if (CurPanel.GetComponent<PanelBase>().PanelName == ePanel.SettingPanel)
            CurPanel.GetComponent<SettingPanel>().ChangePW();
    }

    public void CreateYNPopup(string desc, eYNPopup type)
    {
        if (PopupPanel != null)
            DestroyImmediate(PopupPanel);

        PopupPanel = GameObject.Instantiate(Resources.Load("Prefabs/UI/YNPopupPanel") as GameObject) as GameObject;
        PopupPanel.GetComponent<YNPopupPanel>().Init(desc, type);
    }

    public void SetNotify()
    {
        InfoPanel.GetComponent<InfoPanel>().SetNotify();

        if (CurPanel.GetComponent<PanelBase>().PanelName == ePanel.LobbyPanel)
            CurPanel.GetComponent<LobbyPanel>().Init();
    }
}
