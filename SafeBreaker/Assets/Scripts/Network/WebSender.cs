using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WebSender : Singleton<WebSender>
{
    public class ProtocolChecker
    {
        //< 현재 전송가능여부
        public bool canSend;

        //< 중복해서 전송가능여부(즉 결과를 안받고 보내기만 해도되는 데이터)
        public bool canDuplicate;

        //< 재전송 여부
        public bool ReSend;

        public ProtocolChecker()
        {
            //< 초기 셋팅
            foreach (PROTOCOL eEnum in PROTOCOL.GetValues(typeof(PROTOCOL)))
                SendCheker.Add(eEnum, new ProtocolChecker(false));

            //< 만약 중복으로 보낼값이 있다면 따로 처리
            SendCheker[PROTOCOL.P_USER_LOGIN].canDuplicate = true;
//            SendCheker[Protocol.PROTOCOL.P_USER_MISSION_UPDATE].canDuplicate = true;
//            SendCheker[Protocol.PROTOCOL.P_CHAT_GET_CHANNEL_IP].canDuplicate = true;

//#if UNITY_EDITOR
//            SendCheker[Protocol.PROTOCOL.P_ITEM_UNIT_SELL].canDuplicate = true;
//#endif
        }

        public ProtocolChecker(bool _canDuplicate, bool resend = true)
        {
            ReSend = resend;
            canSend = true;
            canDuplicate = _canDuplicate;
        }

        public Dictionary<PROTOCOL, ProtocolChecker> SendCheker = new Dictionary<PROTOCOL, ProtocolChecker>();
    };

    public ProtocolChecker Checker = new ProtocolChecker();

    NetworkBase netBase = NetworkBase.instance;
    System.Text.StringBuilder info = new System.Text.StringBuilder();

    public WebReceiver webReceiver = new WebReceiver();

    WebSender()
    {
        netBase.SetNetReceiver(webReceiver.Receiver);
    }

    void SendData(int cmd, string info, NetCallBack callback)
    {
        if (Checker.SendCheker.ContainsKey((PROTOCOL)cmd))
        {
            ProtocolChecker checker = Checker.SendCheker[(PROTOCOL)cmd];

            callback += (json) => { checker.canSend = true; };

            //< 중복해서 전송이 가능하면 또 전송
            if (checker.canDuplicate)
                netBase.SendData(cmd, info, callback);

            else
            {
                //< 중복이 안된다면 현재 전송이 가능할때만 전송
                if (checker.canSend)
                    netBase.SendData(cmd, info, callback);
            }

            //< 전송 불가능으로 대입
            checker.canSend = false;
        }
    }

    // 서버String Encoding
    string Encoding(string str)
    {
        byte[] bytesToEncode = System.Text.Encoding.UTF8.GetBytes(str);
        return System.Convert.ToBase64String(bytesToEncode);
    }

    public void P_SERVICE_INFO(NetCallBack callback = null)
    {
        info.Remove(0, info.Length);
        SendData((int)PROTOCOL.P_SERVICE_INFO, info.ToString(), callback);
    }

    public void P_USER_LOGIN(string device_id, NetCallBack callback = null)
    {
        info.Remove(0, info.Length);
        info.AppendFormat("\"{0}\"", Encoding(device_id));
        SendData((int)PROTOCOL.P_USER_LOGIN, info.ToString(), callback);
    }

    public void P_USER_INFO(NetCallBack callback = null)
    {
        info.Remove(0, info.Length);
        info.AppendFormat("{0}", DataManager.instance.GetID());
        SendData((int)PROTOCOL.P_USER_INFO, info.ToString(), callback);
    }

    public void P_USER_LIST(NetCallBack callback = null)
    {
        info.Remove(0, info.Length);
        info.AppendFormat("{0}", DataManager.instance.GetID());
        SendData((int)PROTOCOL.P_USER_LIST, info.ToString(), callback);
    }

    public void P_USER_SETPW(string password, NetCallBack callback = null)
    {
        info.Remove(0, info.Length);
        info.AppendFormat("{0},\"{1}\"", DataManager.instance.GetID(), password);
        SendData((int)PROTOCOL.P_USER_SETPW, info.ToString(), callback);
    }

    public void P_USER_UPGRADE(byte type, NetCallBack callback = null)
    {
        info.Remove(0, info.Length);
        info.AppendFormat("{0},{1}", DataManager.instance.GetID(), type);
        SendData((int)PROTOCOL.P_USER_UPGRADE, info.ToString(), callback);
    }

    public void P_USER_NOTIFY(NetCallBack callback = null)
    {
        info.Remove(0, info.Length);
        info.AppendFormat("{0}", DataManager.instance.GetID());
        SendData((int)PROTOCOL.P_USER_NOTIFY, info.ToString(), callback);
    }

    public void P_USER_NICKNAME(string name, NetCallBack callback = null)
    {
        info.Remove(0, info.Length);
        info.AppendFormat("{0},\"{1}\"", DataManager.instance.GetID(), Encoding(name));
        SendData((int)PROTOCOL.P_USER_NICKNAME, info.ToString(), callback);
    }

    public void P_GAME_START(ulong target_id, NetCallBack callback = null)
    {
        info.Remove(0, info.Length);
        info.AppendFormat("{0},{1}", DataManager.instance.GetID(), target_id);
        SendData((int)PROTOCOL.P_GAME_START, info.ToString(), callback);
    }

    public void P_GAME_RESULT(ulong game_id, byte try_count, NetCallBack callback = null)
    {
        info.Remove(0, info.Length);
        info.AppendFormat("{0},{1},{2}", DataManager.instance.GetID(), game_id, try_count);
        SendData((int)PROTOCOL.P_GAME_RESULT, info.ToString(), callback);
    }

    public void P_GAME_LOG(NetCallBack callback = null)
    {
        info.Remove(0, info.Length);
        info.AppendFormat("{0}", DataManager.instance.GetID());
        SendData((int)PROTOCOL.P_GAME_LOG, info.ToString(), callback);
    }

    public void P_GAME_REVENGE(ulong target_id, ulong game_id, NetCallBack callback = null)
    {
        info.Remove(0, info.Length);
        info.AppendFormat("{0},{1},{2}", DataManager.instance.GetID(), target_id, game_id);
        SendData((int)PROTOCOL.P_GAME_REVENGE, info.ToString(), callback);
    }
}