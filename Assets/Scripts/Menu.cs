using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject creditsPanel;
    public GameObject howToPlayPanel;

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void StartSandbox()
    {
        SceneManager.LoadScene("Sandbox");
    }

    public void OpenCredits()
    {
        creditsPanel.SetActive(true);
    }

    public void CloseCredits()
    {
        creditsPanel.SetActive(false);
    }

    public void OpenHowToPlay()
    {
        howToPlayPanel.SetActive(true);
    }

    public void CloseHowToPlay()
    {
        howToPlayPanel.SetActive(false);
    }

    public void StartMenu()
    {
        SceneManager.LoadScene("Menu");
    }
}
