using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Range(1, 10)]
    public float maxSpeed = 4.5f;

    private float currentSpeed = 0f;

    private Transform cameraTransform;

    private SoundEmitter soundEmitter;

    private Vector3 charScale;

    private Rigidbody rb;

    private Vector2 pcControls;

    [Range(1, 20)]
    public float gravity = 10f;


    // Start is called before the first frame update
    void Start()
    {
        soundEmitter = GetComponent<SoundEmitter>();
        //assign the main camera as the variable
        cameraTransform = Camera.main.transform;

        charScale = transform.localScale;

        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        pcControls = PlayerKeyboardInput();
    }

    void FixedUpdate()
    {
        MovePlayer(pcControls, maxSpeed);
    }

    /// <summary>
    /// the main player movement method. It uses the camera direction for the player movement
    /// the controlVector value must be clamped to 1
    /// </summary>
    public void MovePlayer(Vector2 controlVector, float moveSpeed)
    {
        //get camera axis of the transform in world spaces
        Vector3 forwardCam = cameraTransform.forward;
        Vector3 rightCam = cameraTransform.right;

        //normalize the cam value
        forwardCam.y = 0;
        rightCam.y = 0;
        forwardCam = forwardCam.normalized;
        rightCam = rightCam.normalized;

        rb.MovePosition(transform.position + ((forwardCam * controlVector.y + rightCam * controlVector.x) * moveSpeed * Time.deltaTime));
    }

    private Vector2 PlayerKeyboardInput()
    {
        return Vector2.ClampMagnitude(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")), 1);
    }

    private void Sneaking()
    {
        //todo: this currently has keyboard controls in mind, must change to joystick later
        if (Input.GetButton("Sneak"))
        {
            currentSpeed = maxSpeed / 2;
            //shrink the y value of the character mesh by half
            transform.localScale = charScale - new Vector3(0, charScale.y / 2f, 0);
        }
        else
        {
            currentSpeed = maxSpeed;
            //make the character mesh size back to normal
            transform.localScale = charScale;
        }
    }
}
