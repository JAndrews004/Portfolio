using UnityEngine;

public class AIDecision
{
    AIBlackboard blackboard;

    public AIDecision(AIBlackboard blackboard)
    {
        this.blackboard = blackboard;
    }


    public AIStateType GetDesiredState()
    {
        if (blackboard.InvestigationComplete)
        {
            return AIStateType.ReturnToPatrol;
        }

        switch (blackboard.AlertLevel)
        {
            case AlertLevel.Alerted:
                return AIStateType.Alerted;

            case AlertLevel.Investigating:
                return AIStateType.Investigate;

            case AlertLevel.Suspicious:
                return AIStateType.Suspicious;

            case AlertLevel.NeedsToReturnToPatrol:
                return AIStateType.ReturnToPatrol;

            case AlertLevel.Searching:
                return AIStateType.Search;

            case AlertLevel.Chasing:
                return AIStateType.Chase;

            default:
                return AIStateType.Patrol;
        }
        
    }

    public void InvestigationFinished()
    {
        blackboard.AlertLevel = AlertLevel.Searching;
        Debug.Log("Investigation Event triggered in AIDecision");
        blackboard.SearchStarted = true;
    }
    public void SearchFinished()
    {
        blackboard.AlertLevel = AlertLevel.NeedsToReturnToPatrol;
        blackboard.InvestigationComplete = true;
        blackboard.SearchFinished = true;
    }
    public void ReturnToPatrolFinished()
    {
        blackboard.AlertLevel = AlertLevel.Normal;
    }

    public void ChaseFinished()
    {
        blackboard.AlertLevel = AlertLevel.Searching;
        blackboard.ChaseEnded = true;
    }
}
