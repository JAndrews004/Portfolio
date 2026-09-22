using System.Collections.Generic;
using UnityEngine;

public class PatrolState : BaseState
{
    PatrolRoute patrolRoute;
    int waypointIndex = 0;
    Transform CurrentWaypoint;
    Transform CurrentLocation;
    int direction = 1;
    public PatrolState(AIController controller, PatrolRoute patrolWayPoints)
        : base(controller)
    {
        this.patrolRoute = patrolWayPoints;
        stateName = "Patrol";
    }
    public override void Enter()
    {
        Debug.Log("Entering patrol state");
        CurrentWaypoint = patrolRoute.patrolWayPoints[waypointIndex];
        CurrentLocation = controller.gameObject.transform;
    }
    public override void Update()
    {
        CurrentLocation = controller.gameObject.transform;
        if (CurrentWaypoint == null)
        {
            CurrentWaypoint = patrolRoute.patrolWayPoints[0];
        }
        //check if near waypoint and change to next if needed
        CheckWaypointVicinity();
        controller.AIMovement.SetDestination(CurrentWaypoint.position);

        //pass data to motor
        controller.AIMovement.Update(controller.characterMotor.movementConfig.walkSpeed);


    }
    public override void Exit() { }

    private void CheckWaypointVicinity()
    {
        float distance =
            Vector3.Distance(
                CurrentLocation.position,
                CurrentWaypoint.position
            );

        if (distance <= controller.config.waypointThreshold)
        {
            if (patrolRoute.loops)
            {
                waypointIndex++;

                if (waypointIndex >= patrolRoute.patrolWayPoints.Count)
                {
                    waypointIndex = 0;
                }

                CurrentWaypoint = patrolRoute.patrolWayPoints[waypointIndex];
            }
            else
            {
                // if at end switch direction

                waypointIndex += direction;

                if (waypointIndex >= patrolRoute.patrolWayPoints.Count)
                {
                    direction = -1;
                    waypointIndex = patrolRoute.patrolWayPoints.Count - 2;
                }
                else if(waypointIndex < 0)
                {
                    direction = 1;
                    waypointIndex = 1;
                }

                CurrentWaypoint = patrolRoute.patrolWayPoints[waypointIndex];

            }
   
        }
    }

}
