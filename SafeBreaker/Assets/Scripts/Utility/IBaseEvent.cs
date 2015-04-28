using UnityEngine;
using System.Collections;

/// <summary>
/// EventHandler에서 보내질 Event들의 interface.
/// </summary>
/// 
public enum eFSMEvent
{
    None,
    Damage,
    Attack,
}

public interface IBaseEvent
{
    int eventID { get; set; }
}