using UnityEngine;

public class FootstepSystem
{
    NoiseSystem system;
    AnimationEventReceiver receiver;
    public FootstepSystem(AnimationEventReceiver reciever)
    {
        this.receiver = reciever;
    }
    public void TriggerFootstep(float maxSpeedPercentage)
    {
        if(system == null)
        {
            system = GameObject.FindFirstObjectByType<NoiseSystem>();
        }

        //Debug.Log("Footstep system triggered");


        if(system != null )
        {
            //Debug.Log($"Noise made at {receiver.gameObject.transform.position}");
            system.EmitNoise(receiver.gameObject.transform.position, receiver.footNoiseStrength * maxSpeedPercentage, NoiseType.Footstep, receiver.entity);
        }
        // Later:
        // Play sound
        // Notify AI
        // Spawn VFX
    }
}