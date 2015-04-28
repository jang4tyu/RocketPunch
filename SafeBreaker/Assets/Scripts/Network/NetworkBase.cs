//#define NETWORK_ENCRYPT

using UnityEngine;
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Cryptography;

using System.IO;
//using ComponentAce.Compression.Libs.zlib;

public class NetworkBase : Immortal<NetworkBase>
{
    public string address = Auth.ActionUrl + "/protocolx.php";

    
    private NetReceiver netReceiver = null;

    public void SetNetReceiver(NetReceiver _netReceiver)
    {
        netReceiver = _netReceiver;
    }
    System.Text.StringBuilder sb = new System.Text.StringBuilder();

    public void SendData(int cmd, string info)
    {
        SendData(cmd, info, null);
    }

    public void SendData(int cmd, string info, NetCallBack callback)
    {
        if (!NetworkCheck())
            return;

        sb.Remove(0, sb.Length);
        sb.AppendFormat("{{\"cmd\":{0}, \"info\":[{1}]}}", cmd, info);
        StartCoroutine(SendTo(sb.ToString(), callback));
    }

    private IEnumerator SendTo(string request, NetCallBack callback)
    {
        WWWForm form = new WWWForm();
#if NETWORK_ENCRYPT
        Debug.Log("NetworkBase SendTo() : " + m_szActionUrl +"?enc="+ jsonData);
        jsonData =  NetworkBase.Scecurity.Encrypt(jsonData) ;
#endif
        form.AddField("enc", request);
        
        Debug.Log(address+"?enc=" + request);
        
        WWW www = new WWW(address, form);
        www.threadPriority = ThreadPriority.High;

        yield return www;

        if (www.error != null)
        {
            Debug.Log("www error : " + www.error);
        }
        else
        {
            if (www.isDone)
            {                
                DoRecieve(request, Encoding.UTF8.GetString(www.bytes), callback);                
            }
            else
                yield return www;
        }
    }

    private void DoRecieve(string sendString, string recieveData, NetCallBack callback)
    {
        lock (this)
        {
#if NETWORK_ENCRYPT
                recieveData =  NetworkBase.Scecurity.Decrypt(recieveData) ;			
#endif            
            JSONObject jsonObject = new JSONObject(recieveData);
            //Debug.Log(jsonObject);
            try
            {
                if (jsonObject != null)
                {
                    if (jsonObject["cmd"] != null || jsonObject["errno"].n != 0)
                    {
                        if (netReceiver != null)
                            netReceiver(jsonObject, callback);
                    }
                    else
                    {
                        //Debug.LogError("cmd == null");
                        // TODO error 비정상데이타처리
                        throw new Exception("cannot parsing");
                    }
                }
                else
                {
                    // TODO error 비정상데이타처리
                    throw new Exception("cannot parsing");
                }
            }
            catch (Exception e)
            {
                Debug.LogError(e.ToString());
            }
        }
    }

