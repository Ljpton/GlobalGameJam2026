using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject princePrefab;
    public GameObject frogPrefab;

    public GameObject majorPetals;
    public GameObject minorPetals;

    public LevelData currentLevel;

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
        InitLevel();
    }

    public void LoadLevel(LevelData level)
    {
        currentLevel = level;
    }

    public void InitLevel()
    {
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

            for (int j = 0; j < pathLength; j++)
            {
                switch (currentLevel.levelDifficulty)
                {
                    case Difficulties.SINGLE:
                        randomPath[j] = Random.Range(0, 12); // Min incluse, max exclusive
                        break;
                    case Difficulties.MAJOR:
                        randomPath[j] = Random.Range(0, 12); // Min incluse, max exclusive
                        break;
                    case Difficulties.MINOR:
                        randomPath[j] = Random.Range(13, 24); // Min incluse, max exclusive
                        break;
                    case Difficulties.MIXED:    
                        randomPath[j] = Random.Range(0, 24); // Min incluse, max exclusive
                        break;
                }
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
        // TODO

        isPlayingSequence = true;
        Forward();
    }

    public void Forward()
    {
        Debug.Log("Forward");

        currentIndex += 1;
        if(currentIndex >= paths[0].Length)
        {
            currentIndex = 0;
        }
        
        foreach (Frog frog in frogs)
        {
            // frog.JumpToPetal(currentIndex);
        }

        // Play music
        AudioManager._instance.PlaySequence(currentLevel.notes[currentIndex], currentLevel.levelDifficulty);
    }

    public void Backward()
    {
        currentIndex -= 1;
        if (currentIndex < 0)
        {
            currentIndex = paths[0].Length - 1;
        }

        foreach (Frog frog in frogs)
        {
            // frog.JumpToPetal(currentIndex);
        }
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

    public void AskForNextStep()
    {
        if(isPlayingSequence)
        {
            Forward();
        }
    }
}
