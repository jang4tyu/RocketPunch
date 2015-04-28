using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseballPanel : PanelBase {

    List<GameObject> NumberButton = new List<GameObject>();
    List<GameObject> SetNumber = new List<GameObject>();

    List<int> InputNumber = new List<int>();

    GameObject Timer;

    bool    bPlaying = false;
    float   MaxTime = 60.0f;
    float   CurTime = 0.0f;

	// Use this for initialization
	void Start () {
        Timer = GameObject.Find("ProgressBar");

        for(int n = 0 ; n < 10 ; ++n)
        {
            GameObject NumButton = GameObject.Find(n.ToString());
            NumberButton.Add(NumButton);
            UIEventListener.Get(NumButton).onClick = OnClickNumber;
        }

        for(int n = 1 ; n <= 4 ; ++n)
        {
            GameObject SetSprite = GameObject.Find("Set" + n.ToString());
            SetNumber.Add(SetSprite);
        }
	}

    void OnClickNumber(GameObject GO)
    {
        GO.GetComponent<BoxCollider>().enabled = false;
        GO.GetComponent<UISprite>().color = Color.red;

        int InputNum = int.Parse(GO.name);
        InputNumber.Add(InputNum);

        SetInputNumber();
    }

    void SetInputNumber()
    {
        for(int n = 0 ; n < InputNumber.Count ; ++n)
            SetNumber[n].GetComponent<UILabel>().text = InputNumber[n].ToString();

        if (InputNumber.Count == 4)
        {
            foreach(var num in InputNumber)
            {
                NumberButton[num].SetActive(true);
            }

            foreach(var Obj in SetNumber)
            {
                Obj.GetComponent<UILabel>().text = "?";
            }

            InputNumber.Clear();
        }
    }

    void ClearNumber()
    {
        foreach(var Obj in SetNumber)
            Obj.GetComponent<UILabel>().text = "?";

        InputNumber.Clear();
    }

    public void Init()
    {
        bPlaying = true;
        CurTime = 0.0f;
    }
	
	// Update is called once per frame
	void Update () 
    {
        if (!bPlaying)
            return;

        if (CurTime < MaxTime)
        {
            CurTime += Time.deltaTime;
            Timer.GetComponent<UISlider>().value = (float)(1.0f - CurTime / MaxTime);
        }

        else
        {
            bPlaying = false;
            SceneManager.instance.ActionEvent(eAction.NEXT);
        }
	}
}
