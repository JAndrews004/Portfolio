using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    [SerializeField] CharacterMotor motor;
    [Header("Systems")]
    [SerializeField] private FootstepSystem footstepSystem;
    [SerializeField] private MovementEventSystem movementEventSystem;

    [SerializeField] private FootstepAudioSystem footstepAudio;
    [SerializeField] private MovementAudioSystem movementAudio;


    public void OnFootstep()
    {
        if (!motor.IsGrounded)
        {
            return;
        }
        Debug.Log("Footstep event");
        footstepSystem?.TriggerFootstep();
        footstepAudio?.PlayFootstep();
    }

    public void OnJumpStart()
    {
        Debug.Log("Jump start event");
        movementEventSystem?.HandleJumpStart();
        movementAudio?.PlayJumpStart();
    }

    public void OnLand()
    {
        Debug.Log("Land event");
        movementEventSystem?.HandleLand();
        movementAudio?.PlayLand();
    }

    public void OnFinishClimb()
    {
        motor.FinishClimb();    
    }
}