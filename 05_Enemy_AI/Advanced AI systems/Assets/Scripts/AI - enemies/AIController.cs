using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.AI;

public class AIController : MonoBehaviour
{
    [Header("Congig")]
    public AIConfig config;

    public IAIMovement AIMovement;
    public StateMachine stateMachine { get; private set; }

    [Header("Chatacter movement")]
    public CharacterMotor characterMotor;
    public PatrolRoute patrolRoute;
    [HideInInspector] public IDetectableEntity detectableEntity;

    [Header("Perception")]
    public Transform headBone;
    public IAIPerception aIPerception;


    public NoiseSystem noiseSystem;

    public HearingResult lastHearingResult;
    public SuspicionSystem suspicionSystem;
    public AIBlackboard blackboard;

    [Header("AI communication")]
    public CommunicationGroup communicationGroup;
    public AICommunicationReceiver aICommunicationReceiver;
    public List<CommunicationChannelSO> AccessibleChannels;
    public event Action<AICommunicationEvent> OnCommunicaitonSend;
    public event Action<AIChannelRequestEvent> OnChannelRequest;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        detectableEntity = GetComponent<IDetectableEntity>();
        aICommunicationReceiver = new AICommunicationReceiver(this);
        blackboard = new AIBlackboard(this);
        stateMachine = new StateMachine(this, patrolRoute);
        AIMovement = new AIMovement(this, GetComponent<NavMeshAgent>());
        noiseSystem = FindFirstObjectByType<NoiseSystem>();
        aIPerception = new AIPerception(this,noiseSystem);
        suspicionSystem = new SuspicionSystem(this);
        AddTocommunicationGroup();
    }
    private void OnDisable()
    {
        aIPerception.OnDestroy();
        stateMachine.UnSubscribeToAllEvents();
        AICommunicationSystem aICommunicationSystem = FindFirstObjectByType<AICommunicationSystem>();
        if (aICommunicationSystem != null)
        {
            aICommunicationSystem.Unsubscribe(this);
        }
        
    }
    // Update is called once per frame
    void Update()
    {
        
        aIPerception?.Update();
        suspicionSystem?.Update();
        blackboard?.Update();
        stateMachine?.Update();

        //Debug.Log($"Suspicion: {suspicionSystem.GetSuspicion()}");
    }
    public void SetMotorData(Vector3 dir, float speed, bool jump)
    {
        characterMotor.SetFacingData(dir);
        if (characterMotor.IsTurning)
        {
            Vector3 localDir = transform.InverseTransformDirection(dir);
            characterMotor.Tick(localDir, 0.0f, false);
        }
        else
        {
            Vector3 localDir = transform.InverseTransformDirection(dir);
            characterMotor.Tick(localDir, speed, jump);
        }

        
    }
    public void AddTocommunicationGroup()
    {
        AICommunicationSystem aICommunicationSystem = FindFirstObjectByType<AICommunicationSystem>();
        aICommunicationSystem.AddToGroups(this);
    }
    public void SendCommunication(AICommunicationEvent e)
    {
        if(e == null)
        {
            return;
        }
        OnCommunicaitonSend.Invoke(e);
    }
    public void SendChannelRequest(AIChannelRequestEvent e)
    {
        if (e == null)
        {
            return;
        }
        OnChannelRequest.Invoke(e);
    }
    private void OnDrawGizmosSelected()
    {
        if (config == null || headBone == null)
            return;

        DrawVisionVolume(
            headBone.position,
            headBone.forward,
            config.visionRange,
            config.horizontalFOV,
            config.verticalFOV
        );

        DrawPeripheralVolume(
            headBone.position,
            headBone.forward,
            config.peripheralRange,
            config.peripheralHorizontalFOV,
            config.peripheralVerticalFOV
        );

        if (aIPerception == null)
            return;

        foreach (VisionResult result in aIPerception.GetVisionResults())
        {
            DrawVisionResult(result);
        }

        if (!Application.isPlaying)
            return;

        HearingResult hearingResult = lastHearingResult;

        // Noise position
        Vector3 noisePosition = hearingResult.Noise.Position;

        // Draw noise location
        Gizmos.DrawSphere(noisePosition, 0.15f);

        // Draw direction from AI to noise
        Gizmos.DrawLine(
            headBone.position,
            noisePosition
        );

        // Draw the direction stored in the HearingResult
        Gizmos.DrawRay(
            headBone.position,
            hearingResult.Direction * hearingResult.Distance
        );
    }


    private void DrawVisionVolume(
    Vector3 origin,
    Vector3 forward,
    float range,
    float horizontalFOV,
    float verticalFOV)
    {
        const int horizontalSegments = 20;
        const int verticalSegments = 10;

        float horizontalHalfFOV = horizontalFOV * 0.5f;
        float verticalHalfFOV = verticalFOV * 0.5f;

        Vector3 right = headBone.right;
        Vector3 up = headBone.up;

        Vector3[,] points =
            new Vector3[
                verticalSegments + 1,
                horizontalSegments + 1
            ];

        for (int y = 0; y <= verticalSegments; y++)
        {
            float verticalT =
                y / (float)verticalSegments;

            float verticalAngle =
                Mathf.Lerp(
                    -verticalHalfFOV,
                    verticalHalfFOV,
                    verticalT
                );

            for (int x = 0; x <= horizontalSegments; x++)
            {
                float horizontalT =
                    x / (float)horizontalSegments;

                float horizontalAngle =
                    Mathf.Lerp(
                        -horizontalHalfFOV,
                        horizontalHalfFOV,
                        horizontalT
                    );

                Vector3 direction =
                    Quaternion.AngleAxis(
                        horizontalAngle,
                        up
                    ) * forward;

                direction =
                    Quaternion.AngleAxis(
                        -verticalAngle,
                        right
                    ) * direction;

                points[y, x] =
                    origin +
                    direction.normalized *
                    range;
            }
        }

        for (int y = 0; y <= verticalSegments; y++)
        {
            for (int x = 0; x < horizontalSegments; x++)
            {
                Gizmos.DrawLine(
                    points[y, x],
                    points[y, x + 1]
                );
            }
        }

        for (int x = 0; x <= horizontalSegments; x++)
        {
            for (int y = 0; y < verticalSegments; y++)
            {
                Gizmos.DrawLine(
                    points[y, x],
                    points[y + 1, x]
                );
            }
        }
    }
    private void DrawPeripheralVolume(
    Vector3 origin,
    Vector3 forward,
    float range,
    float horizontalFOV,
    float verticalFOV)
    {
        Gizmos.color = new Color(1f, 1f, 0f, 0.5f);

        DrawVisionVolume(
            origin,
            forward,
            range,
            horizontalFOV,
            verticalFOV
        );
    }
    private void DrawDirectVolume(
    Vector3 origin,
    Vector3 forward,
    float range,
    float horizontalFOV,
    float verticalFOV)
    {
        Gizmos.color = new Color(1f, 0f, 0f, 0.5f);

        DrawVisionVolume(
            origin,
            forward,
            range,
            horizontalFOV,
            verticalFOV
        );
    }
    private void DrawVisionResult(VisionResult result)
    {
        if (result.Entity == null)
            return;

        if (result.Region == VisionRegion.Direct)
            Gizmos.color = Color.red;

        else if (result.Region == VisionRegion.Peripheral)
            Gizmos.color = Color.yellow;

        else
            Gizmos.color = Color.white;

        Vector3 target =
            result.ObservationPoint;

        Gizmos.DrawLine(
            headBone.position,
            target
        );

        Gizmos.DrawWireSphere(
            target,
            0.1f
        );
    }
}
