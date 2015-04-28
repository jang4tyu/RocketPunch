using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayLogInfo
{
    public int     num;
    public Color color;
}

public class LogManager : MonoBehaviour {

    GameObject LogGrid;
    GameObject ScrollView;
    GameObject LogRes;

    List<GameObject> LogList = new List<GameObject>();
	// Use this for initialization
	void Start () {
        LogGrid = GameObject.Find("LogGrid");
        ScrollView = GameObject.Find("ScrollView");

        LogRes = Resources.Load("Prefabs/Object/Baseball/LogItem") as GameObject;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init()
    {
        foreach (var log in LogList)
            Destroy(log);
    }

    public void AddLog(List<PlayLogInfo> Log)
    {
        GameObject addLog = GameObject.Instantiate(LogRes);
        UILabel label = addLog.transform.FindChild("Label").GetComponent<UILabel>();
        string log = string.Format("{0}{1}[-] {2}{3}[-] {4}{5}[-] {6}{7}[-]",
            GetColorTag(Log[0].color), Log[0].num,
            GetColorTag(Log[1].color), Log[1].num,
            GetColorTag(Log[2].color), Log[2].num,
            GetColorTag(Log[3].color), Log[3].num);
        label.text = log;

        addLog.transform.SetParent(LogGrid.transform);
        addLog.transform.localPosition = Vector3.zero;
        addLog.transform.localScale = Vector3.one;

        LogList.Add(addLog);

        LogGrid.GetComponent<UIGrid>().AddChild(addLog.transform);
        LogGrid.GetComponent<UIGrid>().Reposition();
        ScrollView.GetComponent<UIScrollView>().ResetPosition();
    }

    public void AddLog(string Log)
    {
        //GameObject addLog = GameObject.Instantiate(LogRes);
        //addLog.transform.SetParent(LogGrid.transform);
        //addLog.transform.localPosition = Vector3.zero;
        //addLog.transform.localScale = Vector3.one;
        //LogList.Add(addLog);

        //UILabel label = addLog.transform.FindChild("Label").GetComponent<UILabel>();
        //label.text = "[FF0000]" + Log;
        //LogGrid.GetComponent<UITable>().Reposition();
    }

    string GetColorTag(Color color)
    {
        string ColorTag = "[FFFFFF]";
        if (color == Color.green)
            ColorTag = "[66FA33]";

        else if (color == Color.red)
            ColorTag = "[FF0000]";

        else if (color == Color.yellow)
            ColorTag = "[FFFF00]";

        return ColorTag;
    }
}
