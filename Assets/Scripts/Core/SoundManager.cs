using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    public AudioClip hurtSound; // Assign in inspector
    public AudioClip gemSound; // Assign in inspector
    public AudioClip breakSound; // Assign in inspector

    public AudioClip stepSound; // Assign in inspector
    public AudioClip flaskSound; // Assign in inspector
    public AudioClip levelUpSound; // Assign in inspector
    public AudioClip npcSound; // Assign in inspector
    public AudioSource gameMusic;
        private Coroutine fadeCoroutine;




    public int maxSimultaneousSounds = 2;

    private List<AudioSource> audioSources = new List<AudioSource>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Initialize audio sources
        for (int i = 0; i < maxSimultaneousSounds; i++)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();
            audioSources.Add(source);
        }
    }

    // Method to change the game music with a fade transition
    public void ChangeGameMusic(AudioClip audioClip, float volume = 1f, float pitchVariation = 0, float fadeDuration = 1f)
    {
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }
        gameMusic.volume = 0;
        gameMusic.clip = audioClip;
        gameMusic.Play();
        fadeCoroutine = StartCoroutine(FadeMusic(audioClip, volume, pitchVariation, fadeDuration));
    }

        // Coroutine for fading the music
    private IEnumerator FadeMusic(AudioClip audioClip, float targetVolume, float pitchVariation, float fadeDuration)
    {
        float startTime = Time.time;
        float startVolume = 0;

        while (Time.time < startTime + fadeDuration)
        {
            float t = (Time.time - startTime) / fadeDuration;
            gameMusic.volume = Mathf.Lerp(startVolume, targetVolume, t);
            yield return null;
        }

        gameMusic.volume = targetVolume;
        gameMusic.clip = audioClip;
        gameMusic.Play();
    }

    // Method to play any sound clip
    public void PlaySound(AudioClip audioClip, float volume = 1f, float pitchVariation = 0.05f)
    {


        // Find an available audio source to play the sound
        foreach (var source in audioSources)
        {
            if (!source.isPlaying)
            {

                if (audioClip == stepSound && source.isPlaying)
                {
                    return;
                }

                source.clip = audioClip; // Assign the clip to play
                source.volume = volume; // Set the volume for this clip
                source.pitch = 1f + Random.Range(-pitchVariation, pitchVariation); // Optional: Slight pitch variation
                source.Play();
                return;
            }
        }
    }


}
