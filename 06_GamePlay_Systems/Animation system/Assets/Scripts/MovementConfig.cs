using UnityEngine;

[CreateAssetMenu(menuName = "Movement Configuration")]
public class MovementConfig : ScriptableObject
{

    [Header("Movement")]
    public float crouchSpeed = 1.5f;
    public float walkSpeed = 4f;
    public float sprintSpeed = 7f;
    public float rotationSpeed = 10f;

    [Header("Jump & Gravity")]
    public float gravity = -9.81f;
    public float jumpForce = 1.5f;

    [Header("Height changes")]
    public float standingHeight = 1.95f;
    public float standingCenter = 0.975f;
    public float crouchHeight = 1.4f;
    public float crouchCentre = 0.65f;
    public float heightChangeSpeed = 10f;

    [Header("Root motion")]
    public bool rootMotionEnabled = false;
    public bool rootMotionRotation = true;
    public bool rootMotionTranslation = true;

    public float rootMotionMoveMultiplier = 1f;
    public float rootMotionRotationMultiplier = 1f;

    [Header("Animation Dampening")]
    public float movementDampTime = 0.1f;
    public float rotationDampTime = 0.05f;

    [Header("Air Control")]
    public float airControlSpeed = 4f;
    public float airControlAcceleration = 6f;

    [Header("Slide")]
    public float slideInitialSpeed = 10f;
    public float slideDeceleration = 12f;
    public float slideMinSpeed = 2f;

    public float slideControl = 0.2f;

    public float slideDuration = 1.2f;

    [Header("Ledge Detection")]
    public LayerMask ledgeMask;

    public float ledgeCheckDistance = 0.5f;
    public float ledgeHeightOffset = 1.5f;
    public float ledgeForwardOffset = 0.3f;


}
