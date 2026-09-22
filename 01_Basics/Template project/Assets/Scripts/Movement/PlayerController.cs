using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform cameraTransform;
    public CharacterMotor motor;

    private InputSystem_Actions input;

    private Vector2 move;
    private bool sprint;
    private bool jump;
    private bool crouch;

    private Vector3 smoothed;
    private Vector3 vel;

    public float smoothTime = 0.1f;
    public float speedLerp = 8f;
    public float rotationSpeed = 12f;

    private float currentSpeed;
    private bool crouchPressedThisFrame;

    private void Awake()
    {
        input = new InputSystem_Actions();
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        input.Enable();

        input.Player.Move.performed += ctx => move = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += _ => move = Vector2.zero;

        input.Player.Sprint.performed += _ => sprint = true;
        input.Player.Sprint.canceled += _ => sprint = false;

        input.Player.Jump.performed += _ => jump = true;

        input.Player.Crouch.performed += _ =>
        {
            crouch = true;
            crouchPressedThisFrame = true;
        };

        input.Player.Crouch.canceled += _ =>
        {
            crouch = false;
        };
    }

    private void OnDisable()
    {
        input.Disable();
    }

    private void Update()
    {
       
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;

        forward.y = 0;
        right.y = 0;

        forward.Normalize();
        right.Normalize();

        Vector3 worldDir = forward * move.y + right * move.x;

        // SMOOTH INPUT
        smoothed = Vector3.SmoothDamp(smoothed, worldDir, ref vel, smoothTime);

        // LOCAL SPACE FOR MOTOR
        Vector3 localDir = smoothed;

        

        float targetSpeed = motor.movementConfig.walkSpeed;

        if (sprint && crouchPressedThisFrame && motor.HasMovementInput && !motor.IsSliding)
        {
            motor.StartSlide(smoothed);
        }
        else if (crouch)
        {
            motor.SetCrouch(crouch);
        }
        else if (sprint)
            targetSpeed = motor.movementConfig.sprintSpeed;

        if(move.magnitude < 0.05f)
        {
            targetSpeed = 0f;
        }
        
        currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Time.deltaTime * speedLerp);

        if (!motor.IsSliding)
        {
            motor.SetCrouch(crouch);
        }
        motor.Tick(motor.transform.InverseTransformDirection(localDir), currentSpeed, jump);

        jump = false;

        Vector3 camForward = cameraTransform.forward;
        camForward.y = 0f;

        if (motor.HasMovementInput && !motor.IsTurning)
        {
            Quaternion targetRot =
                Quaternion.LookRotation(
                    motor.DesiredFacingDirection
                );

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                rotationSpeed * Time.deltaTime
            );
        }

        motor.SetFacingData(camForward);
        crouchPressedThisFrame = false;
    }
}