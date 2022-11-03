using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMManager
{
    //状态列表
    public List<FSMState> stateList = new List<FSMState>();
    //当前状态
    public int CurrentIndex = -1;
    public FSMState CurrentState = null;

    //改变状态
    public void ChangeState(int StateID)
    {
        if(CurrentState != null)
        {
            CurrentState.OnExit();
        }
        CurrentIndex = StateID;
        CurrentState = stateList[CurrentIndex];
        CurrentState.OnEnter();
    }

    public void Update()
    {
        if(CurrentState != null)
        {
            CurrentState.OnUpdate();
        }
    }
}
