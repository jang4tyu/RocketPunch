using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// Network Callback
public delegate void NetReceiver(JSONObject jsonObject, NetCallBack callback);

// int : 0 - out, 1 - ball, 2 - strike
public delegate void BaseballCallback(List<eBaseballResult> Result);