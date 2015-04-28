public enum ePanel
{
    StartPanel = 1,
    PatchPanel,
    LobbyPanel,
    GameLogPanel,
    SettingPanel,
    BaseballPanel,
    BaseballPanel2,
    DialSafePanel,
    InfoPanel,
    NicknamePanel,
}
public enum eState
{
    START = 1,
    PATCH,
    NICKNAME,
    LOBBY,
    GAMELOG,
    SETTING,
    GAME,
}

public enum eAction
{
    NEXT = 1,
    PREV,
    GAMELOG,
    REVENGE,
    SETTING,
    NICKNAME,
}

public enum eBaseballResult
{
    OUT = 1,
    BALL,
    STRIKE
}

public enum eGame
{
    BASEBALL = 1,
    DIALSAFE,
}

public enum eUpgrade
{
    SAFE_LV = 1,
    LOCK_LV,
}

public enum eYNPopup
{
    ATTACK = 1,
    REVENGE,
    CHANGE_PW,
}

public enum eNotify
{
    UPGRADE_SAFELV = 1,
    UPGRADE_LOCKLV,
    SAFE_FULL,
}

public enum eError
{
    // DB ERROR
    ERR_DB = -1001,
    // 소지금 부족
    ERR_USER_NOTENOUGH_COIN = -2001,
    // 하트 부족
    ERR_USER_NOTENOUGH_HEART = -2002,
    // 공격 제한시간
    ERR_GAME_LIMITTIME = -3001,

    // Client Error
    // 공격 대상 없음
    ERR_GAME_NOTARGET = -100000,
    // 중복 숫자
    ERR_GAME_SAMEKEY,
    // 업그레이드 맥스
    ERR_USER_MAXUPGRADE,
    // 이전 비밀번호 동일
    ERR_USER_SAME_PW,
    // 네트워크 에러
    ERR_NETWORK,
}
