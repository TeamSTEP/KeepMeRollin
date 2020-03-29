using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(FieldOfView))]
public class BaseAi : MonoBehaviour
{
    private FieldOfView sensors;

    public bool hasVisualSensor = true;

    public bool hasAudioSensor = true;

    [HideInInspector]
    public Vector3 defaultLocation;

    // the location in which the AI will move to
    [HideInInspector]
    public Vector3 pointOfInterest;
    [HideInInspector]
    public EnemyStates currentState;

    private Renderer lineOfSightMeshRenderer;

    // the minimum sound level to detect, lower the for sensitive it is
    [Range(0.1f, 10f)]
    public float soundDetectionThreshold = 4f;

    // how long the enemy will be looking for the player in seconds
    [Range(1, 60)]
    public float searchDuration = 9f;
    // initialize search duration cool-down
    private float searchCoolDown = 0f;

    // Start is called before the first frame update
    void Start()
    {
        pointOfInterest = transform.position;
        defaultLocation = transform.position;

        currentState = EnemyStates.Passive;

        sensors = GetComponent<FieldOfView>();

        lineOfSightMeshRenderer = transform.Find("LineOfSightMesh").GetComponent<Renderer>();
        // lineOfSightMeshRenderer.material.shader = Shader.Find("Lightweight Render Pipeline/Simple Lit");

    }

    // Update is called once per frame
    void FixedUpdate()
    {
        ChangeLineOfSightColor();
        StateControl();
    }

    /// <summary>
    /// Handles the condition for how the AI will transition its state
    /// </summary>
    void StateControl()
    {

        if (hasVisualSensor && CanSeeObject())
        {
            searchCoolDown = Time.time + searchDuration;
            currentState = EnemyStates.Aggressive;
        }
        else if (hasAudioSensor && CanHearObject())
        {
            searchCoolDown = Time.time + searchDuration;
            // only change the state when the AI is not aggressive
            if(currentState != EnemyStates.Aggressive)
            {
                currentState = EnemyStates.Detect;
            }
        }
        // set state to passive if there is no feedback from object for a certain amount of time
        if(searchCoolDown <= Time.time)
        {
            currentState = EnemyStates.Passive;
        }

        // draw point of interest of AI
        Debug.DrawLine(transform.position, pointOfInterest, Color.blue);

        // draw a line between the sensor object and the targets
        foreach (Transform visibleTarget in sensors.visibleTargets)
        {
            Debug.DrawLine(sensors.transform.position, visibleTarget.position, Color.red);
        }
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
                break;
            default:
                if (lineOfSightMeshRenderer.material.GetColor("_BaseColor") != Color.green)
                {
                    lineOfSightMeshRenderer.material.SetColor("_BaseColor", Color.green);
                }
                break;
        }
    }

    /// <summary>
    /// Returns true when the AI can see an object. This also sets the condition for when the AI can see a certain object
    /// </summary>
    /// <returns></returns>
    bool CanSeeObject()
    {
        if (sensors.visibleTargets.Count > 0)
        {
            foreach (var i in sensors.visibleTargets)
            {
                // only check objects with the tag Player
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

    /// <summary>
    /// Returns true when the AI can hear an object. This also defines what is audible
    /// </summary>
    /// <returns></returns>
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

}


