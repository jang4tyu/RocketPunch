using UnityEngine;
using System.Collections;

public delegate void NetCallBack(JSONObject jsonObj);

public class Auth
{
    public static string authURL = "http://112.169.56.66:8080/";

    public static string ActionUrl { get { return m_szActionUrl; } }
    public static string ImageLoadUrl { get { return m_szImageLoadUrl; } }
    public static string ImageUploadUrl { get { return m_szImageUploadUrl; } }
    public static string AndroidAssetUrl { get { return m_szAndroidAssetUrl; } }
    public static string IosAssetUrl { get { return m_szIosAssetUrl; } }
    public static string JsonUrl { get { return m_szJsonUrl; } }
    public static string PhotoDelUrl { get { return m_szPhotoDelUrl; } }

    private static string m_szActionUrl = ""; // real domain
    private static string m_szImageLoadUrl = "";
    private static string m_szImageUploadUrl = "";
    private static string m_szAndroidAssetUrl = "";
    private static string m_szWindowsAssetUrl = "";
    private static string m_szIosAssetUrl = "";
    private static string m_szJsonUrl = "";
    private static string m_szPhotoDelUrl = "";

    public static void SetActionUrl(string _url)
    {        
        m_szActionUrl = "http://" + _url;        
    }
    public static string GetActionUrl()
    {
        return m_szActionUrl;
    }
    public static void SetAssetUrl(string _url)
    {
		if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor)
        {
            m_szAndroidAssetUrl = string.Format("http://{0}/asset/android/", _url);
        }
#if UNITY_IPHONE
		m_szIosAssetUrl = string.Format("http://{0}/asset/ios/", _url);
#endif
#if UNITY_STANDALONE
        m_szWindowsAssetUrl = string.Format("http://{0}/asset/windows/", _url);
#endif
        m_szJsonUrl = string.Format("http://{0}/json/", _url);        
    }
    public static void SetUploadPhotoUrl(string _url)
    {
        m_szImageUploadUrl = string.Format("http://{0}/upload_photo.php", _url);
    }
    public static void SetDelPhotoUrl(string _url)
    {
        m_szPhotoDelUrl = string.Format("http://{0}/delete_photo.php?uuid=", _url);
    }

    public static string GetAssetUrl(ASSETS asset)
    {
        switch (asset)
        {
            case ASSETS.android_asset:
                return m_szAndroidAssetUrl;
            case ASSETS.json:
                return m_szJsonUrl;
            case ASSETS.windows_asset:
                return m_szWindowsAssetUrl;
            default:
                return "error";
        }
    }
}
