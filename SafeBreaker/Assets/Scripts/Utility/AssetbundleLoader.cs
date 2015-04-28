using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AssetbundleLoader
{
    class AssetbundData
    {
        public List<System.Action<GameObject, ushort>> list = new List<System.Action<GameObject, ushort>>();
        public List<ushort> unitid = new List<ushort>();
    }

    protected static Dictionary<string, string> RealAssetNames = new Dictionary<string, string>();
    static Dictionary<string, GameObject> LoadedAssetbundleList = new Dictionary<string, GameObject>();
    static Dictionary<string, AssetbundData> LoadingAssetbundleList = new Dictionary<string, AssetbundData>();

    public static void AddRealAssetName(string key, string value)
    {
#if UNITY_IPHONE
        //< 아이폰에서는 ^ 이걸 빼버림
        int idx = value.IndexOf("^");
        value = value.Remove(idx, 1);
#endif

        //Debug.Log("key " + key +" value " + value);
        RealAssetNames.Add(key, value);
    }

    public static void AssetLoad(string assetName, ushort UnitId, System.Action<GameObject, ushort> loadedCallback)
    {
        if (!RealAssetNames.ContainsKey(assetName))
        {
            Debug.Log("cant find assetName " + assetName);
            return;
        }

        string realAssetName = RealAssetNames[assetName];

        //이미 로더 된것이 있으면 바로 넘겨 준다.
        if (LoadedAssetbundleList.ContainsKey(realAssetName))
        {
            loadedCallback(LoadedAssetbundleList[realAssetName], UnitId);
        }
        else
        {
            UtilManager.instance.StartCoroutine(LoadAssetbundle(realAssetName, UnitId, loadedCallback));
        }
    }

    static IEnumerator LoadAssetbundle(string _assetbundlename, ushort unitid, System.Action<GameObject, ushort> loadedCallback)
    {
        //현재 로더 하려는것이 로더중이면 딜리게이트만 담아 준다.
        if (LoadingAssetbundleList.ContainsKey(_assetbundlename))
        {
            LoadingAssetbundleList[_assetbundlename].list.Add(loadedCallback); //로딩 중일 또 들어오면 콜백함수는 두번 호출해 줘야 하니까.
            LoadingAssetbundleList[_assetbundlename].unitid.Add(unitid);
            yield return null;
        }
        else//현재 로더중이 아닐경우 로딩중 딕셔너리에 넣어 둔다.
        {
            LoadingAssetbundleList.Add(_assetbundlename, new AssetbundData());
            LoadingAssetbundleList[_assetbundlename].list.Add(loadedCallback);
            LoadingAssetbundleList[_assetbundlename].unitid.Add(unitid);

            string path = "file:///" + NativeManager.instance.AppDataPath + "/asset/" + _assetbundlename;

#if UNITY_STANDALONE_WIN
                path = "file:///" + Application.persistentDataPath + "/asset_Pc\\" + _assetbundlename;
#elif UNITY_ANDROID
            path = "file:///" + Application.persistentDataPath + "/asset/" + _assetbundlename;
#elif UNITY_IOS
                path = "file:///" + NativeManager.instance.AppDataPath + "/asset/" + _assetbundlename;
#endif            
            WWW www = new WWW(path);

            Debug.Log("asset path " + path);
            www.threadPriority = ThreadPriority.Low;
            yield return www;
            Debug.Log(path);

            if (www.isDone)
            {
                if (www.error != null)
                {
                    Debug.Log("www.error " + www.error);
                    yield break;
                }

                object[] tem = www.assetBundle.LoadAllAssets<GameObject>();

                List<GameObject> temlist = new List<GameObject>();

                foreach (object obj in tem)
                {
                    temlist.Add((GameObject)obj);
                }

                www.assetBundle.Unload(false);

                if (LoadedAssetbundleList.ContainsKey(_assetbundlename))
                {
                    Debug.LogError("already loaded " + _assetbundlename);
                }
                else
                {
                    //여기서 조작하면 될 듯 

                    LoadedAssetbundleList.Add(_assetbundlename, tem[0] as GameObject);
                }

                for (int i = 0; i < LoadingAssetbundleList[_assetbundlename].list.Count; i++)
                {
                    LoadingAssetbundleList[_assetbundlename].list[i](tem[0] as GameObject, LoadingAssetbundleList[_assetbundlename].unitid[i]);
                }

                LoadingAssetbundleList[_assetbundlename].list.Clear();
                LoadingAssetbundleList[_assetbundlename].unitid.Clear();
                LoadingAssetbundleList.Remove(_assetbundlename);

                www.Dispose();
            }
            else
            {
                Debug.Log("asset bundle load fail");
            }
        }
    }
}

