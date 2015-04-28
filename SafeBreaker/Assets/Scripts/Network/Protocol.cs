using System;
using System.Collections.Generic;
using System.Text;

public enum PROTOCOL
{
    P_NONE = 0,

    P_SERVICE = 1000,
    P_SERVICE_INFO,

    P_USER = 2000,
    P_USER_LOGIN,
    P_USER_INFO,
    P_USER_LIST,
    P_USER_SETPW,
    P_USER_UPGRADE,
    P_USER_NOTIFY,
    P_USER_NICKNAME,

    P_GAME = 3000,
    P_GAME_START,
    P_GAME_RESULT,
    P_GAME_LOG,
    P_GAME_REVENGE,
}