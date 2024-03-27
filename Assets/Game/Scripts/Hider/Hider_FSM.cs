using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(HiderController))]
public class Hider_FSM : FSM
{
    public readonly int MoveState = Animator.StringToHash("Move");
    public readonly int HideState = Animator.StringToHash("Hide");
    public readonly int WaitState = Animator.StringToHash("Wait");
    public readonly int FoundState = Animator.StringToHash("Found");
}
