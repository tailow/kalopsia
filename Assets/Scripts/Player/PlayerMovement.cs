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
    public float airStrafeAcceleration;
    public float sensitivity;
    public float jumpDelay;
    public float slideSpeedBoost;
    public float slideSpeedBoostCooldown;
    public float slideDeceleration;
    public float FOVChangeSpeed;
    public float coyoteTime;
    public float wallRunTiltAmount;
    public float wallRunTiltSpeed;
    public float lastWallCooldown;

    [HideInInspector]
    public float lastWallContact;

    [HideInInspector]
    public float lastGroundContact;

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
    float wallRunTilt;
    float lastSlideBoost;
    float lastWallJump;

    int jumpsLeft;
    int wallLayerMask;

    bool isGrounded;
    bool isSliding;
    bool isCrouching;
    bool isSprinting;
    bool isWallRunning;

    Vector3 dir;
    Vector3 desiredDir;
    Vector3 movement;
    Vector3 wallDir;

    GameObject lastWall;
    GameObject currentWall;

    Camera playerCamera;

    Rigidbody rigid;

    void Start()
    {
        isGrounded = true;

        wallLayerMask = LayerMask.GetMask("Wall");

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

        transform.Rotate(new Vector3(0, Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime * 100, 0));

        // SPEED CAP
        if (currentSpeed > maxSpeed) {
            currentSpeed = maxSpeed;
        }

        // GROUND CHECK
        if (Time.time - lastGroundContact < coyoteTime){
            isGrounded = true;
            jumpsLeft = extraJumpCount;
        } else {
            isGrounded = false;
        }

        // WALL CHECK
        RaycastHit hit;

        if (Physics.Raycast(transform.position, transform.right, out hit, 0.8f, wallLayerMask)){
            wallDir = Vector3.right;

            currentWall = hit.collider.gameObject;

            lastWallContact = Time.time;
        } else if (Physics.Raycast(transform.position, -transform.right, out hit, 0.8f, wallLayerMask)) {
            wallDir = Vector3.left;

            currentWall = hit.collider.gameObject;

            lastWallContact = Time.time;
        } else {
            wallDir = Vector3.zero;
        }

        // WALL RUNNING
        if (Time.time - lastWallContact < coyoteTime && !isGrounded){

            // CHECK PREVIOUS WALL JUMP
            if (hit.collider && hit.collider.gameObject != lastWall || Time.time - lastWallJump > lastWallCooldown){
                isWallRunning = true;

                jumpsLeft = extraJumpCount;

                if (wallDir == Vector3.right){
                    wallRunTilt = Mathf.Lerp(wallRunTilt, wallRunTiltAmount, Time.deltaTime * wallRunTiltSpeed);
                }
                else if (wallDir == Vector3.left){
                    wallRunTilt = Mathf.Lerp(wallRunTilt, -wallRunTiltAmount, Time.deltaTime * wallRunTiltSpeed);
                }
            }
        } else {
            wallRunTilt = Mathf.Lerp(wallRunTilt, 0, Time.deltaTime * wallRunTiltSpeed);

            currentWall = null;

            isWallRunning = false;
        }

        playerCamera.transform.localRotation = Quaternion.Euler(-xRot, 0, wallRunTilt);

        // CROUCHING
        if (Input.GetButtonDown("Crouch")) {

            // SLIDING
            if (isSprinting && isGrounded) {
                isSliding = true;

                lastSlide = Time.time;

                if (Time.time - lastSlideBoost > slideSpeedBoostCooldown)
                {
                    currentSpeed += slideSpeedBoost;

                    lastSlideBoost = Time.time;
                }

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
                currentSpeed += Mathf.Abs(Input.GetAxisRaw("Mouse X") * sensitivity * Time.deltaTime * airStrafeAcceleration);
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
        }

        // STOPPING
        else {
            desiredSpeed = 0;
            desiredFOV = defaultFOV;
            desiredAcceleration = acceleration;
        }

        // JUMPING
        if (Input.GetButtonDown("Jump") && Time.time - lastJump > jumpDelay) {
            if (isGrounded)
            {
                isGrounded = false;

                rigid.velocity = Vector3.zero;
                rigid.AddForce(Vector3.up * jumpHeight, ForceMode.Impulse);
            }

            else if (isWallRunning)
            {
                isWallRunning = false;

                lastWall = currentWall;

                lastWallJump = Time.time;
                currentWall = null;

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

        desiredDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized;
        dir = Vector3.Lerp(dir, desiredDir, Time.deltaTime * acceleration);

        movement = dir * currentSpeed / 20;
    }

    void FixedUpdate() {
        rigid.MovePosition(rigid.position + transform.TransformDirection(movement) * Time.deltaTime);
    }
}