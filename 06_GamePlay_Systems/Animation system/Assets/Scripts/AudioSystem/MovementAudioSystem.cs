using UnityEngine;

public class MovementAudioSystem : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip landClip;


    public void PlayJumpStart()
    {
        audioSource.PlayOneShot(jumpClip);
    }

    public void PlayLand()
    {

        audioSource.PlayOneShot(landClip);
    }
}