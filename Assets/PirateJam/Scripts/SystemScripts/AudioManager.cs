using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[HelpURL("https://www.youtube.com/watch?v=QuXqyHpquLg&t=7s")]
public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [Header("References")]
    [SerializeField] private AudioSource Player;
    [SerializeField] private AudioSource Environment;
    [SerializeField] private AudioSource Puzzles;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource musicSource;

    private Dictionary<string, AudioSource> audioDict;
    private Dictionary<string, AudioClip[]> sfxDict;

    [Header("Music")]
    [SerializeField] private AudioClip[] musicList;

    [Header("SFX")]
    [SerializeField] private AudioClip[] PlayerSFX;
    [SerializeField] private AudioClip[] PotionExplosionSFX;
    [SerializeField] private AudioClip[] SlimeSFX;
    [SerializeField] private AudioClip[] TurtleSFX;
    [SerializeField] private AudioClip[] EntitiesSFX;
    [SerializeField] private AudioClip[] EnvironmentSFX;
    [SerializeField] private AudioClip[] PuzzlesSFX;

    [Header("ButtonSFX")]
    [SerializeField] private AudioClip[] buttonSfxList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeAudioDict();
            InitializeSFXDict();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void InitializeAudioDict()
    {
        audioDict = new Dictionary<string, AudioSource>
        {
            { "Player", Player },
            { "Environment", Environment },
            { "Puzzles", Puzzles }
        };
    }

    private void InitializeSFXDict()
    {
        sfxDict = new Dictionary<string, AudioClip[]>
        {
            { "Player", PlayerSFX },
            { "PotionExplosion", PotionExplosionSFX },
            { "Entities", EntitiesSFX },
            { "Slime", SlimeSFX },
            { "Turtle", TurtleSFX },
            { "Environment", EnvironmentSFX },
            { "Puzzles", PuzzlesSFX }
        };
    }

    public void PlayMusic(int musicNumber, float volume = 1)
    {
        musicSource.clip = musicList[musicNumber];
        musicSource.volume = volume;
        musicSource.Play();
    }

    public void PlayRandomSFX(string audioKey, float volume = 1)
    {
        if (audioDict.TryGetValue(audioKey, out AudioSource source) && sfxDict.TryGetValue(audioKey, out AudioClip[] clips))
        {
            if (clips.Length == 0)
            {
                Debug.LogWarning($"No AudioClips found for key '{audioKey}'.");
                return;
            }

            int randomIndex = Random.Range(0, clips.Length);

            source.clip = clips[randomIndex];
            source.volume = volume;
            source.Play();
        }
        else
        {
            Debug.LogWarning($"AudioSource or AudioClips with key '{audioKey}' not found.");
        }
    }

    public void PlayRandomSFX(AudioSource audioSource, string audioKey, float volume = 1)
    {
        if (sfxDict.TryGetValue(audioKey, out AudioClip[] clips))
        {
            if (clips.Length == 0)
            {
                Debug.LogWarning($"No AudioClips found for key '{audioKey}'.");
                return;
            }

            int randomIndex = Random.Range(0, clips.Length);

            audioSource.clip = clips[randomIndex];
            audioSource.volume = volume;
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"AudioSource or AudioClips with key '{audioKey}' not found.");
        }
    }

    public void PlaySFX(string audioKey, int index)
    {
        if (audioDict.TryGetValue(audioKey, out AudioSource source) && sfxDict.TryGetValue(audioKey, out AudioClip[] clips))
        {
            if (clips.Length == 0)
            {
                Debug.LogWarning($"No AudioClips found for key '{audioKey}'.");
                return;
            }
            source.clip = clips[index];
            source.Play();
        }
        else
        {
            Debug.LogWarning($"AudioSource or AudioClips with key '{audioKey}' not found.");
        }
    }

    public void PlaySFX(AudioSource audioSource, string audioKey, int index)
    {
        if (sfxDict.TryGetValue(audioKey, out AudioClip[] clips))
        {
            if (clips.Length == 0)
            {
                Debug.LogWarning($"No AudioClips found for key '{audioKey}'.");
                return;
            }
            audioSource.clip = clips[index];
            audioSource.Play();
        }
        else
        {
            Debug.LogWarning($"AudioSource or AudioClips with key '{audioKey}' not found.");
        }
    }

    public void PlayButtonSFX(int buttonNumber, float volume = 1)
    {
        audioSource.clip = buttonSfxList[buttonNumber];
        audioSource.volume = volume;
        audioSource.Play();
    }
}