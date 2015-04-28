using UnityEngine;
using System.Collections;

public class OKPopupPanel : PanelBase 
{
    GameObject OKPopupDesc;
    GameObject OKBtn;

	// Use this for initialization
	public void Init (string desc) {
        OKPopupDesc = GameObject.Find("OKPopupDesc");
        OKBtn = GameObject.Find("OKBtn");

        UIEventListener.Get(OKBtn).onClick = OnClickOK;

        OKPopupDesc.GetComponent<UILabel>().text = desc;
	}
	
    void OnClickOK(GameObject GO)
    {
        Destroy(gameObject);
    }
}