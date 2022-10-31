using System;
using System.Threading.Tasks;
using UnityEngine;

public class AudioSelector : MonoBehaviour
{
    public static AudioSelector instance;
    public AudioClip[] audioIntro;
    public AudioClip[] audioInstructions;
    public AudioClip[] audioSuccess;
    public AudioClip[] audioFail;
    public AudioClip ending;

    public AudioSource audioSource;
    private void Awake()
    {
        instance = this;
        audioSource = GetComponent<AudioSource>();
    }

    public async Task PlayIntro()
    {
        audioSource.clip = audioIntro[UnityEngine.Random.Range(0, audioIntro.Length)];
        await Play();

        foreach (AudioClip audioInstruction in audioInstructions)
        {
            audioSource.clip = audioInstruction;
            await Play();
        }
    }
    public async Task PlaySuccess()
    {
        audioSource.clip = audioSuccess[UnityEngine.Random.Range(0, audioSuccess.Length)];
        await Play();
    }
    public async Task PlayFail()
    {
        audioSource.clip = audioFail[UnityEngine.Random.Range(0, audioFail.Length)];
        await Play();
    }
    public async Task PlayEnding()
    {
        audioSource.clip = ending;
        await Play();
    }

    async Task Play()
    {
        audioSource.Play();
        await Task.Delay(TimeSpan.FromSeconds(audioSource.clip.length));
        await Task.Delay(300);
    }

    public async Task Play(AudioClip clip)
    {
        audioSource.clip = clip;
        await Play();
    }
}
