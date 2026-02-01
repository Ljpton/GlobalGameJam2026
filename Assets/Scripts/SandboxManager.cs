using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SandboxManager : MonoBehaviour
{
    public static SandboxManager Instance;

    public Transform[] allPetals;
    
    private int currentIndex = 0;

    public Frog frog;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        AutoForward();   
    }

    public void AutoForward()
    {
        frog.JumpToPetal(allPetals[currentIndex]);

        AudioManager._instance.PlaySong(PetalToNote(currentIndex), Difficulties.MIXED);
    }


    public void ClickOnPetal(int index)
    {
        StopAutoplay();

        currentIndex = index;

        frog.JumpToPetal(allPetals[currentIndex]);

        // Play music
        AudioManager._instance.PlaySong(PetalToNote(currentIndex), Difficulties.MIXED);
    }

    private Notes PetalToNote(int petalIndex)
    {
        return Notes.C_MAJ;
    }

    public void StartAutoplay()
    {
        AudioManager._instance.StopAllCoroutines();

        AutoForward();
    }
    public void StopAutoplay()
    {
        AudioManager._instance.StopAllSources();
    }

    public void AskForNextStep()
    {
        AutoForward();
    }
}
