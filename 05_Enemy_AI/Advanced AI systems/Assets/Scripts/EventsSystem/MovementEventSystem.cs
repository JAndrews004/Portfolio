using UnityEngine;

public class MovementEventSystem 
{
    NoiseSystem system;
    AnimationEventReceiver receiver;
    public MovementEventSystem(AnimationEventReceiver reciever)
    {
        this.receiver = reciever;
    }
    public void HandleJumpStart()
    {
        if (system == null)
        {
            system = GameObject.FindFirstObjectByType<NoiseSystem>();
        }
        Debug.Log("Jump logic triggered");

        // Example:
        // Apply jump effects

        if (system != null)
        {
            
            system.EmitNoise(receiver.gameObject.transform.position, receiver.movementNoiseStrength, NoiseType.Impact, receiver.entity);
        }
    }

    public void HandleLand()
    {
        if (system == null)
        {
            system = GameObject.FindFirstObjectByType<NoiseSystem>();
        }
        Debug.Log("Landing logic triggered");

        // Example:
        // Play landing impact
        // Trigger camera shake

        if (system != null)
        {
            system.EmitNoise(receiver.gameObject.transform.position, receiver.movementNoiseStrength, NoiseType.Impact, receiver.entity);
        }
    }
}