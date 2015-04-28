using UnityEngine;
using System.Collections;

public class NoticePopupPanel : PanelBase 
{
    GameObject NoticePopupDesc;
    GameObject OKBtn;

	// Use this for initialization
	public void Init () {
        NoticePopupDesc = GameObject.Find("NoticePopupDesc");
        OKBtn = GameObject.Find("OKBtn");

        UIEventListener.Get(OKBtn).onClick = OnClickOK;
	}
	
    void OnClickOK(GameObject GO)
    {
        Destroy(gameObject);
    }
}