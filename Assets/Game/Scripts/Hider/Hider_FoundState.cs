using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hider_FoundState : Hider_BaseState
{
    private AnimationListener animationListener;
    private readonly int TriggerFound = Animator.StringToHash("Found");
    private readonly int ANIM_CRY = Animator.StringToHash("Crying");

    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);
        animationListener = owner.GetComponent<AnimationListener>();
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hiderAnimator.SetTrigger(TriggerFound);

        animationListener.AddAnimationCompletedListener(ANIM_CRY, OnCryAnimationCompleted);
    }

    private void OnCryAnimationCompleted(int obj)
    {
        fsm.ChangeState(fsm.MoveState);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationListener.RemoveAnimationCompletedListener(ANIM_CRY, OnCryAnimationCompleted);
    }
}   
