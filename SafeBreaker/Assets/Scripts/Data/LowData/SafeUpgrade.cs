using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SafeUpgrade
{
	public class DataInfo
	{
		public byte id;
		byte _nextid;
		public byte nextid {
			set { _nextid = EncryptHelper.SSecureBYTE(value); }
			get { return EncryptHelper.GSecureBYTE(_nextid); }
		}
		uint _cost;
		public uint cost {
			set { _cost = EncryptHelper.SSecureUINT(value); }
			get { return EncryptHelper.GSecureUINT(_cost); }
		}
		uint _maxcoin;
		public uint maxcoin {
			set { _maxcoin = EncryptHelper.SSecureUINT(value); }
			get { return EncryptHelper.GSecureUINT(_maxcoin); }
		}
		public string desc;
	}
	public Dictionary<byte, DataInfo>  DataInfoDic = new Dictionary<byte, DataInfo> ();

	public void LoadLowData()
	{
		JSONObject Data = JsonLoader.instance.LoadJsonFile("SafeUpgrade_Data.json");
		for (int i = 0; i < Data.list.Count; i++)
		{
			DataInfo tmpInfo = new DataInfo();
			tmpInfo.id = byte.Parse(Data[i]["id_b"].str);
			tmpInfo.nextid = byte.Parse(Data[i]["nextid_b"].str);
			tmpInfo.cost = uint.Parse(Data[i]["cost_ui"].str);
			tmpInfo.maxcoin = uint.Parse(Data[i]["maxcoin_ui"].str);
			tmpInfo.desc = Data[i]["desc_c"].str;

			DataInfoDic.Add(tmpInfo.id, tmpInfo);
		}

	}
}
