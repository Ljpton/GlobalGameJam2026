using UnityEngine;

public class SandboxPetal : MonoBehaviour
{
    public int petalIndex = 0;

    private void OnMouseDown()
    {
        SandboxManager.Instance.ClickOnPetal(petalIndex);
    }
}
