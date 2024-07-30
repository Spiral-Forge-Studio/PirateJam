using UnityEngine;

public class ExplosionSound : MonoBehaviour
{
    private AudioSource audioSource;
    public string sfxDictName;


    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        AudioManager.instance.PlayRandomSFX(audioSource, sfxDictName);

        Destroy(gameObject, audioSource.clip.length+0.1f);
    }
}
