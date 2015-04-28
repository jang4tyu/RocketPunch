using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StateBase : FSM.BaseState<SceneManager>{
	
    public override void OnEnter(System.Action callback)
    {
        base.OnEnter(callback);
    }

    public override void OnExit(System.Action callback)
    {
        base.OnExit(callback);
    }

    public void SceneSetting()
    {
    }
}
