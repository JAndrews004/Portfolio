using UnityEngine;

public class SuspiciousState : BaseState
{
    public SuspiciousState(AIController controller)
        : base(controller)
    {
        stateName = "Suspicious";
    }

    public override void Enter()
    {
        //Debug.Log("Entering suspicious state");
        SuspicionSource source = controller.suspicionSystem.GetPrimarySuspicionSource();
        Vector3 direction = (source.source.GetObservationPoint() - controller.headBone.transform.position).normalized;
        controller.SetMotorData(direction, 0.0f, false);

        AICommunicationEvent newEvent = new AICommunicationEvent();
        newEvent.Sender = controller;
        newEvent.Target = controller.blackboard.Target;
        newEvent.Type = AICommunicationType.Suspicious;
        newEvent.Confidence = 0.25f;

        if (controller.blackboard.HasLastKnownPosition)
        {
            newEvent.Position = controller.blackboard.LastKnownPosition;
        }
        else if (controller.blackboard.HasLastHeardPosition)
        {
            newEvent.Position = controller.blackboard.LastHeardPosition;
        }

        newEvent.TimeStamp = Time.time;
        controller.SendCommunication(newEvent);
    }
    public override void Update()
    {
        // add ambiguity to direction based on the confidence of the guess and maybe a delay in them changing direction
        SuspicionSource source = controller.suspicionSystem.GetPrimarySuspicionSource();
        Vector3 direction = (source.source.GetObservationPoint() - controller.headBone.transform.position).normalized;
        controller.SetMotorData(direction, 0.0f, false);
        //looking logic e.g. head turning to look around no movement though
    }
    public override void Exit()
    {

    }
}
