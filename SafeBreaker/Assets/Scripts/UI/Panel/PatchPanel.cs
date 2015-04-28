using UnityEngine;
using System.Collections;

public class PatchPanel : PanelBase {

    GameObject PatchInfo;

    bool bPatch = false;

	// Use this for initialization
	void Awake () {
        PatchInfo = GameObject.Find("PatchInfo");

        NativeManager.instance.nativeSet();
        JsonLoader.instance.GetAuthJson();
	}

    void Start()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
            UIManager.instance.CreateOKPopup((int)eError.ERR_NETWORK);
    }
	
	// Update is called once per frame
	void Update () {
        if (bPatch)
            return;

        if(1 <= JsonLoader.instance.GetProgress())
        {
            bPatch = true;
            
            PatchInfo.GetComponent<UILabel>().text = "서비스 정보를 받고 있습니다.";
            WebSender.instance.P_SERVICE_INFO(recv =>
            {
                PatchInfo.GetComponent<UILabel>().text = "로그인 정보를 받고 있습니다.";
                WebSender.instance.P_USER_LOGIN(SystemInfo.deviceUniqueIdentifier, recv2 =>
                {
                    PatchInfo.GetComponent<UILabel>().text = "게임을 시작합니다.";
                    WebSender.instance.P_USER_INFO(recv3 =>
                    {
                        string name = DataManager.instance.GetName();
                        if (name == string.Empty)
                            SceneManager.instance.ActionEvent(eAction.NICKNAME);
                        else
                            Next();
                    });
                });

                // Test계정 생성
                //for (int i = 1; i < 15; ++i)
                //{
                //    WebSender.instance.P_USER_LOGIN(i.ToString(), recv2 =>
                //    {

                //    });
                //}
            });
        }
	}
}
