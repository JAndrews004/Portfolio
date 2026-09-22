using System.Collections.Generic;
using UnityEngine;


public class PatrolRoute : MonoBehaviour
{
    public List<Transform> patrolWayPoints;
    public bool loops;

    public void AddNewWaypoint()
    {
        if (patrolWayPoints.Count>0)
        {

            GameObject newWaypoint = new GameObject("Patrol point");

            newWaypoint.transform.position = patrolWayPoints[patrolWayPoints.Count-1].transform.position;
            newWaypoint.transform.SetParent(transform,true);
            patrolWayPoints.Add(newWaypoint.transform);

        }
        else
        {

            GameObject newWaypoint = new GameObject("Patrol point");
            newWaypoint.transform.position = this.transform.position;
            newWaypoint.transform.SetParent(transform, true);
            patrolWayPoints.Add(newWaypoint.transform);

        }

    }
    public void AddNewWaypoint(Vector3 position)
    {

        GameObject newWaypoint = new GameObject("Patrol point");
        newWaypoint.transform.position = position;
        newWaypoint.transform.SetParent(transform, true);
        patrolWayPoints.Add(newWaypoint.transform);


    }
    public void RemoveWaypoint(int i)
    {
        if(patrolWayPoints.Count > 0)
        {
            Transform waypoint = patrolWayPoints[i];

            patrolWayPoints.RemoveAt(i);

        }
    }
    public void ReverseRoute()
    {
        
        List<Transform> reversedPatrolWayPoints = new List<Transform>() { };

        for (int i = patrolWayPoints.Count-1; i >= 0; i--)
        {
            reversedPatrolWayPoints.Add(patrolWayPoints[i]);
        }

        patrolWayPoints = reversedPatrolWayPoints;

    }
    public void MoveWaypoint(int index, int dir)
    {
        if(dir == -1)
        {
            if(index != 0)
            {
                Transform x = patrolWayPoints[index];

                patrolWayPoints[index] = patrolWayPoints[index + dir];
                patrolWayPoints[index+dir] = x; 

            }
        }
        else
        {
            if (index != patrolWayPoints.Count-1)
            {
                Transform x = patrolWayPoints[index];

                patrolWayPoints[index] = patrolWayPoints[index + dir];
                patrolWayPoints[index + dir] = x;

            }
        }
    }
}
