using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.ComponentModel;

using System.Text;

using System.IO;

using System.Net;


public class AssetDownLoad : Singleton<AssetDownLoad>
{
    static Dictionary<string, string> localAssetDic = new Dictionary<string, string>();

    Dictionary<string, string> serverAssetDic = new Dictionary<string, string>();

    public int file_count = 0;

    public bool isLoad = false;
    public bool isABSoundLoad = false;

    public int localcount = 0;

    string nativeAppdatapath = "";
    //string nativeAppSddatapath = "";

    int randcount = 0;
    WWW www;

    List<string> assetlist = new List<string>();

    List<string> soundassetlist = new List<string>();

    string folderName = "";

    public void SetLocaclVersionInfo()
    {
        nativeAppdatapath = NativeManager.instance.AppDataPath;
        //nativeAppSddatapath = NativeManager.instance.AppSdDataPath;
#if !UNITY_EDITOR && UNITY_ANDROID
		nativeAppdatapath = NativeManager.instance.AppDataPath;
#endif

#if !UNITY_EDITOR || UNITY_IPHONE
        //nativeAppSddatapath = NativeManager.instance.AppDataPath;
#endif

        folderName = "/asset";
#if UNITY_STANDALONE_WIN
        folderName = "/asset_Pc";
#endif
        if (!System.IO.Directory.Exists(nativeAppdatapath + folderName))
        {
            System.IO.Directory.CreateDirectory(nativeAppdatapath + folderName);
        }
        CompareToLocalVersion();
    }

    void assetLoaderCheck()
    {
        AssetbundmgrSet();

        localcount++;

    }

    public void Local_LoadAssetFileCo()
    {
        nativeAppdatapath = NativeManager.instance.AppDataPath;
        //nativeAppSddatapath = NativeManager.instance.AppSdDataPath;
#if !UNITY_EDITOR && UNITY_ANDROID
		nativeAppdatapath = NativeManager.instance.AppDataPath;
#endif

#if !UNITY_EDITOR || UNITY_IPHONE
        //nativeAppSddatapath = NativeManager.instance.AppDataPath;
#endif

        folderName = "/asset";
#if UNITY_STANDALONE_WIN
        folderName = "/asset_Pc";
#endif

        string longPath = nativeAppdatapath + folderName;
        if (!System.IO.Directory.Exists(nativeAppdatapath + folderName))
        {
            Debug.LogError("Can't find Assetbundle folder!!!");
            return;
        }

        string[] FileNames = System.IO.Directory.GetFiles(longPath);
        for (int i = 0; i < FileNames.Length; ++i)
        {
            string pathName = FileNames[i];
            if (string.IsNullOrEmpty(pathName))
                continue;

            string[] splitName1 = pathName.Split(new char[] { '/', '\\' });
            if (splitName1.Length <= 0)
                continue;

            //string[] splitName2 = splitName1[splitName1.Length - 1].Split('#');

            ++file_count;

            assetlist.Add(splitName1[splitName1.Length - 1]);
        }

        foreach (string key in assetlist)
        {
            TempUtil.StartCoroutine(DownAssetFileCo(key));
            break;
        }
    }

