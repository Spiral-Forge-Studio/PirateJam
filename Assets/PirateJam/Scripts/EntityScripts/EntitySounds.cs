using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntitySounds : MonoBehaviour
{
    private AudioSource sound;

    public string sfxDictName;

    // Start is called before the first frame update
    void Awake()
    {
        sound = GetComponent<AudioSource>();
        
    }

    // Update is called once per frame
    void Start()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        AudioManager.instance.PlayRandomSFX(sound, sfxDictName);
    }
}
