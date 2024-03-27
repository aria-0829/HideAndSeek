using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YBot_ClimbDownState : YBot_BaseState
{
    private AnimationListener animationListener;
    private readonly int ANIM_ClimbingDown = Animator.StringToHash("ClimbingDown");

    private void OnAnimatorMove()
    {
        transform.position = YBotAnimator.rootPosition;
        transform.rotation = YBotAnimator.rootRotation;
    }

    private void OnAnimationCompleted(int _animName)
    {
        agent.CompleteOffMeshLink();
        fsm.ChangeState(fsm.MoveToGoalStateName);
    }

    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);
        animationListener = owner.GetComponent<AnimationListener>();
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationListener.OnAnimatorMoveEvent += OnAnimatorMove;
        animationListener.AddAnimationCompletedListener(ANIM_ClimbingDown, OnAnimationCompleted);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationListener.OnAnimatorMoveEvent -= OnAnimatorMove;
        animationListener.RemoveAnimationCompletedListener(ANIM_ClimbingDown, OnAnimationCompleted);
    }
}
