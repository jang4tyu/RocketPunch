using UnityEngine;
using System.Collections;

public class NativeManager : Immortal<NativeManager> {
	
	public bool AssetBundleUpdageFlag = true;
	public string AppDataPath = "";
	public string AppSdDataPath = "";
	
	public int jsonAssetcount = 0;
	public int jsoncount = 0;
	
	public string AppSdDataPathDetail = "/.RocketPunch";
	
	public string and_udid = "";
	public string and_model = "";
	public string and_version = "";
	public string and_macaddress = "";
	public string and_bundleversion = "";
	
	public string ios_bundleversion = "";
	
	public string gcmId = "";	
	
	public GameObject uiCameraCommon, uiCameraBack, uiCameraDefault, uiCameraPopup;	
	
	// Use this for initialization
	void _Start () {
        AppSdDataPathDetail = "/.RocketPunch";
		/*
#if !UNITY_EDITOR && UNITY_IPHONE
		//ios_bundleversion = EtceteraBinding.GetBundleVersion();
		//Debug.Log("ios_bundleversion : " + ios_bundleversion);
#endif
#if !UNITY_EDITOR && UNITY_ANDROID
		EtceteraManager.GcmIdCheckEvent  += GcmIdCheckEvent;
		EtceteraManager.AppDataPathEvent  += AppDataPathEvent;
		
		EtceteraBindingAndroid.CallPath();
		string devinfo = EtceteraBindingAndroid.RequestDeviceInfo();
		and_bundleversion = devinfo;
#else		
		AppDataPath = Application.persistentDataPath;
		Debug.Log(AppDataPath);
		//SceneManager.instance.Initialize();
#endif
		*/

        Debug.Log("AppDataPath " + Application.persistentDataPath);

		AppDataPath = Application.persistentDataPath;
		AppSdDataPath = Application.persistentDataPath;		
	}
	
	public void nativeSet()
	{
        AppSdDataPathDetail = "/.RocketPunch";
        AppDataPath = Application.persistentDataPath;
        AppSdDataPath = Application.persistentDataPath;	
    }
	
	void AppDataPathEvent(string path)
	{
		string[] paths = path.Split('_');
		AppDataPath = paths[0];
		
		AppSdDataPath = paths[1] + AppSdDataPathDetail;
		
		//CharacterGenerator.SetPath(AppDataPath);
		
		Debug.Log("AppDataPath : " + AppDataPath);
		Debug.Log("AppSdDataPath : " + AppSdDataPath);
		//SceneManager.instance.Initialize();
	}
	
	void GcmIdCheckEvent(string _gcmid)
	{
		Debug.Log("gcmid : " + _gcmid);
		gcmId = _gcmid;		
	}

    public float  GetProgress()
    {
        return (float)((JsonLoader.instance.GetProgress() + AssetDownLoad.instance.GetProgress()) / 2);
    }
}
