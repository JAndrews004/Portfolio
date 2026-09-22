using System.Collections.Generic;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class StateMachine 
{
    AIController controller;
    AIDecision decision;
    PatrolRoute patrolRoute;

    public IAIState CurrentState { get; private set; }
    IAIState IdleState;
    IAIState PatrolState;
    IAIState SuspiciousState;
    IAIState InvestigateState;
    IAIState ReturnToPatrolState;
    IAIState SearchState;
    IAIState ChaseState;


    public StateMachine(AIController controller, PatrolRoute patrolRoute)
    {
        this.controller = controller;
        IdleState = new IdleState(controller);
        PatrolState = new PatrolState(controller, patrolRoute);
        SuspiciousState = new SuspiciousState(controller);
        InvestigateState = new InvestigateState(controller);
        ReturnToPatrolState = new ReturnToPatrolState(controller, patrolRoute);
        SearchState = new SearchState(controller);
        ChaseState = new ChaseState(controller);

        decision = new AIDecision(controller.blackboard);

        this.patrolRoute = patrolRoute;

        ChangeState(IdleState);
        InvestigateState.SubscribeToEvent(this);
        ReturnToPatrolState.SubscribeToEvent(this);
        SearchState.SubscribeToEvent(this);
    }
    public void Update()
    {
        CurrentState?.Update();
        AIStateType desiredState = decision.GetDesiredState();

        IAIState newState = null;

        switch (desiredState)
        {
            case AIStateType.Idle:
                newState = IdleState;
                break;
            case AIStateType.Patrol:
                newState = PatrolState;
                break;
            case AIStateType.Suspicious:
                newState = SuspiciousState;
                break;
            case AIStateType.Investigate:
                newState = InvestigateState;
                break;
            case AIStateType.ReturnToPatrol:
                newState = ReturnToPatrolState;
                break;

            case AIStateType.Search:
                newState = SearchState;
                break;
            case AIStateType.Chase:
                newState = ChaseState;
                break;

            case AIStateType.Alerted:

                break;
        }

        ChangeState(newState);
    }

    public bool ChangeState(IAIState newState)
    {
        if(CurrentState == newState || newState == null) return false;

        CurrentState?.Exit();
        CurrentState = newState;
        CurrentState?.Enter();
        return true;

    }
    public void InvestigationFinished()
    {
        decision?.InvestigationFinished();
        Debug.Log("Investigation Event triggered");
    }
    public void ReturnToPatrolFinished()
    {
        decision?.ReturnToPatrolFinished();
        Debug.Log("Return to patrol Event triggered");
    }
    public void SeachingFinished()
    {
        decision?.SearchFinished();
        Debug.Log("IsSearching Event triggered");
    }
    public void ChaseFinished()
    {
        decision?.ChaseFinished();

    }
    public void UnSubscribeToAllEvents()
    {
        IdleState.UnSubscribeToEvent(this);
        PatrolState.UnSubscribeToEvent(this);
        SuspiciousState.UnSubscribeToEvent(this);
        InvestigateState.UnSubscribeToEvent(this);
        ReturnToPatrolState.UnSubscribeToEvent(this);
        SearchState.UnSubscribeToEvent(this);
        ChaseState.UnSubscribeToEvent(this);
    }
}

public enum AIStateType
{
    Idle,
    Patrol,
    Suspicious,
    Investigate,
    Search,
    Chase,
    ReturnToPatrol,
    Alerted
}