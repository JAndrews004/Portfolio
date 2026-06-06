using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class CharacterMotor : MonoBehaviour
{
    private CharacterController controller;

    [Header("Config")]
    public MovementConfig movementConfig;

    [HideInInspector] public bool isCrouching;
    [HideInInspector] public bool hasJumped;

    public Vector3 direction { get; private set; } // LOCAL SPACE
    public Vector3 velocity;

    public float DesiredSpeed { get; private set; }
    public float MoveInputMagnitude { get; private set; }
    public bool HasMovementInput { get; private set; }
    public Vector3 DesiredFacingDirection { get; private set; }
    public float TurnAngle { get; private set; }
    public bool IsTurning { get; private set; }
    public bool IsGrounded { get; private set; }
    public float ShimmyInput { get; private set; }
    private float edgePushTimer;
    public float edgeDropDelay = 0.35f;

    public MovementState currentState;

    private float targetHeight;
    private Vector3 targetCenter;
    private Vector3 airVelocity;

    [HideInInspector] public bool IsSliding { get; private set; }

    [HideInInspector] public float currentSlideSpeed;
    private float slideTimer;

    private Vector3 slideDirection;

    public Vector3 HorizontalVelocity =>
        new Vector3(controller.velocity.x, 0f, controller.velocity.z);


    public bool IsHanging { get; private set; }
    private Vector3 ledgePoint;
    private Vector3 ledgeNormal;

    public bool ClimbUp;
    public bool consumeClimbTrigger;
    public bool consumeHangTrigger;
    public bool consumeHangDismountTrigger;
    private float ledgeCooldown = 0.25f;

    private Vector3 climbStartPos;
    private Vector3 climbEndPos;

    private float climbTimer;

    private Vector3 climbWarpTarget;
    private float climbDuration = 1.13f;
    public AnimationCurve climbWarpStrength;

    public AnimationCurve climbVerticalCurve =
    AnimationCurve.EaseInOut(
        0f, 0f,
        0.56f, 1f
    );

    public AnimationCurve climbForwardCurve =
        AnimationCurve.EaseInOut(
            0.56f, 0f,
            1f, 1f
        );
    private void Awake()
    {
        controller = GetComponent<CharacterController>();

        targetHeight = movementConfig.standingHeight;
        targetCenter = new Vector3(0, targetHeight * 0.5f, 0);

        controller.height = targetHeight;
        controller.center = targetCenter;

        currentState = MovementState.Idle;
    }

    public void Tick(Vector3 localDirection, float speed, bool jumpRequested)
    {
        direction = localDirection;
        DesiredSpeed = speed;

        MoveInputMagnitude = new Vector2(localDirection.x, localDirection.z).magnitude;
        HasMovementInput = MoveInputMagnitude > 0.05f;

        hasJumped = false;

        velocity.y += movementConfig.gravity * Time.deltaTime;

        if (jumpRequested && IsGrounded)
        {
            hasJumped = true;
            velocity.y = Mathf.Sqrt(movementConfig.jumpForce * -2f * movementConfig.gravity);
        }

        if (IsGrounded && velocity.y < 0)
            velocity.y = -2f;

        if (IsHanging)
        {
            IsGrounded = true;
            if (jumpRequested)
            {
                StartClimb();
                return;
            }

            if (isCrouching)
            {
                DropFromLedge();
                return;
            }

            UpdateLedgeHang();

            return;
        }
        ledgeCooldown -= Time.deltaTime;


        if (IsSliding)
        {
            UpdateSlide();
        }
        else if (!movementConfig.rootMotionEnabled)
        {
            Vector3 move =
                new Vector3(localDirection.x, 0f, localDirection.z) * speed;

            controller.Move(
                transform.rotation *
                move *
                Time.deltaTime
            );
        }


        controller.Move(new Vector3(0, velocity.y, 0) * Time.deltaTime);

        controller.height = Mathf.Lerp(controller.height, targetHeight, movementConfig.heightChangeSpeed * Time.deltaTime);
        controller.center = Vector3.Lerp(controller.center, targetCenter, movementConfig.heightChangeSpeed * Time.deltaTime);

        IsGrounded = controller.isGrounded;

        if (currentState == MovementState.LedgeClimb)
        {
            UpdateClimb();

            return;
        }

        if (!IsGrounded)
        {
            CheckForLedge();
        }

        

        if (IsSliding)
        {
            currentState = MovementState.Sliding;
        }
        else if (!IsGrounded)
        {
            currentState = MovementState.InAir;
        }
        else if (!HasMovementInput)
        {
            currentState = MovementState.Idle;
        }
        else if (isCrouching)
        {
            currentState = MovementState.Crouching;
        }
        else
        {
            currentState = MovementState.Running;
        }

        

        if (!IsGrounded)
        {
            Vector3 desiredAirMove =
                transform.TransformDirection(direction) *
                movementConfig.airControlSpeed;

            airVelocity = Vector3.Lerp(
                airVelocity,
                desiredAirMove,
                movementConfig.airControlAcceleration * Time.deltaTime
            );
        }
        else
        {
            airVelocity = Vector3.zero;
        }

        Debug.DrawRay(ledgePoint,Vector3.up,Color.red);
        
    }
    public void StartClimb()
    {
        currentState = MovementState.LedgeClimb;
        IsHanging = false;
        ClimbUp = true;
        consumeClimbTrigger = true;

        climbTimer = 0f;

        climbStartPos = transform.position;

        climbWarpTarget =
            ledgePoint +
            Vector3.up * 0.05f -
            ledgeNormal * 0.55f;
    }
    private void UpdateClimb()
    {
        climbTimer += Time.deltaTime;

        if (climbTimer >= climbDuration)
        {
            FinishClimb();
        }

        
    }

    public Vector3 GetClimbWarpedDelta(Vector3 animationDelta)
    {
        float t =
            Mathf.Clamp01(
                climbTimer /
                climbDuration
            );

        Vector3 target =
            climbWarpTarget;

        Vector3 offset =
            target - climbStartPos;

        float vertical =
        climbVerticalCurve.Evaluate(
            Mathf.Clamp01(
                t * 0.515f
            )
        );

        float forward =
            climbForwardCurve.Evaluate(
                Mathf.Clamp01(
                    (t - 0.35f) / 0.65f
                )
            );

        Vector3 desiredPos =
            climbStartPos;

        desiredPos.y += offset.y * vertical;

        Vector3 horizontal =
            Vector3.ProjectOnPlane(
                offset,
                Vector3.up
            );

        desiredPos += horizontal * forward;

        Vector3 delta =
            desiredPos -
            transform.position;

        return delta;
    }
    private void CheckForLedge()
    {
        if (ledgeCooldown > 0f)
            return;

        Vector3 origin = transform.position + Vector3.up * movementConfig.ledgeHeightOffset;

        if (Physics.Raycast(origin, transform.forward, out RaycastHit wallHit, movementConfig.ledgeCheckDistance, movementConfig.ledgeMask))
        {
            Debug.DrawRay(
                wallHit.point,
                wallHit.normal * 3f,
                Color.yellow
            );
            Vector3 topCheck = wallHit.point + Vector3.up * 1.2f + transform.forward * 0.2f;
            if (Physics.Raycast(topCheck, Vector3.down, out RaycastHit topHit, 2f, movementConfig.ledgeMask))
            {
                StartLedgeHang(topHit.point, wallHit.normal);
            }
        }
    }
    private void DropFromLedge()
    {
        ShimmyInput = 0f;
        consumeHangDismountTrigger = true;
        IsHanging = false;

        currentState = MovementState.InAir;

        velocity.y = -1f;

        ledgeCooldown = 0.25f;
    }
    private void StartLedgeHang(Vector3 point,Vector3 normal)
    {
        IsHanging = true;
        consumeHangTrigger = true;

        currentState = MovementState.LedgeHang;

        velocity = Vector3.zero;

        ledgePoint = point;
        ledgeNormal = normal;
        
    }
    private void UpdateLedgeHang()
    {
        Vector3 hangPos =
            ledgePoint -
            ledgeNormal * 0.5f -
            Vector3.up * 1.2f;

        Vector3 delta =
            hangPos -
            transform.position;

        controller.Move(delta * 10f * Time.deltaTime);

        Quaternion targetRot =
            Quaternion.LookRotation(-ledgeNormal);

        transform.rotation =
            Quaternion.Slerp(
                transform.rotation,
                targetRot,
                15f * Time.deltaTime
            );
    }
    public void FinishClimb()
    {
        IsHanging = false;
        ClimbUp = false;

        velocity = Vector3.zero;

        currentState = MovementState.Idle;

        ledgeCooldown = 0.25f;
    }
    public Vector3 GetLedgeNormal()
    {
        return ledgeNormal;
    }
    public Vector3 GetAirVelocity()
    {
        return airVelocity;
    }

    public void SetCrouch(bool crouch)
    {
        isCrouching = crouch;

        if (crouch)
        {
            targetHeight = movementConfig.crouchHeight;
            targetCenter = new Vector3(0, movementConfig.crouchCentre, 0);
        }
        else
        {
            targetHeight = movementConfig.standingHeight;
            targetCenter = new Vector3(0, movementConfig.standingCenter, 0);
        }
    }

    public float GetSpeed()
    {
        return DesiredSpeed;
    }

    public Vector3 GetDirection()
    {
        return direction;
    }

    public void ApplyRootMotion(Vector3 motion)
    {
        controller.Move(motion);
    }

    public void SetFacingData(Vector3 desiredFacing)
    {
        desiredFacing.y = 0f;

        if (desiredFacing.sqrMagnitude < 0.001f)
            return;

        desiredFacing.Normalize();

        DesiredFacingDirection = desiredFacing;

        TurnAngle = Vector3.SignedAngle(
            transform.forward,
            desiredFacing,
            Vector3.up
        );

        IsTurning = Mathf.Abs(TurnAngle) > 100f;
    }

    public void StartSlide(Vector3 dir)
    {
        IsSliding = true;

        slideDirection = dir;
        slideDirection.y = 0f;
        slideDirection.Normalize();

        currentSlideSpeed = movementConfig.slideInitialSpeed;
    }

    private void UpdateSlide()
    {
        currentSlideSpeed = Mathf.MoveTowards(
            currentSlideSpeed,
            0f,
            movementConfig.slideDeceleration * Time.deltaTime
        );

        controller.Move(
            slideDirection *
            currentSlideSpeed *
            Time.deltaTime
        );

        if (currentSlideSpeed <= movementConfig.crouchSpeed)
        {
            EndSlide();
        }
    }
    public void EndSlide()
    {
        IsSliding = false;
    }
}


public enum MovementState
{
    Idle,
    Running,
    Sliding,
    Crouching,
    InAir,
    LedgeHang,
    LedgeClimb
}