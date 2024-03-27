using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class YBot_JumpWithStyleState : YBot_BaseState
{
    public enum JumpStates
    {
        JumpStart,
        JumpFlightStart,
        JumpFlight,
        JumpLanding,
        JumpCompleted
    }
    private JumpStates jumpState = JumpStates.JumpStart;

    public float JumpHeight = 1.25f;
    public float JumpTime = 1.0f;

    private float currentTime = 0.0f;
    private Vector3 startPosition = Vector3.zero;
    private Vector3 endPosition = Vector3.zero;

    private AnimationListener animationListener;

    private readonly int ANIM_JumpStart = Animator.StringToHash("Jump-Start");
    private readonly int ANIM_JumpLand = Animator.StringToHash("Jump-Land");

    private void OnAnimationCompleted(int _animationName)
    {
        if (_animationName == ANIM_JumpStart)
        {
            jumpState = JumpStates.JumpFlight;
            startPosition = transform.position;
        }
        else if (_animationName == ANIM_JumpLand)
        {
            jumpState = JumpStates.JumpCompleted;
        }
    }

    // or
    //private void OnAnimationJumpStartCompleted(int _animationName)
    //{
    //}
    //private void OnAnimationJumpLandCompleted(int _animationName)
    //{
    //}

    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);
        animationListener = owner.GetComponent<AnimationListener>();
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentTime = 0.0f;
        jumpState = JumpStates.JumpStart;

        endPosition = agent.currentOffMeshLinkData.endPos;
        startPosition = agent.currentOffMeshLinkData.startPos;

        animationListener.AddAnimationCompletedListener(ANIM_JumpStart, OnAnimationCompleted);
        animationListener.AddAnimationCompletedListener(ANIM_JumpLand, OnAnimationCompleted);

        // or
        //animationListener.AddAnimationCompletedListener(ANIM_JumpStart, OnAnimationJumpStartCompleted);
        //animationListener.AddAnimationCompletedListener(ANIM_JumpLand, OnAnimationJumpLandCompleted);
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        switch(jumpState)
        {
            case JumpStates.JumpStart:
                break;

            case JumpStates.JumpFlight:
                currentTime += Time.deltaTime;
                if (currentTime < 1.0f)
                {
                    float normalizedTime = currentTime / JumpTime;
                    float yOffset = JumpHeight * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
                    transform.position = Vector3.Lerp(startPosition, endPosition, normalizedTime) + yOffset * Vector3.up;
                }
                else
                {
                    YBotAnimator.SetTrigger("Jump-Landing");
                    jumpState = JumpStates.JumpLanding;
                }
                break;

            case JumpStates.JumpLanding:
                break;

            case JumpStates.JumpCompleted:
                agent.CompleteOffMeshLink();
                fsm.ChangeState(fsm.MoveToGoalStateName);
                break;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationListener.RemoveAnimationCompletedListener(ANIM_JumpStart, OnAnimationCompleted);
        animationListener.RemoveAnimationCompletedListener(ANIM_JumpLand, OnAnimationCompleted);
    }

}
