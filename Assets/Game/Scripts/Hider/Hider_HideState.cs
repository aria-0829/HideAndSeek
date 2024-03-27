using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Hider_HideState : Hider_BaseState
{
    public float angularDampeningTime = 5.0f;
    public float deadZone = 10.0f;

    private AnimationListener animationListener;
    private readonly int TriggerCover = Animator.StringToHash("Cover");
    private readonly int ANIM_Cover = Animator.StringToHash("T-Pose@Stand To Cover");

    public float rotationThreshold = 0.1f;
    public float correctionTime = 1.0f;

    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);
        animationListener = owner.GetComponent<AnimationListener>();
        Debug.Assert(animationListener != null, $"{owner.name} requires a AnimationListener component");
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        hiderAnimator.SetTrigger(TriggerCover);

        animationListener.OnAnimatorMoveEvent += OnAnimatorMove;
        animationListener.AddAnimationCompletedListener(ANIM_Cover, OnAnimationCompleted);
    }

    private void OnAnimatorMove()
    {
        transform.position = hiderAnimator.rootPosition;
        transform.rotation = hiderAnimator.rootRotation;

        Vector3 direction = controller.currentHidePoint.transform.forward;
        direction.y = 0.0f;
        float rotateAngle = Vector3.Angle(transform.forward, direction);

        if (Mathf.Abs(rotateAngle) > rotationThreshold)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation,
                                                 Quaternion.LookRotation(direction),
                                                 Time.deltaTime * correctionTime);
        }
        else
        {
            //completedRotation = true;
            transform.LookAt(transform.position + direction);
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
    }

    private void OnAnimationCompleted(int shortHashName)
    {
        fsm.ChangeState(fsm.WaitState);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationListener.OnAnimatorMoveEvent -= OnAnimatorMove;
        animationListener.RemoveAnimationCompletedListener(ANIM_Cover, OnAnimationCompleted);
    }
}
