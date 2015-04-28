using UnityEngine;
using System.Collections;

/// <summary>
/// 서버에서 답변이 오면 먼저 캡쳐해 NetData 클래스에 정보를 수납 하는 클래스
/// </summary>

//< 특정 프로토콜이 도착했을경우 호출시켜주기 위한 델리게이트
public delegate void CallEventFunction(JSONObject jsonObj);
public class WebReceiver
{
    /// <summary>
    /// 스크립트로 가장먼저 서버의 ack 를 받는 코드
    /// </summary>
    /// <param name="Ans"></param>
    public void Receiver(JSONObject jsonObject, NetCallBack callback)
    {
        PROTOCOL protocol = (PROTOCOL)jsonObject["cmd"].n;

        Debug.Log(protocol + ": " + jsonObject.ToString());

        int errno = (int)jsonObject["errno"].n;
        if (errno == 0)
        {
            switch (protocol)
            {
                case PROTOCOL.P_SERVICE_INFO:
                    P_SERVICE_INFO(jsonObject);
                    break;
                case PROTOCOL.P_USER_LOGIN:
                    P_USER_LOGIN(jsonObject);
                    break;
                case PROTOCOL.P_USER_INFO:
                    P_USER_INFO(jsonObject);
                    break;
                case PROTOCOL.P_USER_LIST:
                    P_USER_LIST(jsonObject);
                    break;
                case PROTOCOL.P_USER_SETPW:
                    P_USER_SETPW(jsonObject);
                    break;
                case PROTOCOL.P_USER_UPGRADE:
                    P_USER_UPGRADE(jsonObject);
                    break;
                case PROTOCOL.P_USER_NOTIFY:
                    P_USER_NOTIFY(jsonObject);
                    break;
                case PROTOCOL.P_USER_NICKNAME:
                    P_USER_NICKNAME(jsonObject);
                    break;
                case PROTOCOL.P_GAME_START:
                    P_GAME_START(jsonObject);
                    break;
                case PROTOCOL.P_GAME_RESULT:
                    P_GAME_RESULT(jsonObject);
                    break;
                case PROTOCOL.P_GAME_LOG:
                    P_GAME_LOG(jsonObject);
                    break;
                case PROTOCOL.P_GAME_REVENGE:
                    P_GAME_REVENGE(jsonObject);
                    break;
                default:
                    Debug.LogError("not found protocol");
                    break;
            }
        }
        else
        {
            // 팝업으로 에러 띄움
            //ShowErrorPopup(errno);
            UIManager.instance.CreateOKPopup(errno);
            Debug.Log("ErrNo : " + protocol.ToString() + "(" + errno + ")");
        }

        if (callback != null)
            callback(jsonObject);
    }

    // Recv Packet!!
    void P_SERVICE_INFO(JSONObject jsonObj)
    {
        string version = jsonObj["version"].str;
        string server_time = jsonObj["current_time"].str;

        if (version != "1.0.0")
        {
            // TODO : Version Error Process!!
            // Update URL or Application Quit

            Debug.Log("Version Error!");
            return;
        }

        Debug.Log("Version Check Complete! : version - " + version + " server time - " + server_time);
    }

    void P_USER_LOGIN(JSONObject jsonObj)
    {
        JSONObject obj = jsonObj["user_id"];
        if (obj == null)
            return;

        ulong user_id = (ulong)jsonObj["user_id"].n;

        if(user_id == 0 )
        {
            Debug.Log("Login Error!!");
            return;
        }

        DataManager.instance.SetUserData(user_id);
        Debug.Log("Login Complete! : user_id - " + user_id);
    }

    void P_USER_INFO(JSONObject jsonObj)
    {
        JSONObject obj = jsonObj["info"];
        if (obj == null)
            return;

        ulong coin = (ulong)obj["coin"].n;
        string name = obj["name"].str;
        string password = obj["password"].str;
        byte safelv = (byte)obj["safelv"].n;
        byte locklv = (byte)obj["locklv"].n;

        UserInfo MyInfo = new UserInfo();
        MyInfo.user_id = DataManager.instance.GetID();
        MyInfo.name = name;
        MyInfo.password = password;
        MyInfo.coin = coin;
        MyInfo.safelv = safelv;
        MyInfo.locklv = locklv;

        DataManager.instance.SetUserData(MyInfo);
    }

