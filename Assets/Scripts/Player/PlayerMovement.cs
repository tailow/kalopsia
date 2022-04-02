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
    public float slideSpeedBoost;
    public float slideDeceleration;
    public float FOVChangeSpeed;
    public float coyoteTime;

    public int extraJumpCount;
    public int defaultFOV;
    public int sprintingFOVIncrease;
    public int slidingFOVIncrease;

    float desiredSpeed;
    float desiredFOV;
    float desiredAcceleration;
    float lastJump;
    float lastSlide;
    float currentSpeed;
    float xRot;

    int jumpsLeft;

    bool isGrounded;
    bool isSliding;
    bool isCrouching;
    bool isSprinting;
    bool isWallRunning;

    Vector3 dir;
    Vector3 movement;

    Camera playerCamera;

    Rigidbody rigid;

    void Start()
    {
        isGrounded = true;

        playerCamera = Camera.main;

        rigid = gameObject.GetComponent<Rigidbody>();

        if (PlayerPrefs.GetInt("sensitivity") == 0) {
            sensitivity = 10;
        }

        else {
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
        if (currentSpeed > maxSpeed) {
            currentSpeed = maxSpeed;
        }

        // CROUCHING
        if (Input.GetButtonDown("Crouch")) {

            // SLIDING
            if (isSprinting) {
                isSliding = true;

                lastSlide = Time.time;

                currentSpeed += slideSpeedBoost;

                desiredSpeed = 0;
                desiredAcceleration = slideDeceleration;
                desiredFOV = defaultFOV + slidingFOVIncrease;
            }

            else {
                isCrouching = true;

                desiredSpeed = crouchSpeed;
                desiredFOV = defaultFOV;
                desiredAcceleration = acceleration;
            }

            // MOVE PLAYER DOWN
            gameObject.transform.localScale = new Vector3(1, 0.5f, 1);
            gameObject.transform.Translate(new Vector3(0, -0.5f, 0));
        }

        // STOP CROUCHING
        if (Input.GetButtonUp("Crouch")) {
            isSliding = false;

            isCrouching = false;

            // MOVE PLAYER UP
            gameObject.transform.Translate(new Vector3(0, 0.5f, 0));
            gameObject.transform.localScale = new Vector3(1, 1f, 1);
        }

        // STOP SPRINTING
        if (Input.GetButtonUp("Sprint")) {
            isSprinting = false;
        }

        // MOVEMENT
        if (Input.GetAxisRaw("Horizontal") != 0 || Input.GetAxisRaw("Vertical") != 0) {

            // AIR STRAFING
            if (!isGrounded) {
                currentSpeed += Mathf.Abs(Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime * strafeAcceleration);
            }

            // SPRINTING
            if (Input.GetButton("Sprint") && Input.GetAxisRaw("Vertical") > 0 && (currentSpeed < sprintSpeed || isGrounded) && !isSliding){
                isSprinting = true;

                if (!isSliding) {
                    desiredSpeed = sprintSpeed;
                    desiredFOV = defaultFOV + sprintingFOVIncrease;
                    desiredAcceleration = acceleration;
                }
            }

            // WALKING
            if (!isSprinting && !isCrouching && !isSliding && (currentSpeed < movementSpeed || isGrounded))
            {
                desiredSpeed = movementSpeed;
                desiredFOV = defaultFOV;
                desiredAcceleration = acceleration;
            }

            dir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        }

        // STOPPING
        else {
            desiredSpeed = 0;
            desiredFOV = defaultFOV;
            desiredAcceleration = acceleration;
        }

        // JUMPING
        if (Input.GetButtonDown("Jump") && Time.time - lastJump > jumpDelay) {
            if (isWallRunning || isGrounded)
            {
                isGrounded = false;
                isWallRunning = false;

                rigid.velocity = Vector3.zero;
                rigid.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            }

            else if (jumpsLeft > 0){
                isGrounded = false;
                isWallRunning = false;

                jumpsLeft -= 1;

                rigid.velocity = Vector3.zero;
                rigid.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            }

            lastJump = Time.time;
        }

        // SPEED AND FOV CALCULATION
        currentSpeed = Mathf.Lerp(currentSpeed, desiredSpeed, Time.deltaTime * desiredAcceleration);
        playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, desiredFOV, Time.deltaTime * FOVChangeSpeed);

        movement = dir.normalized * currentSpeed / 20;
    }

    void FixedUpdate() {
        rigid.MovePosition(rigid.position + transform.TransformDirection(movement) * Time.deltaTime);
    }

    void OnCollisionEnter(Collision coll) {
        if (coll.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;

            jumpsLeft = extraJumpCount;
        }

        else if (coll.gameObject.CompareTag("Wall"))
        {
            if (!isGrounded)
            {
                isWallRunning = true;
            }

            jumpsLeft = extraJumpCount;
        }
    }

    void OnCollisionExit(Collision coll) {
        if (coll.gameObject.CompareTag("Ground"))
        {
            Invoke("NotGrounded", coyoteTime);
        }

        else if (coll.gameObject.CompareTag("Wall"))
        {
            Invoke("NotWallRunning", coyoteTime);
        }
    }

    void NotGrounded(){
        isGrounded = false;
    }

    void NotWallRunning(){
        isWallRunning = false;
    }
}