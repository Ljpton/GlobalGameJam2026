using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    [SerializeField] private AudioClip[] piano_singleNote;
    [SerializeField] private AudioClip[] piano_major_singleChord;
    [SerializeField] private AudioClip[] piano_major_ChordProgression;
    [SerializeField] private AudioClip[] piano_minor_singleChord;
    [SerializeField] private AudioClip[] piano_minor_ChordProgression;
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;

    [Header("Mixing")]
    [SerializeField] private float SFXVolume;
    [SerializeField] private float MusicVolume;
    [SerializeField] private float MasterVolume;

    private void Awake()
    {
        _instance = this;
    }


    /*public static void PlaySFX()
    {
        
    }*/

    public void PlaySequence(Notes id, Difficulties difficulty)
    {
        switch (difficulty)
        {
            case Difficulties.SINGLE:
                musicSource.PlayOneShot(piano_singleNote[(int)id], MusicVolume);
                break;
            case Difficulties.MAJOR:
                musicSource.PlayOneShot(piano_major_singleChord[(int)id], MusicVolume);
                break;
            case Difficulties.MINOR:
                musicSource.PlayOneShot(piano_minor_singleChord[((int)id - 12)], MusicVolume);
                break;
            case Difficulties.MIXED:
                if ((int)id >= 12)
                {
                    musicSource.PlayOneShot(piano_minor_singleChord[((int)id - 12)], MusicVolume);
                }
                else
                {
                    musicSource.PlayOneShot(piano_major_singleChord[(int)id], MusicVolume);
                }
                break;
        }

        StartCoroutine(WaitUntilClipEnd());
        
    }

    private IEnumerator WaitUntilClipEnd()
    {
        while (musicSource.isPlaying)
        {
            yield return null;
        }
        //LevelManager.Instance.AskForNextStep()
    }

    public void PlaySingle(Notes id, Difficulties difficulty)
    {
        switch (difficulty)
        {
            case Difficulties.SINGLE:
                musicSource.PlayOneShot(piano_singleNote[(int)id], MusicVolume);
                break;
            case Difficulties.MAJOR:
                musicSource.PlayOneShot(piano_major_singleChord[(int)id], MusicVolume);
                break;
            case Difficulties.MINOR:
                musicSource.PlayOneShot(piano_minor_singleChord[((int)id - 12)], MusicVolume);
                break;
            case Difficulties.MIXED:
                if ((int)id >= 12)
                {
                    musicSource.PlayOneShot(piano_minor_singleChord[((int)id - 12)], MusicVolume);
                }
                else
                {
                    musicSource.PlayOneShot(piano_major_singleChord[(int)id], MusicVolume);
                }
                break;
        }
    }
}
