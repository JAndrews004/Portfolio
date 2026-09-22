using UnityEditor;
using UnityEngine;
using UnityEngine.AI;


[CustomEditor(typeof(PatrolRoute))]
public class PatrolRouteEditor : Editor
{
    
    public override void OnInspectorGUI()
    {
        PatrolRoute route = target as PatrolRoute;

        if (route == null) return;

        EditorGUILayout.LabelField("Patrol Route");
        
        for(int i = 0; i < route.patrolWayPoints.Count; i++)
        {
            Transform point = route.patrolWayPoints[i];

            EditorGUILayout.LabelField($"Waypoint {i}");
            EditorGUILayout.BeginHorizontal();
            point.position = EditorGUILayout.Vector3Field("", point.position);

            if (GUILayout.Button("Remove"))
            {
                SetUndo(route, "Remove waypoint");
                route.RemoveWaypoint(i);
                Undo.DestroyObjectImmediate(point.gameObject);
                return;
            }
            if (GUILayout.Button("↑"))
            {
                SetUndo(route, "Move waypoint");
                route.MoveWaypoint(i,-1);
            }
            if (GUILayout.Button("↓"))
            {
                SetUndo(route, "Move waypoint");
                route.MoveWaypoint(i,+1);
            }

            EditorGUILayout.EndHorizontal();   
        }
        



        bool oldLoops = route.loops;
        bool newLoops = EditorGUILayout.Toggle("Loops ", route.loops);

        if (oldLoops != newLoops)
        {
            Undo.RecordObject(route, "Toggle Patrol Loop");
            route.loops = newLoops;
            EditorUtility.SetDirty(route);
        }

        if (GUILayout.Button("Add waypoint"))
        {
            SetUndo(route, "Add waypoint");
            route.AddNewWaypoint();
        }
        
        if (GUILayout.Button("Reverse route"))
        {
            SetUndo(route, "Reverse waypoints");
            route.ReverseRoute();
        }
    }

    private void OnSceneGUI()
    {
        PatrolRoute route = target as PatrolRoute;

        if (route == null) return;

        foreach (Transform point in route.patrolWayPoints)
        {
            Vector3 oldPos = point.position;
            Vector3 newPos = Handles.PositionHandle(point.position, Quaternion.identity);

            if (newPos != oldPos)
            {
                Undo.RecordObject(point, "Move Waypoint");
                point.position = newPos;
                EditorUtility.SetDirty(point);
            }

            Handles.color = UnityEngine.Color.red;

            if (Event.current.type == EventType.Repaint)
            {
                Handles.SphereHandleCap(0, point.position, Quaternion.identity, 0.5f, EventType.Repaint);
            }

            
        }
        
        for (int i = 0; i < route.patrolWayPoints.Count; i++)
        {
            Handles.color = UnityEngine.Color.green;
            Handles.Label(route.patrolWayPoints[i].position, $"{i}");
            Handles.color = UnityEngine.Color.yellow;
            if (i != route.patrolWayPoints.Count - 1)
            {
                Handles.DrawLine(route.patrolWayPoints[i].position, route.patrolWayPoints[i + 1].position, 0.05f);
            }
            else if (route.loops)
            {
                Handles.DrawLine(route.patrolWayPoints[route.patrolWayPoints.Count - 1].position, route.patrolWayPoints[0].position, 0.05f);
            }
            
        }

        if(Event.current.type == EventType.MouseDown && Event.current.button == 0) // left mouse click
        {
            if(Event.current.shift)//while shift is held
            {
                // physics ray cast to get position
                Vector2 mousePosition = Event.current.mousePosition;
                Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);

                if(Physics.Raycast(ray,out RaycastHit hit))
                {
                    Vector3 newWaypointPos = hit.point;
                    if (NavMesh.SamplePosition(newWaypointPos,out NavMeshHit navHit,1.0f, NavMesh.AllAreas))
                    {
                        SetUndo(route, "Add waypoint");
                        route.AddNewWaypoint(navHit.position);
                        Event.current.Use();
                    } 
                } 
            }
        }

    }

    void SetUndo(Object obj, string desc)
    {
        Undo.RecordObject(obj, desc);
        EditorUtility.SetDirty(this);
    }
}
