using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[RequireComponent(typeof(BaseAi))]
public class TriadAi : MonoBehaviour
{
    private NavMeshAgent nav;

    private BaseAi aiState;

    [Range(0.1f, 2.5f)]
    public float passiveSpeed = 1.0f;

    private float maxSpeed;

    // Start is called before the first frame update
    void Start()
    {
        aiState = GetComponent<BaseAi>();

        // get navigation component
        nav = GetComponent<NavMeshAgent>();
        nav.isStopped = true;

        // set the max speed to be same as the player (temp)
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            maxSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().maxSpeed;
        }
        else
        {
            maxSpeed = 5f;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TriadBehaviorControl(aiState);
    }

    void TriadBehaviorControl(BaseAi baseAi)
    {
        switch (baseAi.currentState)
        {
            case EnemyStates.Aggressive:
                Debug.Log("I am angry");
                MoveTowardsPoint(baseAi.pointOfInterest, maxSpeed);
                break;
            case EnemyStates.Detect:
                Debug.Log("I am searching");
                MoveTowardsPoint(baseAi.pointOfInterest, passiveSpeed);
                break;
            default:
                Debug.Log("I am passive");
                // add walk path
                nav.isStopped = true;
                break;
        }

    }

    /// <summary>
    /// Move the nav agent to the given point in the game world with the given speed.
    /// By using the nav mesh, the agent will move until it is has reached it's destination
    /// </summary>
    /// <param name="point"></param>
    /// <param name="speed"></param>
    void MoveTowardsPoint(Vector3 point, float speed)
    {
        if (nav.isStopped)
        {
            nav.isStopped = false;
        }
        if (nav.speed != speed)
            nav.speed = speed;

        nav.SetDestination(point);
    }

}
