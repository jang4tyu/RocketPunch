using UnityEngine;
using System.Collections;

public class PopupPanel : PanelBase 
{
    GameObject PopupDesc;
    GameObject OKBtn;

	// Use this for initialization
	public void Init (string desc) {
        PopupDesc = GameObject.Find("PopupDesc");
        OKBtn = GameObject.Find("OKBtn");

        UIEventListener.Get(OKBtn).onClick = OnClickOK;

        PopupDesc.GetComponent<UILabel>().text = desc;
	}
	
    void OnClickOK(GameObject GO)
    {
        Destroy(gameObject);
    }
}