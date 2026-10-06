using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class AnimationEventReceiver : MonoBehaviour
{
    [SerializeField] CharacterMotor motor;
    public IDetectableEntity entity;
    [Header("Systems")]
    [SerializeField] private FootstepSystem footstepSystem;
    [SerializeField] private MovementEventSystem movementEventSystem;

    private FootstepAudioSystem footstepAudio;
    private MovementAudioSystem movementAudio;

    [Header("Footsteps")]
    public AudioSource footstepAudioSource;
    public AudioClip[] footstepClips;
    public float pitchVariation = 0.1f;
    public float footNoiseStrength = 0.5f;

    [Header("Movement")]
    public AudioSource movemntAudioSource;
    public AudioClip jumpClip;
    public AudioClip landClip;
    public float movementNoiseStrength = 0.75f;

    
    private void Start()
    {
        entity = gameObject.GetComponent<DetectableEntity>();
        footstepSystem = new FootstepSystem(this);
        movementEventSystem = new MovementEventSystem(this);

        footstepAudio = new FootstepAudioSystem(this);
        movementAudio = new MovementAudioSystem(this);
    }
    public void OnFootstep()
    {
        if (!motor.IsGrounded)
        {
            return;
        }
        //Debug.Log("Footstep event");
        footstepSystem?.TriggerFootstep(motor.GetSpeed()/motor.movementConfig.sprintSpeed);
        footstepAudio?.PlayFootstep();
    }

    public void OnJumpStart()
    {
        //Debug.Log("Jump start event");
        movementEventSystem?.HandleJumpStart();
        movementAudio?.PlayJumpStart();
    }

    public void OnLand()
    {
        //Debug.Log("Land event");
        movementEventSystem?.HandleLand();
        movementAudio?.PlayLand();
    }

    public void OnFinishClimb()
    {
        motor.FinishClimb();    
    }
}