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
        switch (petalIndex)
        {
            case 0: return Notes.C_MAJ;
            case 1: return Notes.G_MAJ;
            case 2: return Notes.D_MAJ;
            case 3: return Notes.A_MAJ;
            case 4: return Notes.E_MAJ;
            case 5: return Notes.B_MAJ;
            case 6: return Notes.GB_MAJ;
            case 7: return Notes.DB_MAJ;
            case 8: return Notes.AB_MAJ;
            case 9: return Notes.EB_MAJ;
            case 10: return Notes.BB_MAJ;
            case 11: return Notes.F_MAJ;

            case 12: return Notes.A_MIN;
            case 13: return Notes.E_MIN;
            case 14: return Notes.B_MIN;
            case 15: return Notes.GB_MIN;
            case 16: return Notes.DB_MIN;
            case 17: return Notes.AB_MIN;
            case 18: return Notes.EB_MIN;
            case 19: return Notes.BB_MIN;
            case 20: return Notes.F_MIN;
            case 21: return Notes.C_MIN;
            case 22: return Notes.G_MIN;
            case 23: return Notes.D_MIN;
        }

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
