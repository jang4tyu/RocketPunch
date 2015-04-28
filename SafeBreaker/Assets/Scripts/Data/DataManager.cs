using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DataManager : Immortal<DataManager>
{
    NetData netdata = new NetData();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public ulong GetID()
    {
        return netdata.m_MyInfo.user_id;
    }

    public string GetName()
    {
        return netdata.m_MyInfo.name;
    }

    public ulong GetCoin()
    {
        return netdata.m_MyInfo.coin;
    }

    public void SetUserData(ulong user_id)
    {
        netdata.m_MyInfo.user_id = user_id;
    }

    public void SetUserData(UserInfo MyInfo)
    {
        netdata.m_MyInfo = MyInfo;
    }

    public void AddRankUser(UserInfo user, int Ranking)
    {
        netdata.AddRankUser(user, Ranking);
    }

    public Dictionary<int, UserInfo> GetRankList()
    {
        return netdata.GetRankList();
    }

    public void ResetRank()
    {
        netdata.ResetRank();
    }

    public void AddGameLog(GameLogInfo log)
    {
        netdata.AddGameLog(log);
    }

    public List<GameLogInfo> GetLogList()
    {
        return netdata.GetLogList();
    }

    public void ResetGameLog()
    {
        netdata.ResetGameLog();
    }

    public void SetTarget(UserInfo target)
    {
        netdata.m_TargetInfo = target;
    }

    public ulong GetGameID()
    {
        return netdata.m_game_id;
    }

    public void SetCoin(ulong coin)
    {
        netdata.m_addcoin = coin - netdata.m_MyInfo.coin;
        netdata.m_MyInfo.coin = coin;
        UIManager.instance.ResetInfo();
    }

    public ulong GetAddCoin()
    {
        return netdata.m_addcoin;
    }

    public void SetPassword(string password)
    {
        netdata.m_MyInfo.password = password;
    }

    public string GetPassword()
    {
        return netdata.m_MyInfo.password;
    }

    public string GetTargetPW()
    {
        return netdata.m_TargetInfo.password;
    }

    public byte GetLockLv()
    {
        return netdata.m_MyInfo.locklv;
    }

    public byte GetSafeLv()
    {
        return netdata.m_MyInfo.safelv;
    }

    public void SetGameID(ulong game_id)
    {
        netdata.m_game_id = game_id;
    }

    public void SetSafeLv(byte safelv)
    {
        netdata.m_MyInfo.safelv = safelv;
    }

    public void SetLockLv(byte locklv)
    {
        netdata.m_MyInfo.locklv = locklv;
    }

    public UserInfo GetTarget()
    {
        return netdata.m_TargetInfo;
    }

    public void SetRevengeGameID(ulong revenge_game_id)
    {
        netdata.m_revenge_game_id = revenge_game_id;
    }

    public ulong GetRevengeGameID()
    {
        return netdata.m_revenge_game_id;
    }

    public bool SetNotifyLog(NotifyLog log)
    {
        bool bFlag = false;
        if (netdata.m_NotifyLog.game_id != 0 && netdata.m_NotifyLog.game_id != log.game_id)
            bFlag = true;

        netdata.m_NotifyLog = log;
        return bFlag;
    }

    public NotifyLog GetNotifyLog()
    {
        return netdata.m_NotifyLog;
    }

    public void SetName(string name)
    {
        netdata.m_MyInfo.name = name;
    }

    public string GetRankName(ulong user_id)
    {
        return netdata.GetRankName(user_id);
    }

    public void AddRankUser(JSONObject info, int ranking)
    {
        UserInfo user = new UserInfo();
        user.user_id = (ulong)info["user_id"].n;
        user.name = info["name"].str;
        user.coin = (ulong)info["coin"].n;
        user.safelv = (byte)info["safelv"].n;
        user.locklv = (byte)info["locklv"].n;

        AddRankUser(user, ranking);
    }

    public void AddGameLog(JSONObject info)
    {
        GameLogInfo log = new GameLogInfo();
        log.game_id = (ulong)info["game_id"].n;
        log.result = (byte)info["result"].n;
        log.attacker = (ulong)info["attacker"].n;
        log.target = (ulong)info["target"].n;
        log.money = (ulong)info["money"].n;
        log.revenge = (byte)info["revenge"].n;

        string time = info["time"].str;
        time = time.Replace('T', ' ');
        time = time.Remove(time.Length - 5, 5);
        log.time = time;

        AddGameLog(log);
    }
}