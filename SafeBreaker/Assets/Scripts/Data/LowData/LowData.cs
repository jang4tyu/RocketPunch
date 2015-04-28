#define _DEBUG_

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LowData
{
    public static bool bLoadDone = false;

    //새로운 데이터들
    public static LockUpgrade   lockUpgrade;
    public static SafeUpgrade   safeUpgrade;
    public static Master        master;  

    public static void LoadLowDataALL()
    {
        try
        {
            //new
            lockUpgrade = new LockUpgrade();
            safeUpgrade = new SafeUpgrade();
            master = new Master();

            //load
            lockUpgrade.LoadLowData();
            safeUpgrade.LoadLowData();
            master.LoadLowData();

            bLoadDone = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.ToString() + " : LowDataMgr Load Exception " + e.Message);
        }
    }
}

/// <summary>
/// Simple Encrpyt Class
/// </summary>
public static class EncryptHelper
{
    // SKC (Secure KeyCode)
    static byte SKC_BYTE;
    static short SKC_SHORT;
    static ushort SKC_USHORT;
    static int SKC_INT;
    static uint SKC_UINT;
    static long SKC_LONG;
    static ulong SKC_ULONG;

    static EncryptHelper()
    {
        SKC_BYTE = (byte)Random.Range(99, byte.MaxValue);
        SKC_SHORT = (short)Random.Range(9999, short.MaxValue);
        SKC_USHORT = (ushort)Random.Range(9999, ushort.MaxValue);
        SKC_INT = (int)Random.Range(9999, int.MaxValue);
        SKC_UINT = (uint)Random.Range(9999, int.MaxValue);
        SKC_LONG = (long)Random.Range(9999, int.MaxValue);
        SKC_ULONG = (ulong)Random.Range(9999, int.MaxValue);
    }

    /// <summary>
    /// test code
    /// </summary>
    public static void TestSecureFunc()
    {
        byte _byte = SSecureBYTE(100);
        Debug.LogWarning(_byte + " : " + GSecureBYTE(_byte));

        short _short = SSecureSHORT(30000);
        Debug.LogWarning(_short + " : " + GSecureSHORT(_short));

        ushort _ushort = SSecureUSHORT(60000);
        Debug.LogWarning(_ushort + " : " + GSecureUSHORT(_ushort));

        int _int = SSecureINT(1000000000);
        Debug.LogWarning(_int + " : " + GSecureINT(_int));

        uint _uint = SSecureUINT(4000000000);
        Debug.LogWarning(_uint + " : " + GSecureUINT(_uint));

        long _long = SSecureLONG(10000000000000);
        Debug.LogWarning(_long + " : " + SSecureLONG(_long));

        ulong _ulong = SSecureULONG(40000000000000);
        Debug.LogWarning(_ulong + " : " + SSecureULONG(_ulong));
    }

    public static byte SSecureBYTE(byte value) { return (byte)(value ^ SKC_BYTE); }
    public static byte GSecureBYTE(byte value) { return (byte)(value ^ SKC_BYTE); }

    public static short SSecureSHORT(short value) { return (short)(value ^ SKC_SHORT); }
    public static short GSecureSHORT(short value) { return (short)(value ^ SKC_SHORT); }

    public static ushort SSecureUSHORT(ushort value) { return (ushort)(value ^ SKC_USHORT); }
    public static ushort GSecureUSHORT(ushort value) { return (ushort)(value ^ SKC_USHORT); }

    public static int SSecureINT(int value) { return (int)(value ^ SKC_INT); }
    public static int GSecureINT(int value) { return (int)(value ^ SKC_INT); }

    public static uint SSecureUINT(uint value) { return (uint)(value ^ SKC_UINT); }
    public static uint GSecureUINT(uint value) { return (uint)(value ^ SKC_UINT); }

    public static long SSecureLONG(long value) { return (long)(value ^ SKC_LONG); }
    public static long GSecureLONG(long value) { return (long)(value ^ SKC_LONG); }

    public static ulong SSecureULONG(ulong value) { return (ulong)(value ^ SKC_ULONG); }
    public static ulong GSecureULONG(ulong value) { return (ulong)(value ^ SKC_ULONG); }
}