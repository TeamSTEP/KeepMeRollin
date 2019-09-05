using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject objectToFollow;

    private Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        if (objectToFollow == null)
        {
            //set player object as the default value if none is given
            objectToFollow = GameObject.FindGameObjectWithTag("Player");
        }
        
        //set the camera offset as its initial position
        offset = transform.position - objectToFollow.transform.position;
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //update camera position according to the player's position
        transform.position = objectToFollow.transform.position + offset;
    }
}
