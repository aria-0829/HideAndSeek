using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class YBot_ClimbState : YBot_BaseState
{
    private AnimationListener animationListener;

    private readonly int ANIM_Climb = Animator.StringToHash("Climbing");

    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);
        animationListener = owner.GetComponent<AnimationListener>();
    }

    private void OnAnimatorMove()
    {
        transform.position = YBotAnimator.rootPosition;
    }

    private void OnAnimationCompleted(int shortHashName)
    {
        agent.CompleteOffMeshLink();
        fsm.ChangeState(fsm.MoveToGoalStateName);
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationListener.OnAnimatorMoveEvent += OnAnimatorMove;
        animationListener.AddAnimationCompletedListener(ANIM_Climb, OnAnimationCompleted);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationListener.OnAnimatorMoveEvent -= OnAnimatorMove;
        animationListener.RemoveAnimationCompletedListener(ANIM_Climb, OnAnimationCompleted);
    }
}
