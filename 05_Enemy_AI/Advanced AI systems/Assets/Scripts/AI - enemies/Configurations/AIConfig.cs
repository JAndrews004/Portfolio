using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "AIConfig", menuName = "Configurations/AIConfig")]
public class AIConfig : ScriptableObject
{
    public float waypointThreshold = 0.2f;

    [Header("Perception settings")]
    [Header("Vision")]
    public float visionRange = 10.0f;
    public float horizontalFOV = 1.0f;
    public float verticalFOV = 1.0f;

    public float peripheralRange = 10.0f;
    public float peripheralHorizontalFOV = 1.5f;
    public float peripheralVerticalFOV = 1.5f;

    public LayerMask detectableLayers;
    public LayerMask obstacleLayers;

    [Header("Hearing")]
    public float hearingSensitivity = 1f;

    [Header("Suspicion system")]

    public AnimationCurve visionSuspicionCurve;
    public float suspicionIncreaseRate = 1f;
    public float suspicionDecreaseRate = 1f;
    public float visionSuspicionRate = 1f;
    public float hearingSuspicionRate = 1f;

    [Header("Search state")]
    public float maxSearchRadius = 10.0f;
    public float minimumSearchRadius = 5.0f;
    public float searchDuration = 2.5f;

    [Header("Chase state")]
    public float timeUntilChaseEnds = 3f;
    public float chaseCommunicationTimer = 5f;

}
