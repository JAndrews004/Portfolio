using UnityEngine;

public class MovementAudioSystem
{
    AnimationEventReceiver reciever;
    public MovementAudioSystem(AnimationEventReceiver reciever)
    {
        this.reciever = reciever;
    }


    public void PlayJumpStart()
    {
        reciever.movemntAudioSource.PlayOneShot(reciever.jumpClip);
    }

    public void PlayLand()
    {

        reciever.movemntAudioSource.PlayOneShot(reciever.landClip);
    }
}