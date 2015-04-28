using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public delegate void LoadingCallbackFunction();

public class SceneManager : Immortal<SceneManager>
{
    public eGame SelectGame = eGame.BASEBALL;
    public Camera BlackCamera;
    public static Resolution OriginalRes;

    public float GetScreenRatio()
    {
        return (float)OriginalRes.height / 1280.0f;
    }

    FSM.FSM<eAction, eState, SceneManager> FSM = null;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (FSM.Current_State == eState.LOBBY)
                Application.Quit();
            else
                ActionEvent(eAction.PREV);
        }
    }

    protected override void Init()
    {
        base.Init();

        if (base.gameObject == null)
            return;

        Application.targetFrameRate = 60;

        //NativeManager.instance.nativeSet();
        //JsonLoader.instance.GetAuthJson();

        Init_FSM();

        //게임중 옵션에 의해 Resolution이 바뀌기 때문에 처음시작시, 원본 ScreenSize 저장
        if (OriginalRes.Equals(default(Resolution)))
            OriginalRes = Screen.currentResolution;

        //해상도 조절
        StartCoroutine(SetResolution(false));

        // Timeout 설정
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
    }

    void Init_FSM()
    {
        //< 초기 State 생성
        FSM = new FSM.FSM<eAction, eState, SceneManager>(this);
        FSM.AddState(eState.START, gameObject.AddComponent<StartState>());
        FSM.AddState(eState.PATCH, gameObject.AddComponent<PatchState>());
        FSM.AddState(eState.NICKNAME, gameObject.AddComponent<NicknameState>());
        FSM.AddState(eState.LOBBY, gameObject.AddComponent<LobbyState>());
        FSM.AddState(eState.GAMELOG, gameObject.AddComponent<GameLogState>());
        FSM.AddState(eState.SETTING, gameObject.AddComponent<SettingState>());
        FSM.AddState(eState.GAME, gameObject.AddComponent<GameState>());

        ////< 이벤트 처리
        FSM.RegistEvent(eState.START, eAction.NEXT, eState.PATCH);
        FSM.RegistEvent(eState.PATCH, eAction.NEXT, eState.LOBBY);
        FSM.RegistEvent(eState.LOBBY, eAction.NEXT, eState.GAME);
        FSM.RegistEvent(eState.GAME, eAction.NEXT, eState.LOBBY);

        FSM.RegistEvent(eState.PATCH, eAction.NICKNAME, eState.NICKNAME);
        FSM.RegistEvent(eState.NICKNAME, eAction.NEXT, eState.LOBBY);

        FSM.RegistEvent(eState.GAME, eAction.PREV, eState.LOBBY);
        FSM.RegistEvent(eState.GAMELOG, eAction.PREV, eState.LOBBY);
        FSM.RegistEvent(eState.SETTING, eAction.PREV, eState.LOBBY);

        FSM.RegistEvent(eState.GAMELOG, eAction.NEXT, eState.SETTING);
        FSM.RegistEvent(eState.SETTING, eAction.NEXT, eState.GAMELOG);

        FSM.RegistEvent(eState.GAMELOG, eAction.REVENGE, eState.GAME);

        FSM.RegistEvent(eState.LOBBY, eAction.GAMELOG, eState.GAMELOG);
        FSM.RegistEvent(eState.LOBBY, eAction.SETTING, eState.SETTING);

        //< 초기값 대입
        FSM.Enable(eState.START);
    }

    public static void ChangeState(eAction _action)
    {
        SceneManager.instance.FSM.ChangeState(_action);
    }

    /// <summary>
    /// 화면을 비율에 맞게 바꾸거나, 고정비율로 스크린 사이즈 변경R
    /// </summary>
    /// <param name="_fillScreen">Device 스크린을 채운 상태로 16:9를 표현할지 유무</param>
    IEnumerator SetResolution(bool _fillScreen = false)
    {
        if (null != BlackCamera)
            DestroyImmediate(BlackCamera.gameObject);

        if (_fillScreen)
        {
            ResolutionUtil.AdjuestCamRect(Camera.allCameras, OriginalRes.width, OriginalRes.height);

            // 해상도 16:10로 강제로 맞추기
            int calcHeight = Mathf.CeilToInt((10f / 16f) * (float)OriginalRes.width);
            Screen.SetResolution(OriginalRes.width, calcHeight, true);
        }
        else
        {
#if UNITY_ANDROID
            Screen.SetResolution(OriginalRes.width, OriginalRes.height, true);
#endif
            yield return null;

            // 10:16 비율로 해상도 변경하기.
            float scaleHeight = ResolutionUtil.AdjuestCamRect(Camera.allCameras, 800, 1280);

            // 현재 해상도가 16:10에서 벗어난다면, Background카메라 생성하기
            if (false == Mathf.Approximately(1.0f, scaleHeight))
                BlackCamera = ResolutionUtil.CreateBlackCamera(-3, true);
        }
    }

    public void ActionEvent(eAction NewAction, bool bLoading = false)
    {
        FSM.ChangeState(NewAction);
    }

    void ShowLoading()
    {
        //if (LoadingPan == null)
        //    LoadingPan = UIManager.instance.OpenUI("UI/LoadingPanel", true, -100).GetComponent<LoadingPanel>();
    }

    public float CurrentRatio;
    public void SetLoadingInfo(string DescStr, float LoadingRatio, LoadingCallbackFunction callback = null)
    {
        CurrentRatio = LoadingRatio;

        ShowLoading();

        //LoadingPan.SetLoadingInfo(DescStr, LoadingRatio, callback);
    }

    public void RepairTimeScale(float a_Delay)
    {
        CancelInvoke("RepTimeScale");
        Invoke("RepTimeScale", a_Delay);
    }

    public void RepTimeScale()
    {
        if (Time.timeScale != 1.0f)
        {
            Time.timeScale = 1.0f;
        }
    }
}