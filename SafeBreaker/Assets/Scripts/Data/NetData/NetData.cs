using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NetData 
{
    // public
    public UserInfo m_MyInfo = new UserInfo();
    public UserInfo m_TargetInfo = new UserInfo();

    public NotifyLog m_NotifyLog = new NotifyLog();

    public ulong m_game_id = 0;
    public ulong m_revenge_game_id = 0;
    public ulong m_addcoin = 0;

    // private
    Dictionary<int, UserInfo> m_RankUserList= new Dictionary<int, UserInfo>();
    List<GameLogInfo> m_GameLogList = new List<GameLogInfo>();
       

    public void ResetRank()
    {
        m_RankUserList.Clear();
    }

    public void AddRankUser(UserInfo userInfo, int Ranking) 
    {
        m_RankUserList.Add(Ranking, userInfo);
    }

    public Dictionary<int, UserInfo> GetRankList()
    {
        return m_RankUserList;
    }

    public void ResetGameLog()
    {
        m_GameLogList.Clear();
    }

    public void AddGameLog(GameLogInfo logInfo)
    {
        m_GameLogList.Add(logInfo);
    }

    public List<GameLogInfo> GetLogList()
    {
        return m_GameLogList;
    }

    public string GetPassword()
    {
        return m_MyInfo.password;
    }

    public string GetRankName(ulong user_id)
    {
        foreach(var info in m_RankUserList)
        {
            if (info.Value.user_id == user_id)
                return info.Value.name;
        }

        return string.Empty;
    }
}
public class UserInfo
{
    public ulong    user_id;
    public string   name;
    public string   password;
    public ulong    coin;
    public byte     safelv;
    public byte     locklv;
}

public class GameLogInfo
{
    public ulong    game_id;
    public byte     result;
    public ulong    attacker;
    public ulong    target;
    public ulong    money;
    public byte     revenge;
    public string   time;
}

public class NotifyLog
{
    public ulong    game_id;
    public ulong    attacker_id;
    public ulong    coin;
}