using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

public class EnemyAI : MonoBehaviour
{

    private NavMeshAgent nav;

    private FieldOfView sensors;

    // the location in which the AI will move to
    [HideInInspector]
    public Vector3 pointOfInterest;

    public EnemyStates currentState;

    [Range(0.1f, 2.5f)]
    public float passiveSpeed = 1.0f;

    private float maxSpeed;

    private Renderer lineOfSightMeshRenderer;

    // the minimum sound level to detect, lower the for sensitive it is
    [Range(0.1f, 10f)]
    public float soundDetectionThreshold = 4f;

    // how long the enemy will be looking for the player in seconds
    [Range(1, 60)]
    public int searchDuration = 9;

    // Start is called before the first frame update
    void Start()
    {
        pointOfInterest = transform.position;

        currentState = EnemyStates.Passive;

        // get navigation component
        nav = GetComponent<NavMeshAgent>();
        nav.isStopped = true;

        // set the max speed to be same as the player
        if (GameObject.FindGameObjectWithTag("Player") != null)
        {
            maxSpeed = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>().maxSpeed;
        }
        else
        {
            maxSpeed = 5f;
        }
        
        sensors = GetComponent<FieldOfView>();

        lineOfSightMeshRenderer = transform.Find("LineOfSightMesh").GetComponent<Renderer>();
        // lineOfSightMeshRenderer.material.shader = Shader.Find("Lightweight Render Pipeline/Simple Lit");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ChangeLineOfSightColor();

        MainBehaviorControl();
    }

    void ChangeLineOfSightColor()
    {
        switch (currentState)
        {
            case EnemyStates.Detect:
                if (lineOfSightMeshRenderer.material.GetColor("_BaseColor") != Color.yellow)
                {
                    lineOfSightMeshRenderer.material.SetColor("_BaseColor", Color.yellow);
                }
                break;
            case EnemyStates.Aggressive:
                if (lineOfSightMeshRenderer.material.GetColor("_BaseColor") != Color.red)
                {
                    lineOfSightMeshRenderer.material.SetColor("_BaseColor", Color.red);
                }
                // lineOfSightMeshRenderer.material.SetColor("_BaseColor", Color.red);
                break;
            default:
                // lineOfSightMeshRenderer.material.SetColor("_BaseColor", Color.green);
                if (lineOfSightMeshRenderer.material.GetColor("_BaseColor") != Color.green)
                {
                    lineOfSightMeshRenderer.material.SetColor("_BaseColor", Color.green);
                }
                break;
        }
    }

    /// <summary>
    /// The main behavior of the AI. This method will handle what the AI will do for each state
    /// </summary>
    void MainBehaviorControl()
    {
        
        if (CanSeeObject())
        {
            Debug.Log("AI can see");
            currentState = EnemyStates.Aggressive;
            MoveTowardsPoint(pointOfInterest, maxSpeed);
        }
        else if (CanHearObject())
        {
            Debug.Log("AI can hear");
            currentState = EnemyStates.Detect;
            MoveTowardsPoint(pointOfInterest, passiveSpeed);
        }

        if (nav.remainingDistance <= nav.stoppingDistance)
        {
            Debug.Log("object has stopped");
            nav.isStopped = true;
            currentState = EnemyStates.Passive;
        }

        Debug.DrawLine(transform.position, pointOfInterest, Color.blue);
    }

    bool CanSeeObject()
    {
        if (sensors.visibleTargets.Count > 0)
        {
            foreach(var i in sensors.visibleTargets)
            {
                if (i.CompareTag("Player"))
                {
                    Debug.Log("AI can see player");
                    pointOfInterest = i.position;
                    return true;
                }
                else
                {
                    continue;
                }

            }
            return false;
            
        }
        return false;
    }

    bool CanHearObject()
    {
        if (sensors.audibleTargets.Count != 0)
        {
            foreach (var i in sensors.audibleTargets)
            {
                // check if the object has a sound emitter component
                if (i.gameObject.GetComponent<SoundEmitter>() != null && i.gameObject.GetComponent<SoundEmitter>().currentSoundLevel > soundDetectionThreshold)
                {
                    pointOfInterest = i.position;

                    return true;
                }
            }
        }
        return false;
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

//enums used for AI behaviors
public enum EnemyStates
{
    Passive, // default state, will follow normal route
    Detect, // AI sees something unusual, will move to point of interest
    Aggressive // AI sees player, will run to catch the player
}


