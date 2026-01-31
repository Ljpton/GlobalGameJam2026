using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;

public class ArcJump : MonoBehaviour
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

        while (time < duration)
        {
            time += Time.deltaTime;
            float linearT = time / duration;

            Vector3 currentPos = Vector3.Lerp(startPos, endPos, linearT);
            currentPos.y += Mathf.Sin(linearT * Mathf.PI) * jumpHeight;

            float angle = linearT * 360f * rotationDirection;
            transform.rotation = startRotation * Quaternion.Euler(0, 0, angle);

            transform.position = currentPos;
            yield return null;
        }

        transform.position = endPos;
        transform.rotation = startRotation;
        isJumping = false;
    }
}
