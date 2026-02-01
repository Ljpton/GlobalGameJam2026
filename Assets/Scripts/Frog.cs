using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Frog : MonoBehaviour
{
    public static bool frogIsSelected = false;

    public float jumpHeight = 3.0f;
    public float duration = 0.75f;

    private bool isJumping = false;

    private Color[] randomColors =
    {
        new Color(0.376f, 0.949f, 0.2f),
        new Color(0.66f, 0.949f, 0.2f),
        new Color(0.2f, 0.949f, 0.567f),
        new Color(0.2f, 0.949f, 0.266f)
    };

    private Transform lastPos;
    Color lastColor;

    private SpriteRenderer spriteRenderer;

    public Sprite princeSprite;
    public Sprite notPrinceSprite;
    public Sprite idleSprite;
    public Sprite jumpingSprite;

    private void Awake()
    {
        Frog.frogIsSelected = false;

        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.color = randomColors[Random.Range(0, randomColors.Length)];
    }

    private void OnMouseDown()
    {
        if (LevelManager.Instance.levelWon) return;

        if (Frog.frogIsSelected) return;

        Frog.frogIsSelected = true;

        LevelManager.Instance.TogglePlaybackPanel(false);

        lastPos = transform;
        lastColor = spriteRenderer.color;
        spriteRenderer.sprite = idleSprite;

        LevelManager.Instance.StopAutoplay();

        StopAllCoroutines(); // Stop jumping stuff
        isJumping = false;

        // Play reveal animation
        transform.position = Vector3.zero;
        transform.localScale = new Vector3(2, 2, 2);

        Invoke(nameof(RevealStart), 2);
    }

    private void RevealStart()
    {
        if (CompareTag("Prince"))
        {
            AudioManager._instance.PlayPrinceWin();
            Debug.Log("You won!");

            spriteRenderer.sprite = princeSprite;
            spriteRenderer.color = Color.white;
            idleSprite = princeSprite;
            jumpingSprite = princeSprite;

            LevelManager.Instance.OnLevelWon();
        }
        else
        {
            AudioManager._instance.PlayFrogLose();
            Debug.Log("Wrong frog :(");
        }

        Invoke(nameof(RevealEnd), 2);
    }

    private void RevealEnd()
    {
        transform.position = lastPos.position;
        transform.localScale = Vector3.one;

        if(!CompareTag("Prince"))
        {
            spriteRenderer.color = lastColor;
            LevelManager.Instance.TogglePlaybackPanel(true);
        }
        else
        {
            LevelManager.Instance.ToggleNextLevelPanel(true);
        }

        Frog.frogIsSelected = false;
        LevelManager.Instance.StartAutoplay(); // For now
    }

    public void TeleportToPetal(Transform target)
    {
        transform.position = target.position;
    }

    public void JumpToPetal(Transform target)
    {
        if(isJumping)
        {
            StopAllCoroutines();   
        }

        StartCoroutine(JumpInArc(transform.position, target.position));
    }

    IEnumerator JumpInArc(Vector3 startPos, Vector3 endPos)
    {
        isJumping = true;

        if(spriteRenderer != null)
        {
            spriteRenderer.sprite = jumpingSprite;
        }
        
        float time = 0;

        while (time < duration)
        {
            time += Time.deltaTime;
            float linearT = time / duration;

            Vector3 currentPos = Vector3.Lerp(startPos, endPos, linearT);
            currentPos.y += Mathf.Sin(linearT * Mathf.PI) * jumpHeight;

            transform.position = currentPos;
            yield return null;
        }

        transform.position = endPos;
        isJumping = false;

        if(spriteRenderer != null)
        {
            spriteRenderer.sprite = idleSprite;
        }
    }
}
