using UnityEngine;
using System.Collections;

public class ResultPopupPanel : PanelBase 
{
    GameObject ResultPopupDesc;
    GameObject OKBtn;

    string SuccessText = "[약탈 성공]\n\n총 획득 금액 : {0}\n - 약탈 금액 : {1}\n - 보너스 금액 : {2}";
    string FailText = "[약탈 실패]\n\n금고 털기에 실패 했습니다.\n내 금고가 상대방에게 노출 되었으니\n주의 해 주세요.";

	// Use this for initialization
    public void Success(int AddCoin, int BonusCoin)
    {
        ResultPopupDesc = GameObject.Find("ResultPopupDesc");
        OKBtn = GameObject.Find("OKBtn");

        UIEventListener.Get(OKBtn).onClick = OnClickOK;

        ResultPopupDesc.GetComponent<UILabel>().text = string.Format(SuccessText, UtilManager.GetCoinString((ulong)(AddCoin + BonusCoin)), UtilManager.GetCoinString((ulong)AddCoin), UtilManager.GetCoinString((ulong)BonusCoin));
    }

    public void Fail()
    {
        ResultPopupDesc = GameObject.Find("ResultPopupDesc");
        OKBtn = GameObject.Find("OKBtn");

        UIEventListener.Get(OKBtn).onClick = OnClickOK;

        ResultPopupDesc.GetComponent<UILabel>().text = FailText;
    }
	
    void OnClickOK(GameObject GO)
    {
        Destroy(gameObject);
        Next();
    }
}