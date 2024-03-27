using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Hider_BaseState : FSMBaseState<Hider_FSM>
{
    protected HiderController controller;
    protected NavMeshAgent agent;
    protected Transform transform;
    protected Animator hiderAnimator;

    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);

        controller = owner.GetComponent<HiderController>();
        Debug.Assert(controller != null, $"{owner.name} requires a HiderController Component");

        agent = owner.GetComponent<NavMeshAgent>();
        Debug.Assert(agent != null, $"{owner.name} requires a NavMeshAgent Component");

        transform = owner.GetComponent<Transform>();

        hiderAnimator = owner.GetComponent<Animator>();
        Debug.Assert(hiderAnimator != null, $"{owner.name} requires an Animator Component");
    }
}
