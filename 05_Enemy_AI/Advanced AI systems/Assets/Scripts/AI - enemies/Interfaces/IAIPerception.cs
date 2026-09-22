using System;
using System.Collections.Generic;
using UnityEngine;

public interface IAIPerception
{
    public void OnDestroy();
    public void Update();
    void DetectCandidates();
    bool DirectVisionTest(IDetectableEntity entity);
    bool PeripheralVisionTest(IDetectableEntity entity);
    bool CheckLineOfSight(IDetectableEntity entity);
    VisionResult CreateVisionResults(IDetectableEntity entity, VisionRegion region, float distance);
    void EvaluateCandidate(IDetectableEntity entity);
    float CalculateVerticalAngle(IDetectableEntity entity);
    float CalculateHorizontalAngle(IDetectableEntity entity);
    public List<VisionResult> GetVisionResults();
    public void HearNoise(Noise noise);
    public void RegisterHearingResult(Noise noise, float distance);
    public List<HearingResult> GetCurrentHeaaringResults();
    public bool GetLineOfSight(IDetectableEntity target);
}
