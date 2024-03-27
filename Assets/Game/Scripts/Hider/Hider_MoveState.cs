using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Hider_MoveState : Hider_BaseState
{
    public float angularDampeningTime = 5.0f;
    public float deadZone = 10.0f;

    private readonly int SpeedParameter = Animator.StringToHash("Speed");

    private AnimationListener animationListener;

    public float distanceThreshold = 0.1f;
    public float rotationThreshold = 0.1f;
    public float correctionTime = 1.0f;
    public bool reachingPoint = false;
    public bool completedRotation = false;

    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);
        animationListener = owner.GetComponent<AnimationListener>();
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        controller.RandomSelectHidePoint();
        animationListener.OnAnimatorMoveEvent += OnAnimatorMove;
    }

    private void OnAnimatorMove()
    {
        agent.velocity = hiderAnimator.deltaPosition / Time.deltaTime;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (agent.desiredVelocity != Vector3.zero)
        {
            float speed = Vector3.Project(agent.desiredVelocity, transform.forward).magnitude * agent.speed;
            hiderAnimator.SetFloat(SpeedParameter, speed);

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

            // Check if the hider is close enough
            float distance = (controller.currentHidePoint.transform.position - transform.position).magnitude;

            if (distance <= distanceThreshold)
            {
                hiderAnimator.SetFloat(SpeedParameter, 0.0f);
                agent.ResetPath();

                fsm.ChangeState(fsm.HideState);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animationListener.OnAnimatorMoveEvent -= OnAnimatorMove;
    }
}
