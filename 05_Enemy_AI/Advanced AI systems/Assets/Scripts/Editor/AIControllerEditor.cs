using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(AIController))]
public class AIControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AIController controller = target as AIController;

        if (controller == null) return;

        DrawDefaultInspector();

        DrawDebugger(controller);
        

    }

    void DrawDebugger(AIController controller)
    {
        if(controller.blackboard == null || controller.stateMachine == null || controller.AIMovement == null) return;
        EditorGUILayout.Space();
        EditorGUILayout.BeginFoldoutHeaderGroup(true,"AI DEBUG",EditorStyles.boldLabel);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("STATE", EditorStyles.boldLabel);

        EditorGUILayout.LabelField($"Current State: {controller.stateMachine.CurrentState.stateName}");

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("DECISION", EditorStyles.boldLabel);
        string alertLevel = "";
        switch (controller.blackboard.AlertLevel)
        {
            case AlertLevel.Normal:
                alertLevel = "Normal";
                break;
            case AlertLevel.Suspicious:
                alertLevel = "Suspicious";
                break;
            case AlertLevel.Investigating:
                alertLevel = "Investigating";
                break;
            case AlertLevel.Searching:
                alertLevel = "Searching";
                break;
            case AlertLevel.Chasing:
                alertLevel = "Chasing";
                break;
            case AlertLevel.NeedsToReturnToPatrol:
                alertLevel = "Needs To Return To Patrol";
                break;
            case AlertLevel.Alerted:
                alertLevel = "Alerted";
                break;

        }
        EditorGUILayout.LabelField($"Alert Level: {alertLevel}");

        EditorGUILayout.LabelField($"Suspicion: {controller.blackboard.Suspicion}");

        EditorGUILayout.LabelField($"Primary source: {controller.suspicionSystem.GetPrimarySuspicionSource().type}");



        EditorGUILayout.Space();
        EditorGUILayout.LabelField("PERCEPTION", EditorStyles.boldLabel);

        EditorGUILayout.LabelField("VISION", EditorStyles.boldLabel);


        EditorGUILayout.LabelField($"Candidates: {controller.aIPerception.GetVisionResults().Count}");
        if (controller.aIPerception.GetVisionResults().Count > 0)
        {
            EditorGUILayout.LabelField($"Line of sight: {controller.aIPerception.GetVisionResults()[0].HasLineOfSight}");
        }
        else
        {
            EditorGUILayout.LabelField($"Line of sight: false");
        }

        EditorGUILayout.LabelField("HEARING", EditorStyles.boldLabel);

        EditorGUILayout.LabelField($"Candidates: {controller.aIPerception.GetCurrentHeaaringResults().Count}");
        if (controller.aIPerception.GetCurrentHeaaringResults().Count > 0)
        {
            EditorGUILayout.LabelField($"Distance: {controller.aIPerception.GetCurrentHeaaringResults()[0].Distance}");
        }
        else
        {
            EditorGUILayout.LabelField($"Distance: N/a");
        }


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("BLACKBOARD", EditorStyles.boldLabel);

        if(controller.blackboard.Target != null)
        {
            EditorGUILayout.LabelField($"Target: {controller.blackboard.Target.Name}");
        }
        else
        {
            EditorGUILayout.LabelField($"Target: N/a");
        }


        EditorGUILayout.LabelField($"Last seen position: {controller.blackboard.LastKnownPosition}");
        EditorGUILayout.LabelField($"Last heard position: {controller.blackboard.LastHeardPosition}");


        EditorGUILayout.Space();
        EditorGUILayout.LabelField("MOVEMENT", EditorStyles.boldLabel);

        EditorGUILayout.LabelField($"Destination: {controller.AIMovement.GetDestination()}");
        EditorGUILayout.LabelField($"Has path: {controller.AIMovement.HasPath()}");
        EditorGUILayout.LabelField($"Remaining distance: {controller.AIMovement.GetRemainingDistance()}");


        EditorGUILayout.EndFoldoutHeaderGroup();
    }

}
