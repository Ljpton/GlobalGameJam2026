using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using Unity.VisualScripting;

public class Frog : MonoBehaviour
{
    public float jumpHeight = 3.0f;
    public float duration = 0.75f;

    private int currentStep = 0;
    private bool isJumping = false;

    private Transform lastPos;

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
        Debug.Log("Frog clicked!");

        lastPos = transform;
        spriteRenderer.sprite = idleSprite;

        LevelManager.Instance.StopAutoplay();

        StopAllCoroutines(); // Stop jumping stuff
        isJumping = false;

        // Play reveal animation
        animator.enabled = true;
        animator.SetBool("Reveal", true);

        Invoke(nameof(RevealSound), 3);
    }

    private void RevealSound()
    {
        // Play kissing sound

        Invoke(nameof(RevealImage), 1);
    }

    private void RevealImage()
    {
        if (gameObject.CompareTag("Prince"))
        {
            Debug.Log("You won!");

            spriteRenderer.sprite = princeSprite;

            // Play full song and then finish level
        }
        else
        {
            Debug.Log("Wrong frog :(");
            // Play frog sound
            // Or turn into random sprite

            Invoke(nameof(RevealEnd), 2);
        }
    }

    private void RevealEnd()
    {
        animator.SetBool("Reveal", false);
        animator.enabled = false;

        transform.position = lastPos.position;

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
            Debug.Log("Jump call was ignored because frog is already jumping.");
            return;
        }

        Debug.Log(target.name + "aaaa");

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

        // float rotationDirection = endPos.x < startPos.x ? 1f : -1f;
        // Quaternion startRotation = transform.rotation;
        Vector3 originalScale = transform.localScale;

        while (time < duration)
        {
            time += Time.deltaTime;
            float linearT = time / duration;

            Vector3 currentPos = Vector3.Lerp(startPos, endPos, linearT);
            currentPos.y += Mathf.Sin(linearT * Mathf.PI) * jumpHeight;

            // float spinT = Mathf.Pow(linearT, 0.6f);
            // float angle = spinT * 720f * rotationDirection;
            // float wobble = Mathf.Sin(linearT * Mathf.PI * 6f) * 15f * (1f - linearT);
            // transform.rotation = startRotation * Quaternion.Euler(0, 0, angle + wobble);

            /*float squashStretch;
            if (linearT < 0.15f)
            {
                float t = linearT / 0.15f;
                squashStretch = Mathf.Lerp(1f, 0.5f, t);
            }
            else if (linearT < 0.5f)
            {
                float t = (linearT - 0.15f) / 0.35f;
                squashStretch = Mathf.Lerp(0.5f, 1.4f, t);
            }
            else if (linearT < 0.85f)
            {
                float t = (linearT - 0.5f) / 0.35f;
                squashStretch = Mathf.Lerp(1.4f, 0.6f, t);
            }
            else
            {
                float t = (linearT - 0.85f) / 0.15f;
                squashStretch = Mathf.Lerp(0.6f, 1f, t);
            }

            transform.localScale = new Vector3(
                originalScale.x / squashStretch,
                originalScale.y * squashStretch,
                originalScale.z
            );*/

            transform.position = currentPos;
            yield return null;
        }

        transform.position = endPos;
        // transform.rotation = startRotation;
        transform.localScale = originalScale;
        isJumping = false;
        if(spriteRenderer != null)
        {
            spriteRenderer.sprite = idleSprite;
        }
    }
}
