using UnityEngine;

public class HipIKSolver
{
    private FootIKSettings settings;
    private Vector3 InitialHipPosition;
    private Quaternion InitialHipRotation;
    private float InitialLeftFootHeight;
    private float InitialRightFootHeight;
    private bool hasInitialFootHeight;

    private Transform rootTransform;
    private Transform HipTarget;

    private FootIKSolver leftFoot;
    private FootIKSolver rightFoot;
    public HipIKSolver(FootIKSettings IKSettings, Transform rootTransform, Transform HipTarget, FootIKSolver leftFoot, FootIKSolver rightFoot)
    {
        settings = IKSettings;
        this.rootTransform = rootTransform;
        this.HipTarget = HipTarget;
        InitialHipPosition = HipTarget.localPosition;
        InitialHipRotation = HipTarget.localRotation;

        InitialLeftFootHeight =
            rootTransform.InverseTransformPoint(
                leftFoot.GetGroundPosition()).y;

        InitialRightFootHeight =
            rootTransform.InverseTransformPoint(
                rightFoot.GetGroundPosition()).y;

        this.leftFoot = leftFoot;
        this.rightFoot = rightFoot;
    }

    public void UpdateHipHeight()
    {
        float leftHeight =
            rootTransform.InverseTransformPoint(
                leftFoot.GetGroundPosition()).y;

        float rightHeight =
            rootTransform.InverseTransformPoint(
                rightFoot.GetGroundPosition()).y;


        if (!hasInitialFootHeight)
        {
            InitialLeftFootHeight = leftHeight;
            InitialRightFootHeight = rightHeight;
            hasInitialFootHeight = true;
        }


        float leftDelta =
            leftHeight - InitialLeftFootHeight;

        float rightDelta =
            rightHeight - InitialRightFootHeight;


        float targetOffset =
            Mathf.Min(leftDelta, rightDelta);



        targetOffset =
            Mathf.Clamp(
                targetOffset,
                -settings.MaxHipOffset,
                settings.MaxHipOffset);


        Vector3 targetPosition =
            InitialHipPosition +
            Vector3.up * targetOffset;


        HipTarget.localPosition =
            Vector3.Lerp(
                HipTarget.localPosition,
                targetPosition,
                Time.deltaTime * settings.HipHeightBlendSpeed);

        float heightDifference = leftHeight - rightHeight;

        float tilt =
            Mathf.Clamp(
                heightDifference * 50f,
                -settings.MaxHipTilt,
                settings.MaxHipTilt);

        Quaternion targetRotation =
            InitialHipRotation *
            Quaternion.Euler(
                0f,
                0f,
                tilt);

        HipTarget.localRotation =
            Quaternion.Slerp(
                HipTarget.localRotation,
                targetRotation,
                Time.deltaTime * settings.HipRotationBlendSpeed);

        Debug.Log(
            $"Left Delta: {leftDelta:F2} " +
            $"Right Delta: {rightDelta:F2} " +
            $"Hip Offset: {targetOffset:F2}");
    }
}
