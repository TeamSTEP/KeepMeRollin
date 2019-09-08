using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    public Transform player;

    private PlayerController playerControl;

    private bool touchStart = false;
    private Vector2 pointA;
    private Vector2 pointB;

    public Transform innerCircle;
    public Transform outerCircle;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        playerControl = player.GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            pointA = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
            //pointA = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

            innerCircle.position = pointA;
            outerCircle.position = pointA;

        }
        if (Input.GetMouseButton(0))
        {
            touchStart = true;
            pointB = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.transform.position.z));
            //pointB = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        }
        else
        {
            touchStart = false;
        }

    }

    void FixedUpdate()
    {
        if (touchStart)
        {
            Vector2 offset = pointB - pointA;
            Vector2 direction = Vector2.ClampMagnitude(offset, 1f);
            playerControl.MovePlayer(direction, playerControl.maxSpeed);

            innerCircle.position = new Vector2(pointA.x + direction.x, pointA.y + direction.y);


        }
        
    }
}
