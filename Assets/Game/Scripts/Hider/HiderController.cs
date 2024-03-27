using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Hider_FSM))]
public class HiderController : MonoBehaviour
{
    private NavMeshAgent agent;

    [SerializeField] private GameObject HidepointParent;
    private List<GameObject> hidepoints = new List<GameObject>();
    private int index = 0;
    public GameObject currentHidePoint;
    private GameObject lastHidePoint;   

    // On Gizmos
    public bool drawPath = true;
    private NavMeshPath path;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        path = new NavMeshPath();

        if (HidepointParent != null)
        {
            foreach (Transform hidepoint in HidepointParent.transform)
            {
                hidepoints.Add(hidepoint.gameObject);
            }
        }

    }

    public void RandomSelectHidePoint() 
    {
        while (currentHidePoint == lastHidePoint)
        {
            index = UnityEngine.Random.Range(0, hidepoints.Count);
            currentHidePoint = hidepoints[index];
        }
        
        agent.SetDestination(currentHidePoint.transform.position);
        lastHidePoint = currentHidePoint;
        Debug.Log("SelectHidePoint: " + currentHidePoint.name);
    }

    private void OnDrawGizmos()
    {
        if (drawPath && path != null && path.corners.Length > 0)
        {
            Color drawColor;

            if (path.status == NavMeshPathStatus.PathComplete)
            {
                drawColor = Color.green;
            }
            else if (path.status == NavMeshPathStatus.PathInvalid)
            {
                drawColor = Color.red;
            }
            else
            {
                drawColor = Color.yellow;
            }

            for (int i = 1; i < path.corners.Length; i++)
            {
                Debug.DrawLine(path.corners[i - 1], path.corners[i]);
            }
        }
    }
}
