using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Baseball : MonoBehaviour {

    List<int> NumberList = new List<int>();

	// Use this for initialization
	void Start () {
        //for (int n = 0; n < 10; ++n)
        //{
        //    NumberList.Add(n);
        //}

        //ShuffleList();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void Init()
    {
        //ShuffleList();
        NumberList.Clear();

        string target_pw = DataManager.instance.GetTargetPW();
        for (int n = 0; n < 4; ++n)
            NumberList.Add(int.Parse(target_pw[n].ToString()));
    }

    void ShuffleList()
    {
        NumberList.Sort((x, y) => Random.value < 0.5f ? -1 : 1);
    }

    public void SetPlay(List<int> SetList, BaseballCallback callback)
    {
        int index = 0;
        List<eBaseballResult> Result = new List<eBaseballResult>();
        foreach(var num in SetList)
        {
            eBaseballResult result = CheckNum(num, index);
            Result.Add(result);
            ++index;
        }

        callback(Result);
    }

    eBaseballResult CheckNum(int num, int index)
    {
        int numIndex = 0;
        foreach (var number in NumberList)
        {
            if (numIndex == 4)
                break;

            if (num == number)
            {
                if (numIndex == index)
                    return eBaseballResult.STRIKE;

                else
                    return eBaseballResult.BALL;
            }
            ++numIndex;
        }

        return eBaseballResult.OUT;
    }
}