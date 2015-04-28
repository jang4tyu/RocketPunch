using UnityEngine;
using System.Collections;

public class WheelItem : MonoBehaviour {

    public int Num = 0;
    public UILabel NumLabel;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetNum(int num)
    {
        Num = num;
        NumLabel.text = num.ToString();
    }
}