    void P_USER_LIST(JSONObject jsonObj)
    {
        JSONObject obj = jsonObj["info"];
        if (obj == null)
            return;

        for (int i = 0; i < obj.list.Count; ++i)
        {
            JSONObject info = obj[i];
            if (info == null || info.keys.Count == 0)
                continue;

            DataManager.instance.AddRankUser(info, i + 1);
        }

        if( obj.GetField("user_id") != null)
            DataManager.instance.AddRankUser(obj, 1);
    }

    void P_USER_SETPW(JSONObject jsonObj)
    {
        JSONObject obj = jsonObj["info"];
        if (obj == null)
            return;

        string password = obj["password"].str;
        ulong coin = (ulong)obj["coin"].n;

        DataManager.instance.SetPassword(password);
        DataManager.instance.SetCoin(coin);
    }

    void P_USER_UPGRADE(JSONObject jsonObj)
    {
        JSONObject obj = jsonObj["info"];
        if (obj == null)
            return;

        ulong coin = (ulong)obj["coin"].n;
        DataManager.instance.SetCoin(coin);

        if (obj["safelv"] != null)
        {
            byte safelv = (byte)obj["safelv"].n;
            DataManager.instance.SetSafeLv(safelv);
        }

        if (obj["locklv"] != null)
        {
            byte locklv = (byte)obj["locklv"].n;
            DataManager.instance.SetLockLv(locklv);
        }
    }

    void P_USER_NOTIFY(JSONObject jsonObj)
    {
        JSONObject obj = jsonObj["info"];
        if (obj == null || obj["game_id"] == null)
            return;

        ulong game_id = (ulong)obj["game_id"].n;
        ulong attacker = (ulong)obj["attacker"].n;
        ulong coin = (ulong)obj["coin"].n;

        NotifyLog log = new NotifyLog();
        log.game_id = game_id;
        log.attacker_id = attacker;
        log.coin = coin;

        if (DataManager.instance.SetNotifyLog(log))
            UIManager.instance.SetNotify();
    }

    void P_USER_NICKNAME(JSONObject jsonObj)
    {
        JSONObject obj = jsonObj["info"];
        if (obj == null)
            return;

        string name = obj["name"].str;
        DataManager.instance.SetName(name);
    }

    void P_GAME_START(JSONObject jsonObj)
    {
        JSONObject obj = jsonObj["info"];
        if (obj == null)
            return;

        ulong game_id = (ulong)obj["game_id"].n;
        ulong target_id = (ulong)obj["target_id"].n;
        string target_pw = obj["password"].str;
        byte safe_lv = (byte)obj["safelv"].n;
        byte lock_lv = (byte)obj["locklv"].n;
        ulong coin = (ulong)obj["coin"].n;

        UserInfo Target = new UserInfo();
        Target.user_id = target_id;
        Target.name = "로켓펀치" + target_id;
        Target.coin = coin;
        Target.password = target_pw;
        Target.safelv = safe_lv;
        Target.locklv = lock_lv;

        DataManager.instance.SetGameID(game_id);
        DataManager.instance.SetTarget(Target);
        DataManager.instance.SetCoin(coin);
    }

    void P_GAME_RESULT(JSONObject jsonObj)
    {
        JSONObject obj = jsonObj["info"];
        if (obj == null)
            return;

        ulong coin = (ulong)obj["coin"].n;
        DataManager.instance.SetCoin(coin);
    }

    void P_GAME_LOG(JSONObject jsonObj)
    {
        JSONObject obj = jsonObj["info"];
        if (obj == null)
            return;

        for (int i = 0; i < obj.list.Count; ++i)
        {
            JSONObject info = obj[i];
            if (info == null || info.keys.Count == 0)
                continue;

            DataManager.instance.AddGameLog(info);
        }

        if (obj.GetField("result") != null)
            DataManager.instance.AddGameLog(obj);
    } 

    void P_GAME_REVENGE(JSONObject jsonObj)
    {
        JSONObject obj = jsonObj["info"];
        if (obj == null)
            return;

        ulong game_id = (ulong)obj["game_id"].n;
        ulong target_id = (ulong)obj["target_id"].n;
        string target_pw = obj["password"].str;
        byte safe_lv = (byte)obj["safelv"].n;
        byte lock_lv = (byte)obj["locklv"].n;

        UserInfo Target = new UserInfo();
        Target.user_id = target_id;
        Target.name = "로켓펀치" + target_id;
        Target.password = target_pw;
        Target.safelv = safe_lv;
        Target.locklv = lock_lv;

        DataManager.instance.SetGameID(game_id);
        DataManager.instance.SetTarget(Target);
    }
}