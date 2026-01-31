using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;

    public GameObject princePrefab;
    public GameObject frogPrefab;

    public LevelData currentLevel;

    private List<Frog> frogs = new();
    private List<int[]> paths = new();
    
    private int currentIndex = 0;

    private void Awake()
    {
        Instance = this;
    }

    public void LoadLevel(LevelData level)
    {
        currentLevel = level;
    }

    public void InitLevel()
    {
        // Create paths
        int[] princePath = currentLevel.GetPetalIndices();
        paths.Add(princePath);

        int pathLength = princePath.Length;

        for (int i = 1; i < currentLevel.frogCount; i++)
        {
            int[] randomPath = new int[pathLength];

            for (int j = 0; j < pathLength; j++)
            {
                randomPath[j] = Random.Range(0, 12); // Min incluse, max exclusive
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
    }

    public void Forward()
    {
        currentIndex += 1;
        if(currentIndex >= paths[0].Length)
        {
            currentIndex = 0;
        }
        
        foreach (Frog frog in frogs)
        {
            // frog.JumpToPetal(currentIndex);
        }
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
}
