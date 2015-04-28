using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Master
{
	public class DataInfo
	{
		public ushort defaultcoin;
		ushort _dailycoin;
		public ushort dailycoin {
			set { _dailycoin = EncryptHelper.SSecureUSHORT(value); }
			get { return EncryptHelper.GSecureUSHORT(_dailycoin); }
		}
		byte _pancreasrate;
		public byte pancreasrate {
			set { _pancreasrate = EncryptHelper.SSecureBYTE(value); }
			get { return EncryptHelper.GSecureBYTE(_pancreasrate); }
		}
		ushort _shieldtime;
		public ushort shieldtime {
			set { _shieldtime = EncryptHelper.SSecureUSHORT(value); }
			get { return EncryptHelper.GSecureUSHORT(_shieldtime); }
		}
		byte _attackcoin;
		public byte attackcoin {
			set { _attackcoin = EncryptHelper.SSecureBYTE(value); }
			get { return EncryptHelper.GSecureBYTE(_attackcoin); }
		}
		byte _rewardrate;
		public byte rewardrate {
			set { _rewardrate = EncryptHelper.SSecureBYTE(value); }
			get { return EncryptHelper.GSecureBYTE(_rewardrate); }
		}
		ushort _passwordcoin;
		public ushort passwordcoin {
			set { _passwordcoin = EncryptHelper.SSecureUSHORT(value); }
			get { return EncryptHelper.GSecureUSHORT(_passwordcoin); }
		}
		ushort _attackcoinlimit;
		public ushort attackcoinlimit {
			set { _attackcoinlimit = EncryptHelper.SSecureUSHORT(value); }
			get { return EncryptHelper.GSecureUSHORT(_attackcoinlimit); }
		}
		ushort _bonuscoin;
		public ushort bonuscoin {
			set { _bonuscoin = EncryptHelper.SSecureUSHORT(value); }
			get { return EncryptHelper.GSecureUSHORT(_bonuscoin); }
		}
	}
	public List<DataInfo>  DataInfoList = new List<DataInfo> ();

	public void LoadLowData()
	{
		JSONObject Data = JsonLoader.instance.LoadJsonFile("Master_Data.json");
		for (int i = 0; i < Data.list.Count; i++)
		{
			DataInfo tmpInfo = new DataInfo();
			tmpInfo.defaultcoin = ushort.Parse(Data[i]["defaultcoin_us"].str);
			tmpInfo.dailycoin = ushort.Parse(Data[i]["dailycoin_us"].str);
			tmpInfo.pancreasrate = byte.Parse(Data[i]["pancreasrate_b"].str);
			tmpInfo.shieldtime = ushort.Parse(Data[i]["shieldtime_us"].str);
			tmpInfo.attackcoin = byte.Parse(Data[i]["attackcoin_b"].str);
			tmpInfo.rewardrate = byte.Parse(Data[i]["rewardrate_b"].str);
			tmpInfo.passwordcoin = ushort.Parse(Data[i]["passwordcoin_us"].str);
			tmpInfo.attackcoinlimit = ushort.Parse(Data[i]["attackcoinlimit_us"].str);
			tmpInfo.bonuscoin = ushort.Parse(Data[i]["bonuscoin_us"].str);

			DataInfoList.Add(tmpInfo);
		}

	}
}
