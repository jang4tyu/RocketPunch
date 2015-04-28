using UnityEngine;
using System.Collections;

public class RankItem : MonoBehaviour
{
    public UILabel Name;
    public UILabel Coin;
    public UILabel Ranking;
    public UILabel SafeLv;
    public UILabel LockLv;

    public UISprite BackGround;
    public UserInfo Info;

    public Color DefaultColor;

    // Use this for initialization
    void Start()
    {
        UIEventListener.Get(gameObject).onClick = OnClickItem;
        DefaultColor = BackGround.color;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnClickItem(GameObject GO)
    {
        if (GO.GetComponent<RankItem>().Name.text == DataManager.instance.GetName())
            return;

        ClearSelect();
        BackGround.color = Color.red;
        DataManager.instance.SetTarget(Info);
        UIManager.instance.ResetTarget();
    }

    void ClearSelect()
    {
        GameObject Grid = transform.parent.gameObject;
        foreach (var item in Grid.GetComponent<UIGrid>().GetChildList())
            item.FindChild("Sprite/BackGround").GetComponent<UISprite>().color = DefaultColor;
    }

    public void SetInfo(UserInfo user, int ranking)
    {
        Info = user;

        Name.text = user.name;
        Coin.text = UtilManager.GetCoinString(user.coin);
        Ranking.text = ranking.ToString() + ".";
        SafeLv.text = "Lv." + user.safelv;
        LockLv.text = "Lv." + user.locklv;
    }

    public void SetMyItem()
    {
        Name.GetComponent<UILabel>().color = Color.green;
        Coin.GetComponent<UILabel>().color = Color.green;
        Ranking.GetComponent<UILabel>().color = Color.green;
        SafeLv.GetComponent<UILabel>().color = Color.green;
        LockLv.GetComponent<UILabel>().color = Color.green;
    }
}