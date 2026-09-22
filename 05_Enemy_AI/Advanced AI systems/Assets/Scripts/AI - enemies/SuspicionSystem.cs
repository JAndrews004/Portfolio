using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SuspicionSystem
{
    float currentSuspicion = 0;
    public float maximumSuspicion { get;private set; } = 100f;

    AIController controller;

    Dictionary<IDetectableEntity,ObservedEntity> currentlySeenEntities = new Dictionary<IDetectableEntity, ObservedEntity>();

    SuspicionSource currentSuspicionSource;
    Dictionary<(IDetectableEntity,StimulusType),SuspicionSource> suspicionSources = new Dictionary<(IDetectableEntity, StimulusType), SuspicionSource>();
    bool currentlyAlerted = false;
    public SuspicionSystem( AIController controller)
    {
        this.controller = controller;

    }
    public void Update()
    {
        List<VisionResult> visionResults = controller.aIPerception.GetVisionResults();
        List<HearingResult> hearingResults = controller.aIPerception.GetCurrentHeaaringResults();
        UpdateObservedEntities(visionResults);

        //Debug.Log($"Vision results: {visionResults.Count}");

        List<Stimulus> visionStimuli = CreateVisionStimuli(visionResults);
        List<Stimulus> hearingStimuli = CreateHearingStimuli(hearingResults);

        //Debug.Log($"Vision stimuli: {visionStimuli.Count}");
        List<Stimulus> stimuli = new List<Stimulus>() { };

        foreach(Stimulus stimulus in visionStimuli)
        {
            stimuli.Add( stimulus );
        }
        foreach (Stimulus stimulus in hearingStimuli)
        {
            stimuli.Add(stimulus);
        }

        ProcessStimuli(stimuli);

        //Debug.Log($"Suspicion sources: {suspicionSources.Count}");

        if (visionStimuli.Count == 0 && hearingStimuli.Count == 0)
        {
            UpdateSuspicionDecay();
        }
        CheckCommunication();
        CheckSearchResults();

    }

    public void CheckSearchResults()
    {
        if (controller.blackboard.SearchStarted && controller.blackboard.SearchFinished)
        {
            if (controller.blackboard.TargetFound)
            {
                AddSuspicion(20);
            }
            else
            {
                ReduceSuspicion(40);
            }
        }
        controller.blackboard.SearchStarted = false;
        controller.blackboard.SearchFinished = false;
        controller.blackboard.TargetFound = false;
    }
    public void ReceiveCommunicationStimulus(Stimulus stimulus)
    {
        ProcessStimuli(new List<Stimulus> { stimulus});
    }
    void CheckCommunication()
    {
        if (IsAlerted() && !currentlyAlerted)
        {
            SuspicionSource source = GetPrimarySuspicionSource();
            AICommunicationEvent e = new AICommunicationEvent();
            e.Type = AICommunicationType.PlayerSpotted;
            e.Sender = controller;
            e.Position = source.position;
            e.Target = source.source;
            e.Confidence = source.confidence;
            e.TimeStamp = Time.time;

            controller.SendCommunication(e);
            currentlyAlerted = true;
        }
        else if (!IsAlerted())
        {
            currentlyAlerted = false;
        }
    }
    void AddSuspicion(float amount)
    {
        currentSuspicion += amount;

        currentSuspicion = Mathf.Clamp(currentSuspicion, 0, maximumSuspicion); 
    }
    void ReduceSuspicion(float amount)
    {
        currentSuspicion -= amount;

        currentSuspicion = Mathf.Clamp(currentSuspicion, 0, maximumSuspicion);
    }

    public float GetSuspicion()
    {
        return currentSuspicion;
    }

    public bool IsSuspicious()
    {
        
        return currentSuspicion >= maximumSuspicion *0.30f;
    }
    public bool IsInvestigating()
    {

        return currentSuspicion >= maximumSuspicion * 0.6f;
    }
    public bool IsAlerted() 
    { 
        return currentSuspicion >= maximumSuspicion * 0.9f; 
    }  
    void UpdateSuspicionDecay()
    {
        ReduceSuspicion(controller.config.suspicionDecreaseRate*Time.deltaTime);
    }
    void UpdateObservedEntities(List<VisionResult> visionResults)
    {
        List<IDetectableEntity> seenThisFrame = new List<IDetectableEntity>() { };
        foreach (VisionResult visionResult in visionResults)
        {
            seenThisFrame.Add(visionResult.Entity);
            if (currentlySeenEntities.ContainsKey(visionResult.Entity))
            {
                if (visionResult.HasLineOfSight)
                {
                    currentlySeenEntities.TryGetValue(visionResult.Entity, out ObservedEntity observedData);
                    observedData.lastKnownPosition = visionResult.ObservationPoint;
                    observedData.lastSeenTime = Time.time;
                    observedData.timeVisable = Time.time - observedData.firstSeenTime;

                    currentlySeenEntities[visionResult.Entity] = observedData;
                }
            }
            else
            {
                ObservedEntity newObservedData = new ObservedEntity();
                newObservedData.entity = visionResult.Entity;
                newObservedData.firstSeenTime = Time.time;
                newObservedData.isCurrentlyVisable = visionResult.HasLineOfSight;
                newObservedData.lastKnownPosition = visionResult.ObservationPoint;
                newObservedData.confidence = 1.0f;
                newObservedData.totalVisableTime = 0;

                currentlySeenEntities.Add(newObservedData.entity, newObservedData);
            }
        }

        //clean up
        foreach(KeyValuePair< IDetectableEntity,ObservedEntity> observedPair in currentlySeenEntities)
        {
            if (!observedPair.Value.isCurrentlyVisable)
            {
                currentlySeenEntities.TryGetValue(observedPair.Key, out ObservedEntity observedData);
                observedData.timeVisable = observedData.lastSeenTime - observedData.firstSeenTime;
                observedData.isCurrentlyVisable = false;
                observedData.totalVisableTime += observedData.timeVisable;
            }
            if (!seenThisFrame.Contains(observedPair.Key))
            {
                currentlySeenEntities.TryGetValue(observedPair.Key, out ObservedEntity observedData);
                observedData.timeVisable = observedData.lastSeenTime - observedData.firstSeenTime;
                observedData.isCurrentlyVisable = false;
                observedData.totalVisableTime += observedData.timeVisable;
            }
            
        }
    }
    List<Stimulus> CreateVisionStimuli(List<VisionResult> visionResults)
    {
        List<Stimulus> stimuli = new List<Stimulus>() { };
        foreach (VisionResult visionResult in visionResults)
        {
            Stimulus stimulus = new Stimulus();
            stimulus.position = visionResult.ObservationPoint;
            stimulus.type = StimulusType.Vision;
            stimulus.source = visionResult.Entity;

            stimulus.strength = visionResult.Region == VisionRegion.Direct ? 1 : visionResult.Region == VisionRegion.Peripheral ? 0.5f : 0;

            stimulus.confidence = visionResult.Region == VisionRegion.Direct ? 0.5f : visionResult.Region == VisionRegion.Peripheral ? 0.25f : 0;
            currentlySeenEntities.TryGetValue(visionResult.Entity, out ObservedEntity observedData);
            stimulus.source = visionResult.Entity;
            stimuli.Add(stimulus);
        }
        return stimuli;
    }
    List<Stimulus> CreateHearingStimuli(List<HearingResult> hearingResults)
    {
        List<Stimulus> stimuli = new List<Stimulus>() { };
        foreach (HearingResult hearingResult in hearingResults)
        {
            Stimulus stimulus = new Stimulus();
            stimulus.position = hearingResult.Noise.Position;
            stimulus.type = StimulusType.Hearing;
            stimulus.source = hearingResult.Noise.Instigator;
            stimulus.strength = hearingResult.Strength;
            
            if(stimulus.strength > 5f || hearingResult.Distance < 5.0f)
            {
                stimulus.confidence = 1.0f;
            }
            else
            {
                stimulus.confidence = 0.25f;
            }

            stimulus.source = hearingResult.Noise.Instigator;
            stimuli.Add(stimulus);
            //Debug.Log($"Created hearing stimulus with strength: {stimulus.strength}");
        }
        
        return stimuli;
    }

    void ProcessStimuli(List<Stimulus> stimuli)
    {
        foreach(Stimulus stimulus in stimuli)
        {
            float suspicionRate = GetSuspicionRate(stimulus);

            float suspicionChange = suspicionRate * stimulus.strength * stimulus.confidence * Time.deltaTime;

            if(stimulus.type == StimulusType.Vision)
            {
                currentlySeenEntities.TryGetValue(stimulus.source, out ObservedEntity observedData);
                if (observedData.entity != null)
                {
                    float time = observedData.timeVisable;

                    float value = controller.config.visionSuspicionCurve.Evaluate(time);

                    suspicionChange *= value;
                }

                if(stimulus.strength >= 0.95f)
                {
                    controller.blackboard.TargetFound = true;
                }

            }
            
            
            UpdateSuspicionSources(stimulus,suspicionChange);

            AddSuspicion(suspicionChange);
        }
    }

    float GetSuspicionRate(Stimulus stimulus)
    {
        if (stimulus.type == StimulusType.Vision)
        {
            return controller.config.visionSuspicionRate;
        }
        if(stimulus.type == StimulusType.Hearing)
        {
            return controller.config.hearingSuspicionRate;
        }


        return 0f;
    }
    void UpdateSuspicionSources(Stimulus stimulus,float contribution)
    {
        if (suspicionSources.ContainsKey((stimulus.source,stimulus.type)))
        {
            suspicionSources.TryGetValue((stimulus.source, stimulus.type), out SuspicionSource source);
            source.position = stimulus.position;
            source.strength = stimulus.strength;
            source.confidence = stimulus.confidence;
            source.currentSuspicionContribution += contribution;
            source.timeReceived = Time.time;

            suspicionSources[(stimulus.source, stimulus.type)] = source;
        }
        else
        {
            SuspicionSource source = new SuspicionSource();
            source.type = stimulus.type;
            source.position = stimulus.position;
            source.strength = stimulus.strength;
            source.source = stimulus.source;
            source.confidence = stimulus.confidence;
            source.currentSuspicionContribution = contribution;
            source.timeReceived = Time.time;

            suspicionSources.Add((stimulus.source, stimulus.type), source); 
        }

        List<(IDetectableEntity, StimulusType)> toRemove = new List<(IDetectableEntity, StimulusType)>() { };
        Dictionary<(IDetectableEntity, StimulusType), SuspicionSource> toUpdate = new Dictionary<(IDetectableEntity, StimulusType), SuspicionSource> { };
        foreach (KeyValuePair<(IDetectableEntity, StimulusType), SuspicionSource> suspicionSource in suspicionSources)
        {
            float timeSinceReceived = Time.time - suspicionSource.Value.timeReceived;
            if (suspicionSource.Value.currentSuspicionContribution == 0)
            {
                toRemove.Add(suspicionSource.Key);
            }
            suspicionSources.TryGetValue((stimulus.source, stimulus.type), out SuspicionSource source);
            if (source.type == StimulusType.Hearing)
            {
                if (timeSinceReceived > 4f)
                {
                    source.currentSuspicionContribution -= controller.config.suspicionDecreaseRate * Time.deltaTime;
                }
            }
            else if(source.type == StimulusType.Vision)
            {
                source.currentSuspicionContribution -= controller.config.suspicionDecreaseRate * Time.deltaTime;
            }
            else if (source.type == StimulusType.Communication)
            {
                if (timeSinceReceived > 8f)
                {
                    source.currentSuspicionContribution -= controller.config.suspicionDecreaseRate * Time.deltaTime;
                }
            }
            source.currentSuspicionContribution = Mathf.Clamp(source.currentSuspicionContribution, 0, maximumSuspicion);

            toUpdate.Add(suspicionSource.Key, source);

        }
        foreach((IDetectableEntity, StimulusType) entity in toRemove)
        {
            suspicionSources.Remove(entity);
            
        }
        foreach(KeyValuePair<(IDetectableEntity, StimulusType), SuspicionSource> suspicionSource in toUpdate)
        {
            suspicionSources[suspicionSource.Key] = suspicionSource.Value;
        }
    }
    public SuspicionSource GetPrimarySuspicionSource()
    {
        if (suspicionSources.Values.Count == 0) return new SuspicionSource();

        SuspicionSource currentSource = suspicionSources.Values.First();

        foreach(KeyValuePair<(IDetectableEntity, StimulusType), SuspicionSource> suspicionSource in suspicionSources)
        {
            if(suspicionSource.Value.type == StimulusType.Vision)
            {
                //Debug.Log($"{suspicionSource.Value.currentSuspicionContribution}, Vision");
            }
            else if (suspicionSource.Value.type == StimulusType.Hearing)
            {
                //Debug.Log($"{suspicionSource.Value.currentSuspicionContribution}, Hearing");
            }


            if (suspicionSource.Value.currentSuspicionContribution > currentSource.currentSuspicionContribution)
            {
                currentSource = suspicionSource.Value;
            }
        }

        return currentSource;
        
    }
}


public struct Stimulus
{
    public StimulusType type;
    public Vector3 position;
    public float strength;
    public IDetectableEntity source;
    public float confidence;
}
public struct SuspicionSource
{
    public StimulusType type;
    public Vector3 position;
    public float strength;
    public IDetectableEntity source;
    public float confidence;

    public float timeReceived;
    public float currentSuspicionContribution;
}
public class ObservedEntity
{
    public IDetectableEntity entity;
    public float firstSeenTime;
    public float lastSeenTime;
    public float timeVisable;
    public bool isCurrentlyVisable;
    public Vector3 lastKnownPosition;
    public float confidence;
    public float totalVisableTime;
}

public enum StimulusType
{
    Communication,
    Vision,
    Hearing,
    None,
}


