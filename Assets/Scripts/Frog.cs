using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Frog : MonoBehaviour
{
    public float jumpHeight = 3.0f;
    public float duration = 0.75f;

    private bool isJumping = false;

    private Transform lastPos;
    Color lastColor;

    private Animator animator;
    private SpriteRenderer spriteRenderer;

    public Sprite princeSprite;
    public Sprite notPrinceSprite;
    public Sprite idleSprite;
    public Sprite jumpingSprite;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {

    }

    private void OnMouseDown()
    {
        if (LevelManager.Instance.levelWon) return;

        Debug.Log("Frog clicked!");

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
            Debug.Log("You won!");

            spriteRenderer.sprite = princeSprite;
            spriteRenderer.color = Color.white;
            idleSprite = princeSprite;
            jumpingSprite = princeSprite;

            LevelManager.Instance.OnLevelWon();

            // Play full song and then finish level
        }
        else
        {
            Debug.Log("Wrong frog :(");
            // Play frog sound
            // Or turn into random sprite
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
