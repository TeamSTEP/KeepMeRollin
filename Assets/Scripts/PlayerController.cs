using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(1, 10)]
    public float maxSpeed = 4.5f;

    private float currentSpeed = 0f;

    private Vector3 moveDir = Vector3.zero;

    private CharacterController controller;

    private Transform cameraTransform;

    private SoundEmitter soundEmitter;

    private Vector3 charScale;

    [Range(1, 20)]
    public float gravity = 10f;


    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
        soundEmitter = GetComponent<SoundEmitter>();
        //assign the main camera as the variable
        cameraTransform = Camera.main.transform;

        charScale = transform.localScale;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Sneaking();

        Debug.Log("Current Speed of player: " + currentSpeed);

        MovePlayer();

    }

    /// <summary>
    /// the main player movement method. It uses the camera direction for the player movement
    /// </summary>
    void MovePlayer()
    {
        //todo: change it when implementing joystick
        if (controller.isGrounded)
        {
            //get camera axis of the transform in world spaces
            Vector3 forwardCam = cameraTransform.forward;
            Vector3 rightCam = cameraTransform.right;

            //get player input from keyboard as vector 2
            Vector2 playerInput = PlayerKeyboardInput();

            //clamp the input value to be -1 ~ 1
            playerInput = Vector2.ClampMagnitude(playerInput, 1);

            //normalize the cam value
            forwardCam.y = 0;
            rightCam.y = 0;
            forwardCam = forwardCam.normalized;
            rightCam = rightCam.normalized;

            //calculate the player's movement direction
            moveDir = (forwardCam * playerInput.y + rightCam * playerInput.x) * Time.deltaTime;
            //add movement speed
            moveDir *= currentSpeed;

        }

        if ((Input.GetAxis("Horizontal") != 0f || Input.GetAxis("Vertical") != 0f))
        {
            //todo: make the value vary depending on the floor type
            soundEmitter.currentSoundLevel = currentSpeed;
        }

        //simulated gravity by decreasing the y value by every ms
        moveDir.y -= gravity * Time.deltaTime;

        //finally change the controller component
        controller.Move(moveDir);

    }

    private Vector2 PlayerKeyboardInput()
    {
        
        return new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void Sneaking()
    {
        if (Input.GetButton("Sneak"))
        {
            currentSpeed = maxSpeed / 2;
            //shrink the y value by half
            transform.localScale = charScale - new Vector3(0, charScale.y / 2f, 0);
        }
        else
        {
            currentSpeed = maxSpeed;
            //make the size back to normal
            transform.localScale = charScale;
        }
    }
}
