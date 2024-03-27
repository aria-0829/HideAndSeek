using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class YBot_MoveToGoalState : YBot_BaseState
{
    public float OffMeshAngleThreshold = 5.0f;
    public float angularDampeningTime = 5.0f;
    public float deadZone = 10.0f;

    private readonly int SpeedParameter = Animator.StringToHash("Speed");

    private AnimationListener animationListener;

    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);
        animationListener = owner.GetComponent<AnimationListener>();
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationListener.OnAnimatorMoveEvent += OnAnimatorMove;
    }

    private void OnAnimatorMove()
    {
        agent.velocity = YBotAnimator.deltaPosition / Time.deltaTime;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            YBotAnimator.SetTrigger("Crying");
        }

        if (agent.isOnOffMeshLink == true)
        {
            OffMeshLinkAction action = agent.currentOffMeshLinkData.offMeshLink.GetComponent<OffMeshLinkAction>();
            Debug.Assert(action != null, $"{agent.currentOffMeshLinkData.offMeshLink.name} requires an OffMeshLinkAction to complete OffMeshLink movement");

            if (action.applyRotationBlend)
            {
                Vector3 direction = (agent.currentOffMeshLinkData.endPos - agent.currentOffMeshLinkData.startPos).normalized;
                direction.y = 0.0f;
                float angle = Vector3.Angle(transform.forward, direction);
                if (Mathf.Abs(angle) > OffMeshAngleThreshold)
                {
                    transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(direction), Time.deltaTime * angularDampeningTime);
                    return;
                }

                transform.LookAt(transform.position + direction);
            }

            switch (action.action)
            {
                case OffMeshLinkAction.Action.Climb:
                    fsm.ChangeState(fsm.ClimbStateName);
                    YBotAnimator.SetTrigger("Climb");
                    break;

                case OffMeshLinkAction.Action.JumpWithStyle:
                    transform.position = agent.currentOffMeshLinkData.startPos;
                    fsm.ChangeState(fsm.JumpWithStyleStateName);
                    YBotAnimator.SetTrigger("Jump-With-Style");
                    break;

                case OffMeshLinkAction.Action.JumpWithAniamtion:
                    fsm.ChangeState(fsm.JumpAcrossStateName);
                    YBotAnimator.SetTrigger("Jump-Across");
                    break;

                case OffMeshLinkAction.Action.ClimbDown:
                    fsm.ChangeState(fsm.ClimbDownStateName);
                    YBotAnimator.SetTrigger("Climb-Down");
                    break;
            }
        }
        else if (agent.desiredVelocity != Vector3.zero)
        {
            float speed = Vector3.Project(agent.desiredVelocity, transform.forward).magnitude * agent.speed;
            YBotAnimator.SetFloat(SpeedParameter, speed);

            float angle = Vector3.Angle(transform.forward, agent.desiredVelocity);
            if (Mathf.Abs(angle) <= deadZone)
            {
                transform.LookAt(transform.position + agent.desiredVelocity);
            }
            else
            {
                transform.rotation = Quaternion.Lerp(transform.rotation,
                                                     Quaternion.LookRotation(agent.desiredVelocity),
                                                     Time.deltaTime * angularDampeningTime);
            }
        }
        else
        {
            YBotAnimator.SetFloat(SpeedParameter, 0.0f);
            fsm.ChangeState(fsm.IdleStateName);
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationListener.OnAnimatorMoveEvent -= OnAnimatorMove;
    }
}
