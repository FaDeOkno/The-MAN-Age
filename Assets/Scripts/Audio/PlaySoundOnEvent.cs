using UnityEngine;

public class PlaySoundOnEvent : MonoBehaviour
{
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private float _delay = 0f;

    public void PlaySound()
    {
        _audioSource.PlayDelayed(_delay);
    }
}
