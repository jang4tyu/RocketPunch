using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LockUpgrade
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
            get { return EncryptHelper.SSecureUINT(_cost); }
		}
		byte _maxcount;
		public byte maxcount {
			set { _maxcount = EncryptHelper.SSecureBYTE(value); }
			get { return EncryptHelper.GSecureBYTE(_maxcount); }
		}
		byte _limittime;
		public byte limittime {
			set { _limittime = EncryptHelper.SSecureBYTE(value); }
			get { return EncryptHelper.GSecureBYTE(_limittime); }
		}
		byte _limitvalue;
		public byte limitvalue {
			set { _limitvalue = EncryptHelper.SSecureBYTE(value); }
			get { return EncryptHelper.GSecureBYTE(_limitvalue); }
		}
		public string desc;
	}
	public Dictionary<byte, DataInfo>  DataInfoDic = new Dictionary<byte, DataInfo> ();

	public void LoadLowData()
	{
		JSONObject Data = JsonLoader.instance.LoadJsonFile("LockUpgrade_Data.json");
		for (int i = 0; i < Data.list.Count; i++)
		{
			DataInfo tmpInfo = new DataInfo();
			tmpInfo.id = byte.Parse(Data[i]["id_b"].str);
			tmpInfo.nextid = byte.Parse(Data[i]["nextid_b"].str);
			tmpInfo.cost = uint.Parse(Data[i]["cost_ui"].str);
			tmpInfo.maxcount = byte.Parse(Data[i]["maxcount_b"].str);
			tmpInfo.limittime = byte.Parse(Data[i]["limittime_b"].str);
			tmpInfo.limitvalue = byte.Parse(Data[i]["limitvalue_b"].str);
			tmpInfo.desc = Data[i]["desc_c"].str;

			DataInfoDic.Add(tmpInfo.id, tmpInfo);
		}

	}
}
