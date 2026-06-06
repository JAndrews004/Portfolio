using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;

public class DebugUIController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private CharacterMotor motor;
    [SerializeField] private Animator animator;
    private InputSystem_Actions inputActions;

    private VisualElement root;

    private Label stateLabel;
    private Label speedLabel;
    private Label directionLabel;
    private Label groundedLabel;

    private Label clipLabel;
    private Label blendLabel;
    private Label directionXLabel;
    private Label directionZLabel;
    private Label airLayerWeightLabel;
    private Label upperLayerWeightLabel;
    private Label directionRawLabel;

    private bool isVisible = true;

    private float displayedSpeed = 0f;

    void Awake()
    {
        inputActions = new InputSystem_Actions();
    }

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        root = uiDocument.rootVisualElement;

        // Query UI elements (names must match UXML)
        stateLabel = root.Q<Label>("stateLabel");
        speedLabel = root.Q<Label>("speedLabel");
        directionLabel = root.Q<Label>("directionLabel");
        groundedLabel = root.Q<Label>("groundedLabel");

        clipLabel = root.Q<Label>("clipLabel");
        blendLabel = root.Q<Label>("blendLabel");
        directionXLabel = root.Q<Label>("directionXLabel");
        directionZLabel = root.Q<Label>("directionZLabel");
        airLayerWeightLabel = root.Q<Label>("airLayerWeightLabel");
        upperLayerWeightLabel = root.Q<Label>("upperLayerWeightLabel");
        directionRawLabel = root.Q<Label>("directionRawLabel");


        // Enable input + subscribe
        inputActions.Enable();
        inputActions.Player.ToggleDebug.performed += OnToggleDebug;

        // Ensure correct initial visibility
        root.style.display = DisplayStyle.Flex;
        isVisible = true;
    }

    void OnDisable()
    {
        inputActions.Player.ToggleDebug.performed -= OnToggleDebug;
        inputActions.Disable();
    }

    void Update()
    {
        if (!isVisible || motor == null) return;

        UpdateState();
        UpdateSpeed();
        UpdateDirection();
        UpdateGrounded();
        UpdateAnimationVariables(); 
        UpdateClip();
    }

    private void OnToggleDebug(InputAction.CallbackContext context)
    {
        isVisible = !isVisible;
        root.style.display = isVisible ? DisplayStyle.Flex : DisplayStyle.None;
    }

    void UpdateState()
    {
        stateLabel.text = $"State: {motor.currentState}";
    }

    void UpdateSpeed()
    {
        float targetSpeed = motor.GetSpeed();

        
        speedLabel.text = $"Speed: {animator.GetFloat("Speed"):F2}";
    }

    void UpdateDirection()
    {
        Vector3 dir = motor.GetDirection();

        if (dir.magnitude < 0.1f)
        {
            directionLabel.text = "Direction: Idle";
            return;
        }

        // Convert to local space (relative to player)
        Vector3 localDir = motor.transform.InverseTransformDirection(dir.normalized);

        // Get angle in degrees (-180 to 180)
        float angle = Mathf.Atan2(localDir.x, localDir.z) * Mathf.Rad2Deg;

        string direction;

        if (angle >= -22.5f && angle < 22.5f)
            direction = "Forward";
        else if (angle >= 22.5f && angle < 67.5f)
            direction = "Front Right";
        else if (angle >= 67.5f && angle < 112.5f)
            direction = "Right";
        else if (angle >= 112.5f && angle < 157.5f)
            direction = "Back Right";
        else if (angle >= 157.5f || angle < -157.5f)
            direction = "Backward";
        else if (angle >= -157.5f && angle < -112.5f)
            direction = "Back Left";
        else if (angle >= -112.5f && angle < -67.5f)
            direction = "Left";
        else // -67.5 to -22.5
            direction = "Front Left";

        directionLabel.text = $"Direction: {direction}";
    }

    void UpdateGrounded()
    {
        bool grounded = motor.IsGrounded;

        groundedLabel.text = $"Grounded: {grounded}";

        // Colour feedback
        groundedLabel.style.color = grounded
            ? new StyleColor(Color.green)
            : new StyleColor(Color.red);
    }
    void UpdateAnimationVariables()
    {
        
        float dirX = animator.GetFloat("DirectionX");
        float dirZ = animator.GetFloat("DirectionZ");

        directionRawLabel.text = $"Dir: ({dirX:F2}, {dirZ:F2})";

        float airWeight = animator.GetLayerWeight(animator.GetLayerIndex("InAir"));
        float upperWeight = animator.GetLayerWeight(animator.GetLayerIndex("Upper"));

        airLayerWeightLabel.text = $"Air: {airWeight:F2}";
        upperLayerWeightLabel.text = $"Upper: {upperWeight:F2}";
    }

    void UpdateClip()
    {
        var clips = animator.GetCurrentAnimatorClipInfo(0);

        if (clips.Length == 0)
        {
            clipLabel.text = "Clip: None";
            return;
        }

        AnimatorClipInfo bestClip = clips[0];

        foreach (var clip in clips)
        {
            if (clip.weight > bestClip.weight)
                bestClip = clip;
        }

        clipLabel.text = $"Clip: {bestClip.clip.name} ({bestClip.weight:F2})";
    }
}