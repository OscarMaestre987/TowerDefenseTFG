using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource effectsSource;
    public AudioClip shootClip;
    public AudioClip reloadClip;
    public AudioClip hitClip;
    public AudioClip enemyDeathClip;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
            effectsSource.PlayOneShot(clip);
    }
}