    public bool NetworkCheck()
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            UIManager.instance.CreateOKPopup((int)eError.ERR_NETWORK);
            return false;
        }

        return true;
    }

    //public class Scecurity
    //{
    //    public static string DNEncrypt(string Data)
    //    {
    //        byte[] key = Encoding.ASCII.GetBytes(m_key);
    //        byte[] iv = Encoding.ASCII.GetBytes(m_szIv);
    //        byte[] data = Encoding.UTF8.GetBytes(Data);
    //        byte[] enc = new byte[0];
    //        TripleDES tdes = TripleDES.Create();
    //        tdes.IV = iv;
    //        tdes.Key = key;
    //        tdes.Mode = CipherMode.CBC;
    //        tdes.Padding = PaddingMode.Zeros;
    //        ICryptoTransform ict = tdes.CreateEncryptor();
    //        enc = ict.TransformFinalBlock(data, 0, data.Length);
    //        return ByteArrayToString(enc);
    //    }

    //    public static string ByteArrayToString(byte[] ba)
    //    {
    //        string hex = BitConverter.ToString(ba);
    //        return hex.Replace("-", "");
    //    }

    //    public static byte[] StringToByteArray(String hex)
    //    {
    //        int NumberChars = hex.Length;
    //        byte[] bytes = new byte[NumberChars / 2];
    //        for (int i = 0; i < NumberChars; i += 2)
    //            bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
    //        return bytes;
    //    }

    //    public static string DNDecrypt(string Data)
    //    {
    //        byte[] key = Encoding.ASCII.GetBytes(m_key);
    //        byte[] iv = Encoding.ASCII.GetBytes(m_szIv);
    //        byte[] data = StringToByteArray(Data);
    //        byte[] enc = new byte[0];
    //        TripleDES tdes = TripleDES.Create();
    //        tdes.IV = iv;
    //        tdes.Key = key;
    //        tdes.Mode = CipherMode.CBC;
    //        tdes.Padding = PaddingMode.Zeros;
    //        ICryptoTransform ict = tdes.CreateDecryptor();
    //        enc = ict.TransformFinalBlock(data, 0, data.Length);
    //        return Encoding.UTF8.GetString(enc);
    //    }
    //    public static string Decrypt(string input)
    //    {
    //        TripleDES tripleDes = TripleDES.Create();
    //        tripleDes.IV = Encoding.ASCII.GetBytes(m_szIv);
    //        tripleDes.Key = Encoding.ASCII.GetBytes(m_key);
    //        tripleDes.Mode = CipherMode.CBC;
    //        tripleDes.Padding = PaddingMode.Zeros;

    //        ICryptoTransform crypto = tripleDes.CreateDecryptor();
    //        byte[] decodedInput = Decoder(input);
    //        byte[] decryptedBytes = crypto.TransformFinalBlock(decodedInput, 0, decodedInput.Length);

    //        return Encoding.ASCII.GetString(decryptedBytes);
    //    }

    //    public static byte[] Decoder(string input)
    //    {
    //        byte[] bytes = new byte[input.Length / 2];
    //        int targetPosition = 0;

    //        for (int sourcePosition = 0; sourcePosition < input.Length; sourcePosition += 2)
    //        {
    //            string hexCode = input.Substring(sourcePosition, 2);
    //            bytes[targetPosition++] = Byte.Parse(hexCode, NumberStyles.AllowHexSpecifier);
    //        }

    //        return bytes;
    //    }

    //    public static string Encoder(byte[] input)
    //    {
    //        string ret = "";
    //        ret = BitConverter.ToString(input).Replace("-", string.Empty).ToLower();
    //        return ret;
    //    }

    //    public static string Encrypt(string input)
    //    {
    //        TripleDES tripleDes = TripleDES.Create();
    //        tripleDes.IV = Encoding.ASCII.GetBytes(m_szIv);
    //        tripleDes.Key = Encoding.ASCII.GetBytes(m_key);
    //        tripleDes.Mode = CipherMode.CBC;
    //        tripleDes.Padding = PaddingMode.Zeros;

    //        ICryptoTransform crypto = tripleDes.CreateEncryptor();
    //        byte[] decodedInput = Encoding.UTF8.GetBytes(input);
    //        byte[] decryptedBytes = crypto.TransformFinalBlock(decodedInput, 0, decodedInput.Length);
    //        return Encoder(decryptedBytes);
    //    }
    //}

    /*public void OnGUI()
    {
        return;
        float posx = 0, posy = 0, xoffset = 5, yoffset = 5, sizex = 100, sizey = 30;

        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "Login"))
        {
            WebSender.instance.Login("maldo3", "1", NetRecieve);
        }

        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "UserAssetInfo"))
        {
            WebSender.instance.UserAssetInfo(MyUUID, NetRecieve);
        }

        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "UserLevelInfo"))
        {
            WebSender.instance.UserLevelInfo(MyUUID, NetRecieve);
        }

        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "SetDesc"))
        {
            WebSender.instance.SetDesc(MyUUID, "한글 되나 보자", NetRecieve);
        }

        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "SetNickName"))
        {
            WebSender.instance.SetNickName(MyUUID, "kbshero", NetRecieve);
        }

        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "GetVisitCount"))
        {
            WebSender.instance.GetVisitCount(MyUUID, NetRecieve);
        }

        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "GetLandInfo"))
        {
            WebSender.instance.GetLandInfo(MyUUID, 1, NetRecieve);
        }

        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "ExpandLand"))
        {
            WebSender.instance.ExpandLand(MyUUID, 1, 36, NetRecieve);
        }

        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "BuildList"))
        {
            WebSender.instance.BuildList(MyUUID, 1, NetRecieve);
        }

        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "ReqProductList"))
        {
            WebSender.instance.ReqProductList(MyUUID, 1, NetRecieve);
        }

        posy += yoffset + sizey;
        posx = 0;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "AvatarInfo"))
        {
            WebSender.instance.AvatarInfo(MyUUID, NetRecieve);
        }

        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "FriendCount"))
        {
            WebSender.instance.FriendCount(MyUUID, NetRecieve);
        }

        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "FriendList"))
        {
            WebSender.instance.FriendList(MyUUID, 1, 0, 10, NetRecieve);
        }

        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "FriendRequestList"))
        {
            WebSender.instance.FriendRequestList(MyUUID, NetRecieve);
        }

        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "AlbaList"))
        {
            WebSender.instance.AlbaList(MyUUID, NetRecieve);
        }
        
        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "AlbaResultResponseList"))
        {
            WebSender.instance.AlbaResultResponseList(MyUUID, NetRecieve);
        }

        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "RequestItemList"))
        {
            WebSender.instance.RequestItemList(MyUUID, NetRecieve);        
        }

        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "NpcList"))
        {
            WebSender.instance.NpcList(MyUUID, NetRecieve);
        }

        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "NpcWorkList"))
        {
            WebSender.instance.NpcWorkList(MyUUID, NetRecieve);
        }

        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "CollectionList"))
        {
            WebSender.instance.CollectionList(MyUUID, NetRecieve);
        }

        posy += yoffset + sizey;
        posx = 0;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "QuestList"))
        {
            WebSender.instance.QuestList(MyUUID, NetRecieve);
        }

        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "GetVoteInfo"))
        {
            WebSender.instance.GetVoteInfo(MyUUID, NetRecieve);
        }

        posx += xoffset + sizex;
        if (GUI.Button(new Rect(posx, posy, sizex, sizey), "GetMiniGameInfo"))
        {
            WebSender.instance.GetMiniGameInfo(MyUUID, NetRecieve);
        }
    }*/


    public void NetRecieve(JSONObject json)
    {
        //Debug.Log("NetRecieve " + json.ToString());        
    }
}