    private void CompareToLocalVersion()
    {
        file_count = 0;
        foreach (string assetstr in JsonLoader.instance.Assetlist)
        {
            if (assetstr.CompareTo("crossdomain.xml") == 0 || assetstr.CompareTo("") == 0)
            {
                continue;
            }

            file_count++;

            string realfilename = "";
            if (assetstr.Contains(".anim"))
            {
                realfilename = assetstr.Split('.')[0] + ".anim";
            }
            else if (assetstr.Contains(".sound"))
            {
                realfilename = assetstr.Split('.')[0] + ".sound";
            }
            else
            {
#if UNITY_STANDALONE_WIN
                realfilename = assetstr.Split('.')[0] + ".assetwin";
#elif UNITY_ANDROID
                realfilename = assetstr.Split('.')[0] + ".assetand";
#elif UNITY_IOS
                realfilename = assetstr.Split('.')[0] + ".assetios";
#endif

            }

            string[] savefiles = realfilename.Split('^');
            string savefile = "";
            string realdate = "";
            if (savefiles.Length > 1)
            {
                if (realfilename.Contains(".anim"))
                {
                    savefile = savefiles[0] + ".anim";
                }
                else if (realfilename.Contains(".sound"))
                {
                    savefile = savefiles[0] + ".sound";

                    soundassetlist.Add(savefile);
                }
                else
                {
#if UNITY_STANDALONE_WIN
                    savefile = savefiles[0] + ".assetwin";
#elif UNITY_ANDROID
                    savefile = savefiles[0] + ".assetand";
#elif UNITY_IOS
                    savefile = savefiles[0] + ".assetios";
#endif

                    if (savefile.Contains("_snd."))
                    {
                        //Debug.Log(savefiles[0]);
                        //Rc_SoundMgr.instance.g_SoundABNameList.Add(savefiles[0]);
                    }
                }

                string date = savefiles[1];
                realdate = date.Split('.')[0];
            }
            else
            {
                savefile = realfilename;
            }

            //Debug.Log(savefile);
            string[] files = Directory.GetFiles(nativeAppdatapath + folderName, savefiles[0] + "^*");
            foreach (string file in files)
            {
                if (!file.Contains(realdate))
                {
                    File.Delete(file);
                }
            }
            assetlist.Add(realfilename);
        }

        if (assetlist.Count > 0)
        {
            foreach (string key in assetlist)
            {                
                TempUtil.StartCoroutine(DownAssetFileCo(key));
                break;
            }
        }
    }

    private void StartDownload(List<string> _keylist)
    {
        FileStream fs = new FileStream(nativeAppdatapath + "\\LocalAssetversion.txt", FileMode.Create);
        StreamWriter sw = new StreamWriter(fs);


        foreach (KeyValuePair<string, string> pair in serverAssetDic)
        {

            if (pair.Key != "" && pair.Value != "")
            {
                if (pair.Key != "crossdomain.xml")
                {
                    sw.WriteLine(pair.Key + "\t" + pair.Value);
                }

            }

        }
        sw.Close();
        fs.Close();

    }

    private void DelDiffFile()
    {
        string[] files = System.IO.Directory.GetFiles(nativeAppdatapath + folderName);

        foreach (string fi in files)
        {
            string temstr = fi.Replace("\\", "/");
            string[] strs = temstr.Split(new char[] { '/' });
            foreach (string str in strs)
            {

                if (str.Contains(".assetbundle"))
                {
                    if (localAssetDic.ContainsKey(str))
                    {

                    }
                    else
                    {
                        File.Delete(fi);
                    }
                }
            }

        }

    }


    private static Dictionary<string, string> GetHtmlString(string url)
    {

        string strHtml = url;

        string[] str = strHtml.Split(new char[] { '\n' });
        strHtml = null;

        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        foreach (string s in str)
        {
            if (s.Trim() != "")
                strHtml += s + ":";

            string[] ss = s.Split(new char[] { ' ' });
            string filename = "";
            string resultfailname = "";
            string datestr = "";
            foreach (string strSs in ss)
            {
                filename = strSs;
                filename = filename.Replace("<a", "");
                if (filename.Length > 0)
                {
                    if (filename.StartsWith("href"))
                    {
                        filename = filename.Replace(">", ":").Replace("</a:", "");

                        string[] s1 = filename.Split(new char[] { ':' });
                        foreach (string s1Str in s1)
                        {
                            if (s1Str.StartsWith("<") || s1Str.StartsWith("href") || s1Str.StartsWith("../"))
                            {

                            }
                            else
                            {
                                resultfailname = s1Str;
                            }
                        }

                    }
                    else
                    {
                        datestr += strSs;

                    }
                }
            }
            if (resultfailname.Length > 0)
            {

            }

            if (dictionary.ContainsKey(resultfailname))
            {
                dictionary.Remove(resultfailname);
            }


            dictionary.Add(resultfailname, datestr);

        }

        return dictionary;
    }

