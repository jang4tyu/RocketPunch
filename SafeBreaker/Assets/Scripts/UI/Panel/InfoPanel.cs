
using UnityEngine;
using System.Collections;

public class InfoPanel : PanelBase 
{
    GameObject Name;
    GameObject Coin;
    GameObject AddCoinRes;

    public GameObject Siren;
    public UILabel SirenDesc;

    float UpdateTime = 5.0f;
    float CheckTime = 0.0f;

	// Use this for initialization
	void Start () {
        Name = GameObject.Find("Name");
        Coin = GameObject.Find("Coin");

        AddCoinRes = Resources.Load("Prefabs/Object/Baseball/AddCoin") as GameObject;

        SetInfo();
	}
	
	// Update is called once per frame
	void Update () {
	    if(UpdateTime <= CheckTime)
        {
            WebSender.instance.P_USER_NOTIFY(recv =>
            {
            });
            CheckTime = 0.0f;
        }

        CheckTime += Time.deltaTime;
	}

    public void SetInfo()
    {
        Name.GetComponent<UILabel>().text = DataManager.instance.GetName();
        Coin.GetComponent<UILabel>().text = UtilManager.GetCoinString(DataManager.instance.GetCoin());
    }

    public void AddCoin(ulong coin)
    {
        GameObject AddCoin = GameObject.Instantiate(AddCoinRes);
        AddCoin.transform.SetParent(transform.FindChild("TopRight"));
        AddCoin.transform.localPosition = Vector3.zero;
        AddCoin.transform.localScale = Vector3.one;
        AddCoin.GetComponent<UILabel>().text = "+" + coin.ToString();
    }

    public void SetNotify()
    {
        NotifyLog log = DataManager.instance.GetNotifyLog();
        if (log == null)
            return;

        ulong Coin = DataManager.instance.GetCoin();
        DataManager.instance.SetCoin(Coin - log.coin);

        SirenDesc.text = DataManager.instance.GetRankName(log.attacker_id) + " 에게 내 금고를 털렸습니다.\n보유금액이 감소 되었습니다.";

        TweenPosition[] tweens = Siren.GetComponents<TweenPosition>();
        foreach(var tween in tweens)
        {
            tween.ResetToBeginning();
            tween.PlayForward();
        }

        Siren.SetActive(true);
    }

    public void HideNotify()
    {
        Siren.SetActive(false);
    }
}
