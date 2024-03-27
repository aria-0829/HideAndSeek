using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hider_WaitState : Hider_BaseState
{
    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.Instance.OnFound += HandleFoundEvent;
    }

    private void HandleFoundEvent()
    {
        fsm.ChangeState(fsm.FoundState);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        GameManager.Instance.OnFound -= HandleFoundEvent;
    }
}   
