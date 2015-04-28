using UnityEngine;
using System.Collections;
using System;
using System.Text;

public class UtilManager : Immortal<UtilManager> {

    AsyncOperation loading;
    string LevelName;
	public void LoadLevel(string _LevelName)
	{
        LevelName = _LevelName;
        StartCoroutine(Loading());
	}

    IEnumerator Loading()
    {
        yield return new WaitForSeconds(0.5f);

        if (Application.loadedLevelName != LevelName)
        {
            if (Application.CanStreamedLevelBeLoaded(LevelName))
            {
                loading = Application.LoadLevelAsync(LevelName);

                yield return loading;
            }
            else
            {
                Debug.Log(LevelName + " can't load");
            }
        }
    }

	public void DontDestroyLoad(GameObject GO)
	{
		DontDestroyOnLoad(GO);
	}
	
	public static void SetLayer(Transform T,int LayerNo)
	{
		for(int i=0;i<T.childCount;i++)
		{
			UtilManager.SetLayer(T.GetChild(i),LayerNo);
		}
	}

    public IEnumerator SetGameSpeed(float speed, float time)
    {
        Time.timeScale = speed;

        yield return new WaitForSeconds(time);

        //< 속도 정상으로 돌림
        Time.timeScale = 1;
    }

    static public string GetBase64string(string BaseString)
    {
        byte[] bytesToEncode = Encoding.UTF8.GetBytes(BaseString);
        return Convert.ToBase64String(bytesToEncode);
    }

    static public string GetUTF8string(string BaseString)
    {
        byte[] decodedBytes = Convert.FromBase64String(BaseString);
        return Encoding.UTF8.GetString(decodedBytes);
    }

    static public string GetCoinString(ulong coin)
    {
        string coinString = string.Empty;
        if (10 < coin)
            coinString = string.Format("{0:0,0}", coin);
        else
            coinString = coin.ToString();

        return coinString;
    }
}
