using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class YBot_BaseState : FSMBaseState<YBot_FSM>
{
    protected YBot_Controller controller;
    protected NavMeshAgent agent;
    protected Transform transform;
    protected Animator YBotAnimator;

    public override void Init(GameObject _owner, FSM _fsm)
    {
        base.Init(_owner, _fsm);

        controller = owner.GetComponent<YBot_Controller>();
        Debug.Assert(controller != null, $"{owner.name} requires a YBot_Controller Component");

        agent = owner.GetComponent<NavMeshAgent>();
        Debug.Assert(agent != null, $"{owner.name} requires a NavMeshAgent Component");

        transform = owner.GetComponent<Transform>();

        YBotAnimator = owner.GetComponent<Animator>();
        Debug.Assert(YBotAnimator != null, $"{owner.name} requires an Animator Component");
    }
}
