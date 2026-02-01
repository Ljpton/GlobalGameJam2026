using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager _instance;
    [Header("Piano Samples")]
    [SerializeField] private AudioClip[] piano_singleNote;
    [SerializeField] private AudioClip[] piano_singleProgression;
    [SerializeField] private AudioClip[] piano_major_singleChord;
    [SerializeField] private AudioClip[] piano_major_ChordProgression;
    [SerializeField] private AudioClip[] piano_minor_singleChord;
    [SerializeField] private AudioClip[] piano_minor_ChordProgression;
    [Header("Guitar Samples")]
    [SerializeField] private AudioClip[] guitar_singleNote;
    [SerializeField] private AudioClip[] guitar_singleProgression;
    [SerializeField] private AudioClip[] guitar_major_singleChord;
    [SerializeField] private AudioClip[] guitar_major_ChordProgression;
    [SerializeField] private AudioClip[] guitar_minor_singleChord;
    [SerializeField] private AudioClip[] guitar_minor_ChordProgression;
    [Header("Rhythm Samples")]
    [SerializeField] private AudioClip[] percussion;
    [SerializeField] private AudioClip[] bass_singleNote;

    [Header("SFX Clips")] 
    [SerializeField] private AudioClip[] prince_win;
    [SerializeField] private AudioClip[] frog_lose;
    [SerializeField] private AudioClip ambience;
    
    
    [Header("Audio Sources")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource percussionSource;
    [SerializeField] private AudioSource bassSource;
    [SerializeField] private AudioSource guitarSource;
    [SerializeField] private AudioSource strummingSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource ambienceSource;

    [Header("Mixing")]
    [SerializeField] private float SFXVolume;
    [SerializeField] private float MusicVolume;
    [SerializeField] private float MasterVolume;

    private bool firstPercussionActive = true;

    private void Awake()
    {
        _instance = this;
    }


    /*public static void PlaySFX()
    {
        
    }*/

    private void Start()
    {
        StopAmbience();
        PlayAmbience();
    }

    public void PlaySequence(Notes id, Difficulties difficulty)
    {
        if (musicSource.isPlaying) musicSource.Stop();

        switch (difficulty)
        {
            case Difficulties.SINGLE:
                musicSource.PlayOneShot(guitar_singleProgression[(int)id], MusicVolume);
                guitarSource.PlayOneShot(guitar_major_ChordProgression[(int)id], MusicVolume);
                break;
            case Difficulties.MAJOR:
                musicSource.PlayOneShot(guitar_major_ChordProgression[(int)id], MusicVolume);
                guitarSource.PlayOneShot(guitar_major_singleChord[(int)id], MusicVolume);
                break;
            case Difficulties.MINOR:
                musicSource.PlayOneShot(guitar_minor_ChordProgression[((int)id - 12)], MusicVolume);
                guitarSource.PlayOneShot(guitar_minor_singleChord[((int)id - 12)], MusicVolume);
                break;
            case Difficulties.MIXED:
                if ((int)id >= 12)
                {
                    musicSource.PlayOneShot(guitar_minor_ChordProgression[((int)id - 12)], MusicVolume);
                    guitarSource.PlayOneShot(guitar_minor_singleChord[((int)id - 12)], MusicVolume);
                }
                else
                {
                    musicSource.PlayOneShot(guitar_major_ChordProgression[(int)id], MusicVolume);
                    guitarSource.PlayOneShot(guitar_major_singleChord[(int)id], MusicVolume);
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

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.AskForNextStep();
        }
        else if (SandboxManager.Instance != null)
        {
            SandboxManager.Instance.AskForNextStep();
        }
    }

    private IEnumerator WaitUntilSongClipEnd()
    {
        while (percussionSource.isPlaying)
        {
            yield return null;
        }

        if (LevelManager.Instance != null)
        {
            LevelManager.Instance.AskForNextStep();
        }
        else if (SandboxManager.Instance != null)
        {
            SandboxManager.Instance.AskForNextStep();
        }
    }

    public void PlaySingle(Notes id, Difficulties difficulty)
    {
        switch (difficulty)
        {
            case Difficulties.SINGLE:
                musicSource.PlayOneShot(guitar_singleNote[(int)id], MusicVolume);
                break;
            case Difficulties.MAJOR:
                musicSource.PlayOneShot(guitar_major_singleChord[(int)id], MusicVolume);
                break;
            case Difficulties.MINOR:
                musicSource.PlayOneShot(guitar_minor_singleChord[((int)id - 12)], MusicVolume);
                break;

            case Difficulties.MIXED:
                if ((int)id >= 12)
                {
                    musicSource.PlayOneShot(guitar_minor_singleChord[((int)id - 12)], MusicVolume);
                }
                else
                {
                    musicSource.PlayOneShot(guitar_major_singleChord[(int)id], MusicVolume);
                }
                break;
        }
    }

    public void PlaySong(Notes id, Difficulties difficulty)
    {
        PlayPercussion();
        switch (difficulty)
        {
            case Difficulties.SINGLE:
                musicSource.PlayOneShot(guitar_singleProgression[(int)id], MusicVolume);
                guitarSource.PlayOneShot(guitar_major_ChordProgression[(int)id], MusicVolume);
                bassSource.PlayOneShot(bass_singleNote[(int)id], MusicVolume);
                break;
            case Difficulties.MAJOR:
                guitarSource.PlayOneShot(guitar_major_singleChord[(int)id], MusicVolume);
                strummingSource.PlayOneShot(guitar_major_ChordProgression[(int)id], MusicVolume);
                bassSource.PlayOneShot(bass_singleNote[(int)id], MusicVolume);
                break;
            case Difficulties.MINOR:
                guitarSource.PlayOneShot(guitar_minor_singleChord[((int)id - 12)], MusicVolume);
                strummingSource.PlayOneShot(guitar_minor_ChordProgression[((int)id - 12)], MusicVolume);
                bassSource.PlayOneShot(bass_singleNote[((int)id - 12)], MusicVolume);
                break;
            case Difficulties.MIXED:
                if ((int)id >= 12)
                {
                    guitarSource.PlayOneShot(guitar_minor_singleChord[((int)id - 12)], MusicVolume);
                    strummingSource.PlayOneShot(guitar_minor_ChordProgression[((int)id - 12)], MusicVolume);
                    bassSource.PlayOneShot(bass_singleNote[((int)id - 12)], MusicVolume);
                }
                else
                {
                    guitarSource.PlayOneShot(guitar_major_singleChord[(int)id], MusicVolume);
                    strummingSource.PlayOneShot(guitar_major_ChordProgression[(int)id], MusicVolume);
                    bassSource.PlayOneShot(bass_singleNote[(int)id], MusicVolume);
                }
                break;
        }

        StartCoroutine(WaitUntilSongClipEnd());
    }

    public void StopAllSources()
    {
        musicSource.Stop();
        percussionSource.Stop();
        bassSource.Stop();
        guitarSource.Stop();
        strummingSource.Stop();
    }

    private void PlayPercussion()
    {
        if (firstPercussionActive)
        {
            percussionSource.PlayOneShot(percussion[0], MusicVolume);
            firstPercussionActive = false;
        }
        else
        {
            percussionSource.PlayOneShot(percussion[1], MusicVolume);
            firstPercussionActive = true;
        }
    }

    public void PlayPrinceWin()
    {
        int rand = Random.Range(0, prince_win.Length);
        sfxSource.PlayOneShot(prince_win[rand], SFXVolume);
    }

    public void PlayFrogLose()
    {
        int rand = Random.Range(0, frog_lose.Length);
        sfxSource.PlayOneShot(frog_lose[rand], SFXVolume);
    }

    public void PlayAmbience()
    {
        ambienceSource.clip = ambience;
        ambienceSource.volume = SFXVolume;
        ambienceSource.Play();
    }

    public void StopAmbience()
    {
        if (ambienceSource.isPlaying)
        {
            ambienceSource.Stop();
            
        }
    }
}
