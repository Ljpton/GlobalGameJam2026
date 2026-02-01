using NUnit.Framework;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    public LevelData[] allLevels;

    public GameObject levelSelectionPanel;

    private int currentLevelIndex = 0;

    public void StartLevel(int levelIndex)
    {
        currentLevelIndex = levelIndex;

        LevelManager.Instance.LoadLevel(allLevels[levelIndex]);

        CloseLevelSelection();

        LevelManager.Instance.InitLevel();
    }

    public void StartNextLevel()
    {
        currentLevelIndex++;

        if (currentLevelIndex >= allLevels.Length)
        {
            currentLevelIndex = 0;
        }

        StartLevel(currentLevelIndex);
    }

    public void OpenLevelSelection()
    {
        levelSelectionPanel.SetActive(true);
    }

    public void CloseLevelSelection()
    {
        levelSelectionPanel.SetActive(false);
    }
}
