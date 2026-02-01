using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject nextLevelPanel;
    public GameObject playbackPanel;

    public Image playButtonImage; 
    public Sprite stopImage;
    public Sprite playImage;

    public GameObject princePrefab;
    public GameObject frogPrefab;

    public GameObject majorPetals;
    public GameObject minorPetals;

    private List<int> majPetalIndices = new();
    private List<int> minPetalIndices = new();
    private List<int> mixedPetalIndices = new();

    public Transform[] allPetals;

    public LevelData currentLevel;
    public bool levelWon = false;

    private List<Frog> frogs = new();
    private List<int[]> paths = new();
    
    private int currentIndex = 0;

    private bool isPlayingSequence = true;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // I don't know how to do it otherwise
        majPetalIndices.Add(0); majPetalIndices.Add(1); majPetalIndices.Add(2); majPetalIndices.Add(3);
        majPetalIndices.Add(4); majPetalIndices.Add(5); majPetalIndices.Add(6); majPetalIndices.Add(7);
        majPetalIndices.Add(8); majPetalIndices.Add(9); majPetalIndices.Add(10); majPetalIndices.Add(11);

        minPetalIndices.Add(12); minPetalIndices.Add(13); minPetalIndices.Add(14); minPetalIndices.Add(15);
        minPetalIndices.Add(16); minPetalIndices.Add(17); minPetalIndices.Add(18); minPetalIndices.Add(19);
        minPetalIndices.Add(20); minPetalIndices.Add(21); minPetalIndices.Add(22); minPetalIndices.Add(23);

        mixedPetalIndices.AddRange(majPetalIndices);
        mixedPetalIndices.AddRange(minPetalIndices);
    }

    public void LoadLevel(LevelData level)
    {
        currentLevel = level;
    }

    public void InitLevel()
    {
        ClearLevel();

        levelWon = false;
        nextLevelPanel.SetActive(false);
        playbackPanel.SetActive(true);

        // Show petal corresponding petals (major, minor)
        majorPetals.SetActive(false);
        minorPetals.SetActive(false);

        switch (currentLevel.levelDifficulty)
        {
            case Difficulties.SINGLE:
                majorPetals.SetActive(true);
                break;
            case Difficulties.MAJOR:
                majorPetals.SetActive(true);
                break;
            case Difficulties.MINOR:
                minorPetals.SetActive(true);
                break;
            case Difficulties.MIXED:
                majorPetals.SetActive(true);
                minorPetals.SetActive(true);
                break;
        }

        // Create paths
        int[] princePath = currentLevel.GetPetalIndices();
        paths.Add(princePath);

        int pathLength = princePath.Length;

        for (int i = 1; i < currentLevel.frogCount; i++)
        {
            int[] randomPath = new int[pathLength];

            List<int> viablePetalsToJumpOn = new();

                switch (currentLevel.levelDifficulty)
                {
                    case Difficulties.SINGLE:
                        viablePetalsToJumpOn.AddRange(majPetalIndices);
                        break;
                    case Difficulties.MAJOR:
                        viablePetalsToJumpOn.AddRange(majPetalIndices);
                        break;
                    case Difficulties.MINOR:
                        viablePetalsToJumpOn.AddRange(minPetalIndices);
                        break;
                    case Difficulties.MIXED:
                        viablePetalsToJumpOn.AddRange(majPetalIndices);
                        viablePetalsToJumpOn.AddRange(minPetalIndices);
                        break;
                }

            for (int j = 0; j < pathLength; j++)
            {
                List<int> viablePetalsToJumpOnPerStep = new List<int>(viablePetalsToJumpOn);
                Debug.Log("Viable Petals Count: " + viablePetalsToJumpOnPerStep.Count);

                // For each previous frog
                for (int k = 0; k < i; k++)
                {
                    viablePetalsToJumpOnPerStep.Remove(paths[k][j]); // Remove petal which other frog already choose at this step
                }

                //if (j > 0)
                //{
                //    viablePetalsToJumpOnPerStep.Remove(paths[i][j - 1]); // Remove current petal from viable list
                //}

                Debug.Log("Viable Petals Count After: " + viablePetalsToJumpOnPerStep.Count);
                
                int randomIndex = Random.Range(0, viablePetalsToJumpOnPerStep.Count);

                Debug.Log("Random Index: " + randomIndex);

                randomPath[j] = viablePetalsToJumpOnPerStep[randomIndex];
            }

            paths.Add(randomPath);
        }

        // Spawn number of frogs
        GameObject prince = Instantiate(princePrefab);
        frogs.Add(prince.GetComponent<Frog>());

        for (int i = 1; i < currentLevel.frogCount; i++)
        {
            GameObject frog = Instantiate(frogPrefab);
            frogs.Add(frog.GetComponent<Frog>());
        }

        // Set frogs to starting petal
        for (int i = 0; i < currentLevel.frogCount; i++)
        {
            frogs[i].TeleportToPetal(allPetals[paths[i][0]]);
        }

        isPlayingSequence = true;
        StartAutoplay();
    }

    public void AutoForward()
    {
        if (!isPlayingSequence) return;

        currentIndex += 1;
        if(currentIndex >= paths[0].Length)
        {
            currentIndex = 0;
        }

        for(int i = 0; i < frogs.Count; i++)
        {
            frogs[i].JumpToPetal(allPetals[paths[i][currentIndex]]);
        }

        // Play music
        if(levelWon)
        {
            AudioManager._instance.PlaySong(currentLevel.notes[currentIndex], currentLevel.levelDifficulty);
        }
        else
        {
            AudioManager._instance.PlaySequence(currentLevel.notes[currentIndex], currentLevel.levelDifficulty);
        }
    }

    public void ToggleAutoPlay()
    {
        if (isPlayingSequence)
        {
            StopAutoplay();
        }
        else
        {
            StartAutoplay();
        }
    }

    public void Backward()
    {
        StopAutoplay();

        currentIndex -= 1;
        if (currentIndex < 0)
        {
            currentIndex = paths[0].Length - 1;
        }

        for (int i = 0; i < frogs.Count; i++)
        {
            frogs[i].JumpToPetal(allPetals[paths[i][currentIndex]]);
        }

        // Play music
        AudioManager._instance.PlaySequence(currentLevel.notes[currentIndex], currentLevel.levelDifficulty);
    }

    public void Forward()
    {
        StopAutoplay();

        currentIndex += 1;
        if (currentIndex >= paths[0].Length)
        {
            currentIndex = 0;
        }


        for (int i = 0; i < frogs.Count; i++)
        {
            frogs[i].JumpToPetal(allPetals[paths[i][currentIndex]]);
        }

        // Play music
        AudioManager._instance.PlaySequence(currentLevel.notes[currentIndex], currentLevel.levelDifficulty);
    }

    public void ClearLevel()
    {
        foreach (Frog frog in frogs)
        {
            Destroy(frog.gameObject);
        }

        frogs.Clear();
        paths.Clear();

        currentIndex = 0;
    }

    public void StartAutoplay()
    {
        AudioManager._instance.StopAllCoroutines();

        playButtonImage.sprite = stopImage;
        isPlayingSequence = true;
        AutoForward(); // This will cause bugs with double play, need check in AudioManager if AskForBlblbl pending (only forward if not)
    }
    public void StopAutoplay()
    {
        playButtonImage.sprite = playImage;
        isPlayingSequence = false;
        AudioManager._instance.StopAllSources();
    }

    public void AskForNextStep()
    {
        if(isPlayingSequence)
        {
            AutoForward();
        }
    }

    public void OnLevelWon()
    {
        levelWon = true;

        Frog prince = frogs[0];

        foreach (Frog frog in frogs)
        {
            if (!frog.CompareTag("Prince"))
            {
                Destroy(frog.gameObject);
            }
        }

        frogs.Clear();
        frogs.Add(prince);

        // Show next level button instead of control panel
    }

    public void TogglePlaybackPanel(bool toggle)
    {
        playbackPanel.SetActive(toggle);
    }

    public void ToggleNextLevelPanel(bool toggle)
    {
        nextLevelPanel.SetActive(toggle);
    }
}
