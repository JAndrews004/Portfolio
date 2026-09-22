using System;
using UnityEngine;

public class ChaseState : BaseState
{
    IDetectableEntity target;
    Action OnChaseEnded;
    float lostTargetTimer = 0;
    float totalChaseTime = 0;

    bool channelRequested = false;

    Vector3 targetPosition;
    public ChaseState(AIController controller)
        : base(controller)
    {
        stateName = "Chase";
    }
    public override void Enter() 
    {
        controller.blackboard.ChaseEnded = false;
        channelRequested = false;
        lostTargetTimer = 0;
        totalChaseTime = 0;

        if (controller.blackboard.Target != null)
        {
            target = controller.blackboard.Target;

            controller.AIMovement.SetDestination(target.GetPosition());
        }
        else
        {
            ChaseEnd();
            
        }
        
    }
    public override void Update() 
    {
        if(target == null)
        {
            ChaseEnd();
        }
        if (controller.aIPerception.GetLineOfSight(target))
        {
            targetPosition = target.GetPosition();
            controller.AIMovement.SetDestination(targetPosition);
            lostTargetTimer = 0;
        }
        else
        {
            if (controller.blackboard.HasLastKnownPosition)
            {
                targetPosition = controller.blackboard.LastKnownPosition;
            }
            else if(controller.blackboard.HasLastHeardPosition)
            {
                targetPosition = controller.blackboard.LastHeardPosition;
            }
            else
            {
                targetPosition = controller.transform.position;
            }
            controller.AIMovement.SetDestination(targetPosition);

            lostTargetTimer += Time.deltaTime;
            if (lostTargetTimer >= controller.config.timeUntilChaseEnds)
            {
                ChaseEnd();// enter seach state
                return;
            }
        }

        if (controller.AIMovement.HasReachedDestination())
        {
            controller.AIMovement.Update(0);
        }
        else
        {
            controller.AIMovement.Update(controller.characterMotor.movementConfig.sprintSpeed);
        }

        totalChaseTime += Time.deltaTime;
        // After chasing for x seconds request emergency channel
        if (totalChaseTime >= controller.config.chaseCommunicationTimer && channelRequested)
        {
            channelRequested = true;
            foreach (CommunicationChannelSO channel in controller.AccessibleChannels)
            {
                CommunicationChannel ch = new CommunicationChannel();

                ch.groupA = channel.groupA;
                ch.groupB = channel.groupB;
                ch.CommunicationRange = channel.CommunicationRange;

                AIChannelRequestEvent e = new AIChannelRequestEvent();
                e.channel = ch;
                e.action = ChannelAction.Request;
                e.requester = controller.detectableEntity;
                controller.SendChannelRequest(e);
            }
            

            AICommunicationEvent newEvent = new AICommunicationEvent();
            newEvent.Sender = controller;
            newEvent.Target = target;
            newEvent.Type = AICommunicationType.Emergency;
            newEvent.Confidence = 1f;
            newEvent.Position = targetPosition;
            newEvent.TimeStamp = Time.time;
            //request emergency channel
            controller.SendCommunication(newEvent);
        }
    }

    public override void Exit() 
    {
        foreach (CommunicationChannelSO channel in controller.AccessibleChannels)
        {
            CommunicationChannel ch = new CommunicationChannel();

            ch.groupA = channel.groupA;
            ch.groupB = channel.groupB;
            ch.CommunicationRange = channel.CommunicationRange;

            AIChannelRequestEvent e = new AIChannelRequestEvent();
            e.channel = ch;
            e.action = ChannelAction.Release;
            e.requester = controller.detectableEntity;
            controller.SendChannelRequest(e);
        }
    }

    public override void SubscribeToEvent(StateMachine stateMachine) 
    {
        OnChaseEnded += stateMachine.ChaseFinished;
    }
    public override void UnSubscribeToEvent(StateMachine stateMachine) 
    {
        OnChaseEnded -= stateMachine.ChaseFinished;
    }

    void ChaseEnd()
    {
        
        OnChaseEnded?.Invoke();
    }
}
