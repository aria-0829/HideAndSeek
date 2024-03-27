using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class YBot_IdleState : YBot_BaseState
{
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Input.GetMouseButton(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                agent.SetDestination(hit.point);
                NavMesh.CalculatePath(transform.position, hit.point, -1, controller.path);
                fsm.ChangeState(fsm.MoveToGoalStateName);
            }
        }
    }
}
