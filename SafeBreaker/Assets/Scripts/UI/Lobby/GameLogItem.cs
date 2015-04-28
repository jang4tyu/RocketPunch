using UnityEngine;
using System.Collections;

public class GameLogItem : MonoBehaviour {

    public UILabel Result;
    public UILabel Target;
    public UILabel Money;
    public UILabel Time;

    public UISprite Icon;
    public UISprite BackGround;

    public GameObject RevengeBtn;

    GameLogInfo Info = null;

	// Use this for initialization
	void Start () {
        UIEventListener.Get(RevengeBtn).onClick = OnClickRevenge;
	}

    void OnClickRevenge(GameObject GO)
    {
        UserInfo revenge_target = new UserInfo();
        revenge_target.user_id = Info.attacker;
        DataManager.instance.SetTarget(revenge_target);
        DataManager.instance.SetRevengeGameID(Info.game_id);
        UIManager.instance.CreateYNPopup("복수하러 가시겠습니까?", eYNPopup.REVENGE);
    }
	// Update is called once per frame
	void Update () {
	
	}

    public void SetInfo(GameLogInfo info, bool isAttack)
    {
        Info = info;

        string resString = string.Empty;
        if (isAttack && info.result == 1)
        {
            resString = "[66FA33]털기 성공";
            Target.text = "대상 : " + DataManager.instance.GetRankName(info.target);
            Money.text = string.Format("빼앗은 금액 : {0}", UtilManager.GetCoinString(info.money));
        }
            
        else if( isAttack && info.result == 0)
        {
            resString = "[FF0000]털기 실패";
            Target.text = "대상 : " + DataManager.instance.GetRankName(info.target);
            Money.text = "빼앗은 금액 : 0";
        }
            
        else if( !isAttack && info.result == 0)
        {
            resString = "[66FA33]금고 방어";
            Target.text = "도둑 : " + DataManager.instance.GetRankName(info.attacker);
            Money.text = "빼앗긴 금액 : 0";
        }

        else if (!isAttack && info.result == 1)
        {
            resString = "[FF0000]금고 털림";
            Target.text = "도둑 : " + DataManager.instance.GetRankName(info.attacker);
            Money.text = string.Format("빼앗긴 금액 : {0}", UtilManager.GetCoinString(info.money));
        }

        Result.text = "결과 : " + resString;
    }

    public void SetMyAttack()
    {
        RevengeBtn.SetActive(false);
    }

    public void SetMyDefence()
    {
        RevengeBtn.SetActive(Info.revenge == 0);
        Icon.spriteName = "Defense";
    }
}
