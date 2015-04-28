using UnityEngine;
using System.Collections;

public class NicknamePanel : PanelBase 
{
    GameObject OKBtn;
    GameObject Name;

    public void Init()
    {
        OKBtn = GameObject.Find("OKBtn");
        Name = GameObject.Find("Name");

        UIEventListener.Get(OKBtn).onClick = OnOK;
    }

    void OnOK(GameObject GO)
    {
        string name = Name.GetComponent<UILabel>().text;
        if (name == string.Empty)
            return;

        WebSender.instance.P_USER_NICKNAME(name, recv =>
        {
            Next();
        });
    }
}
