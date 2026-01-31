using NUnit.Framework;
using UnityEngine;

public class LevelSelection : MonoBehaviour
{
    public LevelData[] allLevels;

    public GameObject levelSelectionPanel;

    public void StartLevel(int levelIndex)
    {
        LevelManager.Instance.LoadLevel(allLevels[levelIndex]);

        CloseLevelSelection();

        LevelManager.Instance.InitLevel();
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
