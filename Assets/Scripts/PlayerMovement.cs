using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float movementSpeed;
    public float sprintSpeed;
    public float crouchSpeed;
    public float acceleration;
    public float jumpHeight;
    public float maxSpeed;
    public float strafeAcceleration;
    public float sensitivity;
    public float jumpDelay;

    public int maxJumpCount;

    public bool isGrounded;

    public AudioSource jumpSound;

    float lastJump;

    int jumpCount;

    [HideInInspector]
    public float currentSpeed;

    float xRot;
    float t;

    Vector3 dir;
    Vector3 movement;

    Camera playerCamera;

    Rigidbody rigid;

    void Start()
    {
        isGrounded = true;

        playerCamera = Camera.main;

        rigid = gameObject.GetComponent<Rigidbody>();

        if (PlayerPrefs.GetInt("sensitivity") == 0)
        {
            sensitivity = 10;
        }

        else
        {
            sensitivity = PlayerPrefs.GetInt("sensitivity");
        }
    }

    void Update()
    {
        // MOUSE INPUT
        xRot += Input.GetAxisRaw("Mouse Y") * sensitivity * Time.deltaTime * 100;

        xRot = Mathf.Clamp(xRot, -90.0f, 90.0f);

        playerCamera.transform.localEulerAngles = new Vector3(-xRot, playerCamera.transform.localEulerAngles.y, playerCamera.transform.localEulerAngles.z);
        transform.Rotate(new Vector3(0, Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime * 100, 0));

        // SPEED CAP
        if (currentSpeed > maxSpeed)
        {
            currentSpeed = maxSpeed;
        }

        // MOVEMENT
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0)
        {
            t = 0f;

            currentSpeed += Mathf.Abs(Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime * strafeAcceleration);

            // SPRINTING
            if ((currentSpeed < movementSpeed || isGrounded) && Input.GetButton("Sprint"))
            {
                playerCamera.transform.localPosition = new Vector3(0, 1f, 0);

                currentSpeed = Mathf.Lerp(currentSpeed, sprintSpeed, t += Time.deltaTime * acceleration);
            }

            // CROUCHING
            else if (isGrounded && Input.GetButton("Crouch") && !Input.GetButton("Sprint"))
            {
                playerCamera.transform.localPosition = new Vector3(0, 0.8f, 0);

                currentSpeed = Mathf.Lerp(currentSpeed, crouchSpeed, t += Time.deltaTime * acceleration);
            }

            // WALKING
            else if ((currentSpeed < movementSpeed || isGrounded))
            {
                playerCamera.transform.localPosition = new Vector3(0, 1f, 0);

                currentSpeed = Mathf.Lerp(currentSpeed, movementSpeed, t += Time.deltaTime * acceleration);
            }

            // MOVEMENT CAP
            if (Mathf.Abs(currentSpeed - movementSpeed) < 0.01f)
            {
                currentSpeed = movementSpeed;
            }

            dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        }

        // STOPPING
        else
        {
            t = 0f;

            currentSpeed = Mathf.Lerp(currentSpeed, 0, t += Time.deltaTime * acceleration);

            if (currentSpeed < 0.01f)
            {
                currentSpeed = 0f;
            }
        }

        movement = dir.normalized * currentSpeed / 20;

        // JUMPING
        if (Input.GetButtonDown("Jump") && (Time.time - lastJump > jumpDelay))
        {
            if (jumpCount > 0)
            {
                isGrounded = false;

                jumpCount -= 1;

                rigid.velocity = Vector3.zero;
                rigid.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);

                if (jumpSound)
                {
                    jumpSound.Play();
                }
            }

            lastJump = Time.time;
        }
    }

    void FixedUpdate()
    {
        rigid.MovePosition(rigid.position + transform.TransformDirection(movement) * Time.deltaTime);
    }

    void OnCollisionEnter(Collision other) {
        isGrounded = true;

        jumpCount = maxJumpCount;
    }
}