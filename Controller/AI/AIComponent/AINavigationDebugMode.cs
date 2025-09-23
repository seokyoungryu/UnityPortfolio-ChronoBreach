using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AINavigationDebugMode : MonoBehaviour
{
    public bool velocity = false;
    public bool desiredVelocity = false;
    public bool path = false;


    NavMeshAgent nav;

    private void Awake()
    {
        nav = GetComponent<NavMeshAgent>();
    }

    private void OnDrawGizmos()
    {
        if (!Application.isPlaying) return;

        if (velocity)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + nav.velocity);
        }

        if (desiredVelocity)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, transform.position + nav.desiredVelocity);
        }

        if(path)
        {
            Gizmos.color = Color.black;
            NavMeshPath navPath = nav.path;
            Vector3 prevCorner = transform.position;
            foreach (Vector3 corner in navPath.corners)
            {
                Gizmos.DrawLine(prevCorner, corner);
                Gizmos.DrawSphere(corner, 0.2f);
                prevCorner = corner;
            }
        }

    }



}
