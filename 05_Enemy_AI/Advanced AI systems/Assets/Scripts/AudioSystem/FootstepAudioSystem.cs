using UnityEngine;

public class FootstepAudioSystem
{
    AnimationEventReceiver reciever;
    public FootstepAudioSystem(AnimationEventReceiver reciever)
    {
        this.reciever = reciever;
    }

    public void PlayFootstep()
    {
        if (reciever.footstepClips.Length == 0) return;

        AudioClip clip = reciever.footstepClips[Random.Range(0, reciever.footstepClips.Length)];

        float originalPitch = reciever.footstepAudioSource.pitch;

        reciever.footstepAudioSource.pitch = 1f + Random.Range(-reciever.pitchVariation, reciever.pitchVariation);
        reciever.footstepAudioSource.PlayOneShot(clip);

        reciever.footstepAudioSource.pitch = originalPitch;
    }
}