using UnityEditor.Experimental.GraphView;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class AIBlackboard 
{
    AIController controller;
    public IDetectableEntity Target;

    public Vector3 LastKnownPosition;
    public Vector3 LastHeardPosition;
    public Vector3 CurrentDestination;

    public float Suspicion;
    public AlertLevel AlertLevel = AlertLevel.Normal;

    public bool HasTarget;
    public bool HasLastKnownPosition;
    public bool HasLastHeardPosition;
    public bool InvestigationComplete = false;
    public bool SearchStarted = false;
    public bool SearchFinished = false; 
    public bool TargetFound = false;
    public bool ChaseEnded = false;

    public SuspicionSource primarySuspicionSource;

    public AIBlackboard(AIController controller)
    {
        this.controller = controller;
    }
    public void Update()
    {
        HasLastKnownPosition = false;
        HasLastHeardPosition = false;
        Suspicion = controller.suspicionSystem.GetSuspicion();

        if(Suspicion >= controller.suspicionSystem.maximumSuspicion * 0.90f && !ChaseEnded)
        {
            AlertLevel = AlertLevel.Chasing;
        }
        else if(AlertLevel == AlertLevel.Searching)
        {
            AlertLevel = AlertLevel.Searching;
        }
        else if (Suspicion < controller.suspicionSystem.maximumSuspicion * 0.30f)
        {
            AlertLevel = AlertLevel.Normal;
        }
        else if (Suspicion < controller.suspicionSystem.maximumSuspicion * 0.60f )
        {
            AlertLevel = AlertLevel.Suspicious;
        }
        else if (Suspicion < controller.suspicionSystem.maximumSuspicion * 0.80f)
        {
            AlertLevel = AlertLevel.Investigating;
        }
        else
        {
            AlertLevel = AlertLevel.Alerted;
        }



        primarySuspicionSource = controller.suspicionSystem.GetPrimarySuspicionSource();

        if (primarySuspicionSource.source != null)
        {
            if(primarySuspicionSource.type == StimulusType.Vision)
            {
                LastKnownPosition = primarySuspicionSource.position;
                HasLastKnownPosition = true;
            }
            else if(primarySuspicionSource.type == StimulusType.Hearing)
            {
                LastHeardPosition = primarySuspicionSource.position;
                HasLastHeardPosition = true;
            }
        }     
    }

}

public enum AlertLevel
{
    Normal,
    Suspicious,
    Investigating,
    Searching,
    Chasing,
    Alerted,
    NeedsToReturnToPatrol,
}
