using UnityEngine;

public class FrogMenu : MonoBehaviour
{
    private void OnMouseDown()
    {
        if(AudioManager._instance != null)
        {
            AudioManager._instance.PlayFrogLose();
        }
    }
}
