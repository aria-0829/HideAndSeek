using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(YBot_FSM))]
public class YBot_Controller : MonoBehaviour
{
    [HideInInspector] public NavMeshPath path;
    public bool drawPath;

    void Start()
    {
        path = new NavMeshPath();
    }

    private void OnDrawGizmos()
    {
    }
}
