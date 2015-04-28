using UnityEngine;
using System.Collections;

public class YNPopupPanel : PanelBase 
{
    GameObject YNPopupDesc;
    GameObject YesBtn;
    GameObject NoBtn;

    eYNPopup PopupType = eYNPopup.ATTACK;

	// Use this for initialization
	public void Init (string desc, eYNPopup type) 
    {
        PopupType = type;

        YNPopupDesc = GameObject.Find("YNPopupDesc");
        YesBtn = GameObject.Find("YesBtn");
        NoBtn = GameObject.Find("NoBtn");

        UIEventListener.Get(YesBtn).onClick = OnClickYes;
        UIEventListener.Get(NoBtn).onClick = OnClickNo;

        YNPopupDesc.GetComponent<UILabel>().text = desc;
	}

    void OnClickYes(GameObject GO)
    {
        Destroy(gameObject);

        switch (PopupType)
        {
            case eYNPopup.ATTACK:
                WebSender.instance.P_GAME_START(DataManager.instance.GetTarget().user_id, recv =>
                {
                    if ((int)recv["errno"].n == 0)
                        SceneManager.instance.ActionEvent(eAction.NEXT);
                });
                break;

            case eYNPopup.REVENGE:
                WebSender.instance.P_GAME_REVENGE(DataManager.instance.GetTarget().user_id, DataManager.instance.GetRevengeGameID(), recv =>
                {
                    if ((int)recv["errno"].n == 0)
                        SceneManager.instance.ActionEvent(eAction.REVENGE);
                });
                break;

            case eYNPopup.CHANGE_PW:
                UIManager.instance.ChangePW();
                break;
        }
    }

    void OnClickNo(GameObject GO)
    {
        switch (PopupType)
        {
            case eYNPopup.ATTACK:
            case eYNPopup.REVENGE:
            case eYNPopup.CHANGE_PW:
                Destroy(gameObject);
                break;
        }
    }
}