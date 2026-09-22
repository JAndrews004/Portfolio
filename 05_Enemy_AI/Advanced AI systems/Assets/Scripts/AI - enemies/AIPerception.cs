using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using static UnityEngine.EventSystems.EventTrigger;

public class AIPerception : IAIPerception
{
    AIController controller;
    NoiseSystem noiseSystem;

    List<VisionResult> CurrentVisionResults = new List<VisionResult>() { };
    List<IDetectableEntity> Candidates = new List<IDetectableEntity>() { };
    List<HearingResult> CurrentHearingResults = new List<HearingResult>() { };
    List<HearingResult> pendingHearingResults = new List<HearingResult>() { };
    public AIPerception(AIController controller, NoiseSystem noiseSystem)
    {
        this.controller = controller;
        this.noiseSystem = noiseSystem;

        if(noiseSystem != null)
            noiseSystem.OnNoiseEmitted += HearNoise;
        else
        {
            Debug.LogError("NoiseSystem not found");
        }
    }
    
    public void OnDestroy()
    {
        if (noiseSystem != null)
            noiseSystem.OnNoiseEmitted -= HearNoise;
    }
    public void Update()
    {
        CurrentHearingResults = pendingHearingResults;
        CurrentVisionResults = new List<VisionResult>() { };
        pendingHearingResults = new List<HearingResult>() { };
        Candidates = new List<IDetectableEntity>() { };

        DetectCandidates();

        foreach (IDetectableEntity entity in Candidates)
        {
            EvaluateCandidate(entity);
        }

        
    }
    public bool GetLineOfSight(IDetectableEntity target)
    {
        foreach(VisionResult result in CurrentVisionResults)
        {
            if(result.HasLineOfSight && result.Entity == target)
            {
                return true;
            }
        }
        return false;
    }
    #region Vision
    public void DetectCandidates()
    {
        //spatial query to reduce vision checks

        Collider[] detected =Physics.OverlapSphere(controller.gameObject.transform.position, controller.config.visionRange, controller.config.detectableLayers);
        foreach(Collider c in detected)
        {
            IDetectableEntity entity = c.gameObject.GetComponent<DetectableEntity>();
            if (entity != null)
            {
                if (!Candidates.Contains<IDetectableEntity>(entity))
                {
                    Candidates.Add(entity);
                }
            }             
        }
    }

    public bool DirectVisionTest(IDetectableEntity entity)
    {
        float distance = Vector3.Distance(entity.GetObservationPoint(), controller.headBone.position);
        float VerticalAngle = CalculateVerticalAngle(entity);

        float HorizontalAngle = CalculateHorizontalAngle(entity);

        if (distance > controller.config.visionRange) return false;

        return IsInsideVisionEllipse(
            HorizontalAngle,
            VerticalAngle,
            controller.config.horizontalFOV,
            controller.config.verticalFOV
        );
    }

    public bool PeripheralVisionTest(IDetectableEntity entity)
    {
        

        float distance = Vector3.Distance(entity.GetObservationPoint(), controller.headBone.position);

        float VerticalAngle = CalculateVerticalAngle(entity);

        float HorizontalAngle = CalculateHorizontalAngle(entity);

        if (distance > controller.config.peripheralRange) return false;

        return IsInsideVisionEllipse(
            HorizontalAngle,
            VerticalAngle,
            controller.config.peripheralHorizontalFOV,
            controller.config.peripheralVerticalFOV
        );


    }
    private bool IsInsideVisionEllipse(float horizontalAngle, float verticalAngle, float horizontalFOV, float verticalFOV)
    {
        float horizontalNormalised =
            horizontalAngle / (horizontalFOV * 0.5f);

        float verticalNormalised =
            verticalAngle / (verticalFOV * 0.5f);

        float ellipseValue =
            horizontalNormalised * horizontalNormalised +
            verticalNormalised * verticalNormalised;

        return ellipseValue <= 1f;
    }

