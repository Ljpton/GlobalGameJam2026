using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class Frog : MonoBehaviour
{
    public float jumpHeight = 3.0f;
    public float duration = 0.75f;

    public Transform[] petals;
    public int offset = 0;

    private int currentStep = 0;
    private bool isJumping = false;

    private void Start()
    {
        int startIndex = (offset + currentStep * 7) % 12;
        transform.position = petals[startIndex].position;
    }

    void Update()
    {
        if (Keyboard.current[Key.Space].wasPressedThisFrame && !isJumping)
        {
            int fromIndex = (offset + currentStep * 7) % 12;
            currentStep++;
            int toIndex = (offset + currentStep * 7) % 12;

            StartCoroutine(JumpInArc(petals[fromIndex].position, petals[toIndex].position));
        }
    }



    IEnumerator JumpInArc(Vector3 startPos, Vector3 endPos)
    {
        isJumping = true;
        float time = 0;

        float rotationDirection = endPos.x < startPos.x ? 1f : -1f;
        Quaternion startRotation = transform.rotation;
        Vector3 originalScale = transform.localScale;

        while (time < duration)
        {
            time += Time.deltaTime;
            float linearT = time / duration;

            Vector3 currentPos = Vector3.Lerp(startPos, endPos, linearT);
            currentPos.y += Mathf.Sin(linearT * Mathf.PI) * jumpHeight;

            float spinT = Mathf.Pow(linearT, 0.6f);
            float angle = spinT * 720f * rotationDirection;
            float wobble = Mathf.Sin(linearT * Mathf.PI * 6f) * 15f * (1f - linearT);
            transform.rotation = startRotation * Quaternion.Euler(0, 0, angle + wobble);

            float squashStretch;
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
            );

            transform.position = currentPos;
            yield return null;
        }

        transform.position = endPos;
        transform.rotation = startRotation;
        transform.localScale = originalScale;
        isJumping = false;
    }
}
