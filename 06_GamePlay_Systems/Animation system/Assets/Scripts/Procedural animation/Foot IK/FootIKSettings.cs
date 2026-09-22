using UnityEngine;

[CreateAssetMenu(menuName = "Configurations/Foot IK Configuration")]
public class FootIKSettings : ScriptableObject
{
    public LayerMask WalkableLayer;
    public float GroundOffset = 0.01f;
    public float RaycastDistance = 1.0f;
    public float PlantDistance = 0.03f;
    public float LiftDistance = 0.08f;
    public float VelocityThreshold = 0.1f;

    public float MaxHipOffset = 0.5f;
    public float HipRayDistance = 2f;
    public float HipOffset = 0.5f;

    public float MaxHipTilt = 8f;
    public float HipTiltMultiplier = 25f;

    [Header("Blend Speeds")]
    public float WeightBlendSpeed = 15f;
    public float HipHeightBlendSpeed = 5.0f;
    public float HipRotationBlendSpeed = 5.0f;
    public float FootPositionBlendSpeed = 5.0f;
    public float FootRotationBlendSpeed = 5.0f;

}