    public bool CheckLineOfSight(IDetectableEntity entity)
    {
        Vector3 direction = (entity.GetPosition() - controller.headBone.position);
        if (direction.magnitude <= 0.001f) return true;
        return !(Physics.Raycast(controller.headBone.position, direction.normalized, direction.magnitude, controller.config.obstacleLayers));
        
    }
    public VisionResult CreateVisionResults(IDetectableEntity entity, VisionRegion region, float distance)
    {
        VisionResult result = new VisionResult();
        //send back to controller to show gizmo for now of ray cast and vision cones maybe?

        result.Entity = entity;
        result.Region = region;
        result.Distance = distance;
        result.HorizontalAngle = CalculateHorizontalAngle(entity);
        result.VerticalAngle = CalculateVerticalAngle(entity);
        result.HasLineOfSight = CheckLineOfSight(entity);
        result.ObservationPoint = entity.GetObservationPoint();


        return result;
    }

    public float CalculateHorizontalAngle(IDetectableEntity entity)
    {
        Vector3 direction =
            entity.GetObservationPoint() -
            controller.headBone.position;

        direction.y = 0f;

        if (direction.sqrMagnitude < 0.001f)
            return 0f;

        direction.Normalize();

        Vector3 forward = controller.headBone.forward;
        forward.y = 0f;
        forward.Normalize();

        return Vector3.Angle(forward, direction);
    }

    public float CalculateVerticalAngle(IDetectableEntity entity)
    {
        Vector3 direction =
            entity.GetObservationPoint() -
            controller.headBone.position;

        if (direction.sqrMagnitude < 0.001f)
            return 0f;

        direction.Normalize();

        Vector3 horizontalDirection = direction;
        horizontalDirection.y = 0f;

        if (horizontalDirection.sqrMagnitude < 0.001f)
            return 90f;

        horizontalDirection.Normalize();

        float verticalAngle =
            Vector3.Angle(horizontalDirection, direction);

        return verticalAngle;
    }
    public void EvaluateCandidate(IDetectableEntity entity)
    {
        VisionRegion region;

        float distance = Vector3.Distance(entity.GetObservationPoint(), controller.headBone.position);

        if (!PeripheralVisionTest(entity)) return;
        if (DirectVisionTest(entity))
        {
            region = VisionRegion.Direct;
        }
        else region = VisionRegion.Peripheral;

        if(!CheckLineOfSight(entity)) return;

        VisionResult result = CreateVisionResults(entity,region,distance);

        CurrentVisionResults.Add(result);
    }

    public List<VisionResult> GetVisionResults()
    {
        return CurrentVisionResults;
    }
    #endregion Vision

    #region Hearing

    public void HearNoise(Noise noise)
    {
        if(noise.Instigator == controller.detectableEntity) return;
        float effectiveRange = noise.Strength * controller.config.hearingSensitivity;
        float distance = Vector3.Distance(noise.Position, controller.headBone.position);
        if (effectiveRange <= distance) return;

        RegisterHearingResult(noise, distance);
    }

    public void RegisterHearingResult(Noise noise, float distance)
    {
        HearingResult hearingResult = new HearingResult();
        hearingResult.Noise = noise;
        hearingResult.Distance = distance;
        hearingResult.Strength = noise.Strength;
        hearingResult.Direction = (noise.Position-controller.headBone.position).normalized;

        controller.lastHearingResult = hearingResult;
        pendingHearingResults.Add(hearingResult);   
    }
    public List<HearingResult> GetCurrentHeaaringResults()
    {
        return CurrentHearingResults;
    }

    #endregion Hearing
}

public struct VisionResult
{
    public IDetectableEntity Entity;

    public Vector3 ObservationPoint;
    public Vector3 Direction;

    public float Distance;
    public float HorizontalAngle;
    public float VerticalAngle;

    public VisionRegion Region;

    public bool HasLineOfSight;
}

public struct HearingResult
{

    public Noise Noise;
    public float Distance;
    public float Strength;
    public Vector3 Direction;

}
public enum VisionRegion
{
    None,
    Peripheral,
    Direct,
}