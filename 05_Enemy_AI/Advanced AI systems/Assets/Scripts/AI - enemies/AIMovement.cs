using UnityEditor.AdaptivePerformance.Editor;
using UnityEngine;
using UnityEngine.AI;

public class AIMovement : IAIMovement
{
    NavMeshAgent agent;
    public AIController controller;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public AIMovement(AIController controller, NavMeshAgent agent)
    {
        this.agent = agent;
        this.controller = controller;
        agent.updatePosition = false;
        agent.updateRotation = false;
    }


    public void Update(float speed)
    {
        agent.nextPosition = controller.transform.position;

        if (agent.pathPending)
            return;

        if (HasReachedDestination())
        {
            Stop();
            controller.SetMotorData(Vector3.zero, 0f, false);
            return;
        }

        if (!agent.hasPath)
        {
            //agent.CalculatePath();
            return;
        }
        //Vector3 dir = (agent.steeringTarget - controller.transform.position).normalized;
        Vector3 dir = agent.desiredVelocity.normalized;

        controller.SetMotorData(
            dir,
            agent.isStopped ? 0f : speed,
            false
        );
    }

    public void SetDestination(Vector3 destination)
    {
        agent.isStopped = false;
        agent.SetDestination(destination);
        
    }

    public bool HasReachedDestination()
    {

        if (agent.pathPending)
            return false;

        float distanceToDestination = Vector3.Distance(controller.transform.position, agent.destination);

        if(distanceToDestination<= agent.stoppingDistance)
        {
            return true;
        }

        return false;
    }

    public void Stop()
    {
        agent.isStopped = true;
    }

    public Vector3 CheckDestination(Vector3 destination, float maxdistance)
    {
        if (NavMesh.SamplePosition(destination, out NavMeshHit navMeshHit, maxdistance, NavMesh.AllAreas))
        {
            Vector3 validPoint = navMeshHit.position;
            return validPoint;
        }
        else return Vector3.zero;
    }

    public Vector3 GetDestination()
    {
        return agent.destination;
    }
    public bool HasPath()
    {
        return agent.pathPending;
    }
    public float GetRemainingDistance()
    {
        return agent.remainingDistance;
    }
}