    private IEnumerator DownAssetFileCo(string filename)
    {
        string url = "";

        url = Auth.GetAssetUrl(ASSETS.android_asset);
#if UNITY_IPHONE || UNITY_STANDALONE_OSX
		url = Auth.GetAssetUrl(ASSETS.ios_asset);
#elif UNITY_STANDALONE_WIN
        url = Auth.GetAssetUrl(ASSETS.windows_asset);
#endif
        randcount = UnityEngine.Random.Range(0, 10000);

        url += WWW.EscapeURL(filename) + "?p=" + randcount;

        //string[] savefiles = filename.Split('^');
        //string savefile = "";

        //if (savefiles.Length > 1)
        //{
        //    if (filename.Contains(".anim"))
        //    {
        //        savefile = savefiles[0] + ".anim";
        //    }
        //    else if (filename.Contains(".sound"))
        //    {
        //        savefile = savefiles[0] + ".sound";
        //    }
        //    else
        //        savefile = savefiles[0] + ".assetbundle";
        //}
        //else
        //{
        //    savefile = filename;
        //}

#if UNITY_IPHONE
        //< 아이폰에서는 ^ 이걸 빼버림
        int idx = filename.IndexOf("^");
        filename = filename.Remove(idx, 1);
#endif

        if (File.Exists(nativeAppdatapath + folderName + "/" + filename))
        {
            localcount++;
            AssetbundmgrSet();
        }
        else
        {
            www = new WWW(url);
            yield return www;

            if (www.isDone)
            {
                if (www.error != null)
                {
                    Debug.LogError("asset bundle error  : " + www.error + "     " + url);
                }
                else
                {
                    localcount++;

                    SaveAssetFile(www.bytes, nativeAppdatapath + folderName + "/" + filename);
                    AssetbundmgrSet();
                }
            }
            else
            {
                //Debug.Log("asset bundle not load : " + filename);
            }
        }
    }

    private static void SaveAssetFile(byte[] byteData, string fileName)
    {
        FileStream fs = new FileStream(fileName, FileMode.Create);
        fs.Seek(0, SeekOrigin.Begin);
        fs.Write(byteData, 0, byteData.Length);
        fs.Close();
    }

    private void AssetbundmgrSet()
    {   
        if (assetlist.Count > 0)
        {            
            //AssetbundleLoader.AddRealAssetName(assetlist[0], System.Text.RegularExpressions.Regex.Replace(assetlist[0], "[#][0-9]{10,}", ""));

            string firstStr = assetlist[0].Split('^')[0];
            AssetbundleLoader.AddRealAssetName(firstStr.Substring(0, firstStr.LastIndexOf(".")), assetlist[0]);
            
            assetlist.RemoveAt(0);
            if (assetlist.Count > 0)
            {                
                TempUtil.StartCoroutine(DownAssetFileCo(assetlist[0]));
            }
        }

        if ((file_count == localcount) && isLoad == false)
        {
            isLoad = true;
            //foreach (string tem in soundassetlist)
            //{
                //Debug.Log(tem);
            //}

            //for (int a_nn = 0; a_nn < Rc_SoundMgr.instance.g_SoundABNameList.Count; a_nn++)
            //{
            //    if (Rc_SoundMgr.instance.g_SoundABNameList[a_nn] == null)
            //        continue;

            //    Debug.Log(Rc_SoundMgr.instance.g_SoundABNameList[a_nn]);
            //}
         
            if (www != null)
                www.Dispose();
        }
    }

    public float GetProgress()
    {
        if (file_count == 0)
            return 0;

        return (float)((float)localcount / (float)file_count);
    }
}
	
	
