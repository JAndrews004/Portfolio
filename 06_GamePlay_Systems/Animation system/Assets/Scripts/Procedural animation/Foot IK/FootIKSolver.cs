using UnityEngine;
using UnityEngine.Animations.Rigging;

public class FootIKSolver
{
    private Transform RayOrigin;
    private Transform FootTarget;
    private Transform FootGroundPoint;

    private TwoBoneIKConstraint Constraint;

    private float CurrentWeight;

    private FootState State = FootState.Lifted;

    private Quaternion InitialTargetRotation;
    private Vector3 InitialTargetPosition;


    private Vector3 LockedWorldPosition;
    private Quaternion LockedWorldRotation;
    private Vector3 CurrentGroundPosition;

    private Vector3 CurrentIKPosition;
    private Quaternion CurrentIKRotation;

    private FootIKSettings IKSettings;
    public FootIKSolver(Transform rayOrigin, Transform footTarget, FootIKSettings settings, TwoBoneIKConstraint constraint, Transform groundCheck)
    {
        RayOrigin = rayOrigin;
        FootTarget = footTarget;
        FootGroundPoint = groundCheck;

        Constraint = constraint;

        InitialTargetRotation = FootTarget.localRotation;
        InitialTargetPosition = FootTarget.localPosition;
        IKSettings = settings;
    }


    public void UpdateFoot(Transform characterRoot)
    {
        if (!Physics.Raycast(
            RayOrigin.position + Vector3.up * 0.1f,
            Vector3.down,
            out RaycastHit hit,
            IKSettings.RaycastDistance,
            IKSettings.WalkableLayer))
        {
            Lift();
            return;
        }

        Vector3 position = CalculateFootPosition(hit);
        Quaternion rotation = CalculateFootRotation(hit, characterRoot);

        CurrentGroundPosition = hit.point;
        CurrentIKPosition = FootTarget.parent.InverseTransformPoint(position);
        CurrentIKRotation = rotation * InitialTargetRotation;

        switch (State)
        {
            case FootState.Lifted:
            case FootState.Lifting:

                ApplyIK(
                    CurrentIKPosition,
                    CurrentIKRotation,
                    0f);

                if (Constraint.weight <= 0.01f)
                    State = FootState.Lifted;

                break;

            case FootState.Planting:
            case FootState.Planted:

                Vector3 localPosition =
                    FootTarget.parent.InverseTransformPoint(
                        LockedWorldPosition);

                Quaternion localRotation =
                    Quaternion.Inverse(FootTarget.parent.rotation) *
                    LockedWorldRotation;

                ApplyIK(
                    localPosition,
                    localRotation,
                    1f);

                if (Constraint.weight >= 0.99f)
                    State = FootState.Planted;

                break;
        }

        DrawDebug(RayOrigin.position, hit);
    }

    public float GetFootHeight()
    {
        return FootTarget.position.y;
    }

    private Vector3 CalculateFootPosition(RaycastHit hit)
    {
        return hit.point +
               hit.normal * IKSettings.GroundOffset;
    }



    private Quaternion CalculateFootRotation(
        RaycastHit hit,
        Transform characterRoot)
    {
        Vector3 forward =
            Vector3.ProjectOnPlane(
                characterRoot.forward,
                hit.normal)
            .normalized;


        Quaternion rotation =
            Quaternion.LookRotation(
                forward,
                hit.normal);


        return Quaternion.Inverse(characterRoot.rotation)
               * rotation;
    }



    private void ApplyIK(
    Vector3 position,
    Quaternion rotation,
    float targetWeight)
    {
        CurrentWeight = Mathf.MoveTowards(
            CurrentWeight,
            targetWeight,
            Time.deltaTime * IKSettings.WeightBlendSpeed);

        Constraint.weight = CurrentWeight;

        if (CurrentWeight <= 0.001f)
            return;

        FootTarget.localPosition = Vector3.Lerp(
            FootTarget.localPosition,
            position,
            IKSettings.FootPositionBlendSpeed * Time.deltaTime);

        FootTarget.localRotation = Quaternion.Slerp(
            FootTarget.localRotation,
            rotation,
            IKSettings.FootRotationBlendSpeed * Time.deltaTime);

        #if UNITY_EDITOR
                Debug.DrawLine(
                    FootTarget.position,
                    Constraint.data.tip.position,
                    Color.red);
        #endif
    }

    public void Plant()
    {
        if (State == FootState.Planted)
            return;

        LockedWorldPosition = FootTarget.position;
        LockedWorldRotation = FootTarget.rotation;

        State = FootState.Planted;
    }

    public void Lift()
    {
        State = FootState.Lifted;
    }
    public Vector3 GetFootPosition()
    {
        return FootTarget.position;
    }
    public Vector3 GetGroundPosition()
    {
        return CurrentGroundPosition;
    }
    public void ApplyHipOffset(float offset, float speed)
    {
        Vector3 localUp = InitialTargetRotation * Vector3.up;

        Vector3 target = CurrentIKPosition + localUp * offset;

        FootTarget.localPosition =
            Vector3.Lerp(
                FootTarget.localPosition,
                target,
                Time.deltaTime * speed);
    }
    private void DrawDebug(Vector3 origin, RaycastHit hit)
    {
        Debug.DrawRay(
            origin,
            Vector3.down * IKSettings.RaycastDistance,
            Color.yellow);


        Debug.DrawRay(
            hit.point,
            hit.normal * 0.3f,
            Color.green);
    }
}

public enum FootState
{
    Lifted,
    Planting,
    Planted,
    Lifting
}