using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WheelManager : MonoBehaviour {

    public List<GameObject> WheelList;
    public List<UIWrapContent> UIWrap;

    public GameObject WheelItem;

    public void Init()
    {
    }
	// Use this for initialization
	void Start () 
    {
        int index = 0;
        foreach (var obj in WheelList)
        {
            for (int n = 1; n <= 9; n++)
            {
                GameObject Obj = GameObject.Instantiate(WheelItem) as GameObject;
                Obj.GetComponent<WheelItem>().SetNum(n);
                Obj.transform.parent = UIWrap[index].transform;
                Obj.transform.localScale = Vector3.one;
                Obj.transform.localPosition = Vector3.zero + Vector3.down * 60 * n;
            }

            UIWrap[index].GetComponentInParent<UIScrollView>().onDragFinished = Recenter;
            UIWrap[index].SortBasedOnScrollMovement();
            ++index;
        }
	}

    // Update is called once per frame
    void Recenter()
    {
        foreach(var wrap in UIWrap)
        {
            wrap.GetComponent<UICenterOnChild>().Recenter();
        }
	}

    public List<int> GetNumList()
    {
        List<int> NumList = new List<int>();
        foreach(var wrap in UIWrap)
        {
            int num = wrap.GetComponent<UICenterOnChild>().centeredObject.GetComponent<WheelItem>().Num;
            NumList.Add(num);
        }

        return NumList;
    }

    public void SetResult(int index, Color color)
    {
        GameObject obj = UIWrap[index].GetComponent<UICenterOnChild>().centeredObject.transform.FindChild("Label").gameObject;
        TweenColor tween = obj.GetComponent<TweenColor>();
        tween.from = color;
        tween.ResetToBeginning();
        tween.PlayForward();
    }

    public IEnumerator SetNumber(int index, int num)
    {
        yield return new WaitForEndOfFrame();

        Transform child = UIWrap[index].transform.GetChild(num);
        UIWrap[index].GetComponent<UICenterOnChild>().CenterOn(child);
    }
}
