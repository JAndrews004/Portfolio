using System;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public class ReturnToPatrolState : BaseState
{
    PatrolRoute patrolRoute;
    Action OnFinishedReturnToPatrol;
    public ReturnToPatrolState(AIController controller, PatrolRoute patrolRoute)
        : base(controller)
    {
        this.patrolRoute = patrolRoute;
        stateName = "Return to patrol";
    }

    public override void Enter()
    {
        Debug.Log("Entering return to patrol state");
        //return to closest patrol point 
        controller.blackboard.InvestigationComplete = false;
        if (patrolRoute == null) return;

        Transform closestWaypoint = patrolRoute.patrolWayPoints[0];

        foreach (Transform t in patrolRoute.patrolWayPoints)
        {
            if(CalculateDistance(t) < CalculateDistance(closestWaypoint))
            {
                closestWaypoint = t;
            }
        }

        controller.AIMovement.SetDestination(closestWaypoint.position);
    }
    public override void Update()
    {
        if (!controller.AIMovement.HasReachedDestination())
        {
            controller.AIMovement.Update(controller.characterMotor.movementConfig.walkSpeed);

        }
        else
        {
            controller.AIMovement.Update(0);
            OnFinishedReturnToPatrol?.Invoke();


        }
    }
    public override void Exit()
    {

    }
    private float CalculateDistance(Transform pos)
    {
        float XDist = Mathf.Abs(controller.transform.position.x - pos.position.x);
        float ZDist = Mathf.Abs(controller.transform.position.z - pos.position.z);

        return Mathf.Sqrt(XDist * XDist + ZDist * ZDist);
    }
    public override void SubscribeToEvent(StateMachine stateMachine)
    {
        OnFinishedReturnToPatrol += stateMachine.ReturnToPatrolFinished;
    }
    public override void UnSubscribeToEvent(StateMachine stateMachine)
    {
        OnFinishedReturnToPatrol -= stateMachine.ReturnToPatrolFinished;
    }
}
