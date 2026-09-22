using UnityEngine;

[RequireComponent(typeof(Animator))]
public class CharacterAnimationController : MonoBehaviour
{
    private Animator animator;
    private CharacterMotor motor;

    public float blendSpeed = 10f;

    private int airLayer;
    private int upperLayer;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        motor = GetComponent<CharacterMotor>();

        airLayer = animator.GetLayerIndex("InAir");
        upperLayer = animator.GetLayerIndex("Upper");

        animator.applyRootMotion = motor.movementConfig.rootMotionEnabled;
    }

    private void Update()
    {
        animator.applyRootMotion = motor.movementConfig.rootMotionEnabled;

        float air =(!motor.IsGrounded && !motor.IsHanging && motor.currentState != MovementState.LedgeClimb) ? 1f : 0f;
        animator.SetLayerWeight(airLayer,
            Mathf.Lerp(animator.GetLayerWeight(airLayer), air, Time.deltaTime * blendSpeed));

        float upper = motor.IsGrounded ? 1f : 0f;
        animator.SetLayerWeight(upperLayer,
            Mathf.Lerp(animator.GetLayerWeight(upperLayer), upper, Time.deltaTime * blendSpeed));

        upper = motor.IsSliding || motor.IsHanging || motor.ClimbUp ? 0f : 1f;
        animator.SetLayerWeight(upperLayer,
            Mathf.Lerp(animator.GetLayerWeight(upperLayer), upper, Time.deltaTime * blendSpeed *10f));


        float speed = motor.DesiredSpeed / motor.movementConfig.sprintSpeed;

        animator.SetFloat("Speed", speed, motor.movementConfig.movementDampTime, Time.deltaTime);
        animator.SetFloat("y-vel", motor.velocity.y);

        animator.SetBool("IsGrounded", motor.IsGrounded);
        animator.SetBool("IsCrouching", motor.isCrouching);

        if (motor.hasJumped)
        {
            animator.SetTrigger("Jumping");
            motor.hasJumped = false;
        }


        float speedPercent = motor.DesiredSpeed / motor.movementConfig.sprintSpeed;

        Vector3 dir = motor.direction * speedPercent;
        dir.y = 0f;

        animator.SetFloat("DirectionX", dir.x);
        animator.SetFloat("DirectionZ", dir.z);

        animator.SetFloat(
            "TurnAngle",
            motor.TurnAngle
        );

        animator.SetBool(
            "IsTurning",
            motor.IsTurning
        );
        animator.SetBool("IsSliding", motor.IsSliding);

        if (motor.consumeHangTrigger)
        {
            animator.SetTrigger("HangTrigger");
            motor.consumeHangTrigger = false;
        }
        if (motor.consumeClimbTrigger)
        {
            animator.SetTrigger("ClimbUp");
            motor.consumeClimbTrigger = false;
        }
        if (motor.consumeHangDismountTrigger)
        {
            animator.SetTrigger("DismountHang");
            motor.consumeHangDismountTrigger = false;
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 delta = Vector3.zero;
        if (!motor.movementConfig.rootMotionEnabled)
            return;
        if (motor.IsSliding|| motor.IsHanging)
            return;

        if (motor.currentState == MovementState.LedgeClimb)
        {
            Vector3 animDelta = animator.deltaPosition;

            animDelta.y = 0f;

            delta =
                motor.GetClimbWarpedDelta(
                    animDelta
                );

            motor.ApplyRootMotion(delta);

            Quaternion targetRot =
            Quaternion.LookRotation(-motor.GetLedgeNormal());

            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRot,
                Time.deltaTime * 15f
            );

            return;
        }


        if (motor.movementConfig.rootMotionTranslation && motor.IsGrounded)
        {
            delta = animator.deltaPosition;
            delta *= motor.movementConfig.rootMotionMoveMultiplier;
        }
        else if(motor.movementConfig.rootMotionTranslation && !motor.IsGrounded)
        {
            delta = motor.GetAirVelocity() * Time.deltaTime;
        }

        if (motor.movementConfig.rootMotionRotation && !motor.IsSliding)
        {
            Quaternion rot = Quaternion.Slerp(
                Quaternion.identity,
                animator.deltaRotation,
                motor.movementConfig.rootMotionRotationMultiplier
            );

            transform.rotation *= rot;
        }

        

        delta.y = motor.velocity.y * Time.deltaTime;

        motor.ApplyRootMotion(delta);

        
    }
}