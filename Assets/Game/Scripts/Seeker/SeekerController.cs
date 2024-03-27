using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent), typeof(Animator))]
public class SeekerController : MonoBehaviour
{
    public Transform target;

    public GameObject WaypointParent;
    public float distanceToWaypoint = 0.5f;
    public float angularDampeningTime = 5.0f;
    public float deadZone = 10.0f;
    public float sightDistance = 10.0f;
    public float cheerTime = 5.0f;
    public float fov = 70.0f;

    private int index = 0;
    private List<GameObject> waypoints = new List<GameObject>();
    private NavMeshAgent agent;
    private Animator animator;
    private float currentCheerTime = 0.0f;
    private bool canSee = false;

    private enum States
    {
        Walk,
        Cheer
    };
    private States state = States.Walk;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        if (WaypointParent != null)
        {
            foreach (Transform waypoint in WaypointParent.transform)
            {
                waypoints.Add(waypoint.gameObject);
            }
            agent.SetDestination(waypoints[index].transform.position);
        }
    }

    private void OnAnimatorMove()
    {
        switch (state)
        {
            case States.Walk:
                agent.velocity = animator.deltaPosition / Time.deltaTime;
                break;

            case States.Cheer:
                break;
        }
    }

    void Update()
    {
        switch (state)
        {
            case States.Walk:
                Walk();
                break;

            case States.Cheer:
                Cheer();
                break;
        }
    }

    private void Walk()
    {
        float distance = (waypoints[index].transform.position - transform.position).magnitude;
        if (distance < distanceToWaypoint || agent.isStopped || agent.desiredVelocity == Vector3.zero)
        {
            index++;
            if (index >= waypoints.Count)
            {
                index = 0;
            }
            agent.SetDestination(waypoints[index].transform.position);
        }

        float speed = Vector3.Project(agent.desiredVelocity, transform.forward).magnitude * agent.speed;
        animator.SetFloat("Speed", speed);

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

        if (target != null)
        {
            canSee = false;

            Vector3 dirToTarget = target.position - transform.position;
            Vector3 floor = transform.position;

            dirToTarget.y = 0.5f;
            floor.y = 0.5f;

            float viewAngle = Vector3.Angle(transform.forward, dirToTarget);
            if (viewAngle < fov * 0.5f)
            {
                float dstToTarget = Vector3.Distance(transform.position, target.position);
                RaycastHit hitInfo;
                if (Physics.Raycast(transform.position, dirToTarget, out hitInfo, sightDistance))
                {
                    if (hitInfo.transform == target)
                    {
                        if (GameManager.Instance.OnFound != null)
                        {
                            GameManager.Instance.OnFound();
                        }
                        state = States.Cheer;
                        currentCheerTime = cheerTime;
                        animator.SetBool("Cheer", true);
                        canSee = true;
                        agent.isStopped = true;
                    }
                }
            }
        }
    }

    private void Cheer()
    {
        currentCheerTime -= Time.deltaTime;
        if (currentCheerTime <= 0.0f)
        {
            state = States.Walk;
            animator.SetBool("Cheer", false);
            agent.isStopped = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (canSee)
        {
            Handles.color = new Color(1, 0, 0, 0.05f);
        }
        else
        {
            Handles.color = new Color(0, 1, 0, 0.05f);
        }
        Handles.DrawSolidArc(transform.position, Vector3.up, Quaternion.Euler(0, fov * -0.5f, 0) * transform.forward, fov, sightDistance);
    }
}
