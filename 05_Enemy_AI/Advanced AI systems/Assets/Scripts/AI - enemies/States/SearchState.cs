using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;
using static UnityEngine.UI.Image;

public class SearchState : BaseState
{
    float maxSearchRadius;
    float minimumSearchRadius;
    List<Vector3> waypoints = new List<Vector3> { };
    int waypointIndex = 0;
    Vector3 CurrentWaypoint;
    Vector3 CurrentLocation;
    float searchTimer = 0;
    float searchDuration;
    bool isSearching = false;

    float currentMoveSpeed = 0;

    Action OnFinishedSearch;
    public SearchState(AIController controller)
        : base(controller)
    {
        this.maxSearchRadius = controller.config.maxSearchRadius;
        this.searchDuration = controller.config.searchDuration;
        this.minimumSearchRadius = controller.config.minimumSearchRadius;
        stateName = "Search";
    }

    public override void Enter() 
    {
        waypoints = new List<Vector3> { };
        Debug.Log("Entering search state");
        //populate waypoints[] with 4-6 positions
        Vector3 searchOrigin;

        if (controller.blackboard.HasLastKnownPosition)
        {
            searchOrigin = controller.blackboard.LastKnownPosition;
        }
        else if (controller.blackboard.HasLastHeardPosition)
        {
            searchOrigin = controller.blackboard.LastHeardPosition;
        }
        else
        {
            searchOrigin = controller.transform.position;
        }

        int numberOfWaypoints = UnityEngine.Random.RandomRange(4, 7);
        float minimumAngle = 45;
        int maxTrys = 40;
        while (waypoints.Count < numberOfWaypoints && maxTrys > 0)
        {
            maxTrys--;
            Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * UnityEngine.Random.RandomRange(minimumSearchRadius,maxSearchRadius+1);

            Vector3 randomPoint = searchOrigin + randomDirection;

            randomPoint.y = searchOrigin.y;

            Vector3 direction = randomPoint - searchOrigin;
            direction.y = 0;
            direction.Normalize();

            bool valid = true;

            foreach(Vector3 waypoint in waypoints)
            {
                Vector3 existingDirection = waypoint - searchOrigin;
                existingDirection.y = 0;
                existingDirection.Normalize();

                float angle = Vector3.Angle(existingDirection, direction);

                if(angle < minimumAngle)
                {
                    valid = false;
                    break;
                }

               
            }
            if (valid)
            {
                Vector3 newDestination = controller.AIMovement.CheckDestination(randomPoint, 2.0f);
                if (newDestination != Vector3.zero)
                {
                    waypoints.Add(newDestination);
                }
            }

        }
        if(waypoints.Count == 0)
        {
            waypoints.Add(searchOrigin);
        }
        currentMoveSpeed = controller.characterMotor.movementConfig.walkSpeed;

        Debug.Log($"Number of search positions: {waypoints.Count}");

        AICommunicationEvent newEvent = new AICommunicationEvent();
        newEvent.Sender = controller;
        newEvent.Target = controller.blackboard.Target;
        newEvent.Type = AICommunicationType.Suspicious;
        newEvent.Confidence = 0.5f;

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
        CurrentLocation = controller.gameObject.transform.position;
        if (CurrentWaypoint == Vector3.zero)
        {
            CurrentWaypoint = waypoints[0];
        }
        //check if near waypoint and change to next if needed
        CheckWaypointVicinity();
        controller.AIMovement.SetDestination(CurrentWaypoint);

        //pass data to motor
        controller.AIMovement.Update(currentMoveSpeed);

    }
    public override void Exit() { }
    public override void SubscribeToEvent(StateMachine stateMachine) 
    {
        OnFinishedSearch += stateMachine.SeachingFinished;
    }
    public override void UnSubscribeToEvent(StateMachine stateMachine) 
    {
        OnFinishedSearch -= stateMachine.SeachingFinished;
    }

    private void CheckWaypointVicinity()
    {
        float distance =
            Vector3.Distance(
                CurrentLocation,
                CurrentWaypoint
            );

        if (!isSearching && distance <= controller.config.waypointThreshold)
        {
            //wait and look around for 2-3 seconds then move to next waypoint
            isSearching = true;
            searchTimer = 0;

            currentMoveSpeed = 0;
            Debug.Log("Starting search");
        }
        if (isSearching)
        {
            searchTimer += Time.deltaTime;

            if(searchTimer >= searchDuration)
            {
                isSearching = false;
                searchTimer = 0;
                waypointIndex++;

                if(waypointIndex < waypoints.Count)
                {
                    CurrentWaypoint = waypoints[waypointIndex];
                    Debug.Log("Moving to next search point");
                }
                else
                {
                    OnFinishedSearch?.Invoke();
                    Debug.Log("Finishing search");
                }
            }
        }
        else
        {
            currentMoveSpeed = controller.characterMotor.movementConfig.walkSpeed;
        }
        
    }
}
