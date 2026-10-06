using System;
using TMPro;
using UnityEngine;
using static Unity.VisualScripting.Member;

public class InvestigateState : BaseState
{
    Action OnFinishedInvestigation;
    float targetSpeed = 0;

    bool investigationFinished = false;

    Vector3 destination;
    public InvestigateState(AIController controller)
        : base(controller)
    {
        stateName = "Investigate";
    }

    public override void Enter()
    {
        Debug.Log("Entering investigate state");
        if (controller.blackboard.HasLastKnownPosition)
        {
            Debug.Log("Visual stimulus");
            controller.AIMovement.SetDestination(controller.blackboard.LastKnownPosition);
            destination = controller.blackboard.LastKnownPosition;
        }
        else if (controller.blackboard.HasLastHeardPosition)
        {
            Debug.Log("Audio stimulus");
            controller.AIMovement.SetDestination(controller.blackboard.LastHeardPosition);
            destination = controller.blackboard.LastHeardPosition;
        }
        else 
        {
            controller.AIMovement.SetDestination(controller.transform.position);
            destination = controller.transform.position;
            Debug.Log("No stimulus");
        }

        Debug.Log($"Investigation location {controller.blackboard.LastKnownPosition} OR {controller.blackboard.LastHeardPosition}");
        investigationFinished = false;

        
        AICommunicationEvent newEvent = new AICommunicationEvent();
        newEvent.Sender = controller;
        newEvent.Target = controller.blackboard.Target;
        newEvent.Type = AICommunicationType.Investigating;
        newEvent.Confidence = 0.5f;

        if (controller.blackboard.HasLastKnownPosition)
        {
            newEvent.Position = controller.blackboard.LastKnownPosition;
        }
        else if(controller.blackboard.HasLastHeardPosition)
        {
            newEvent.Position = controller.blackboard.LastHeardPosition;
        }
 
        newEvent.TimeStamp = Time.time;
        controller.SendCommunication(newEvent);
        
    }
    public override void Update()
    {
        if (investigationFinished) return;
        
        if(HasReachedDestination())
        {
            Debug.Log("Reached target");
            targetSpeed = 0;
            investigationFinished = true;
            OnFinishedInvestigation?.Invoke();
        }
        else
        {
            targetSpeed = controller.characterMotor.movementConfig.walkSpeed;
        }

        controller.AIMovement.Update(targetSpeed);
    }
    public override void Exit()
    {
        foreach (CommunicationChannelSO channel in controller.AccessibleChannels)
        {
            CommunicationChannel ch = new CommunicationChannel();

            if (controller.AccessibleChannels.Count > 0)
            {
                ch.channelData = controller.AccessibleChannels[0];
            }
            else { return; }

            AIChannelRequestEvent e = new AIChannelRequestEvent();
            e.channel = ch;
            e.action = ChannelAction.Release;
            e.requester = controller.detectableEntity;
            controller.SendChannelRequest(e);
        }
    }
    public override void SubscribeToEvent(StateMachine stateMachine)
    {
        OnFinishedInvestigation += stateMachine.InvestigationFinished;
    }
    public override void UnSubscribeToEvent(StateMachine stateMachine)
    {
        OnFinishedInvestigation -= stateMachine.InvestigationFinished;
    }

    bool HasReachedDestination()
    {
        float distance =
           Vector3.Distance(
               controller.transform.position,
               destination
           );

        if (distance <= controller.config.waypointThreshold)
        {
            return true;

        }
        else { return false; }
    }
}
