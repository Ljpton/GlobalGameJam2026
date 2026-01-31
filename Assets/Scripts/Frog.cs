using UnityEngine;
using System.Collections;

public class Frog : MonoBehaviour
{
    public float jumpHeight = 3.0f;
    public float duration = 0.75f;

    private Transform[] majorPetals;
    private Transform[] minorPetals;
    
    private int[] path;
    private int currentPathIndex = 0;
    private bool isJumping = false;

    public bool IsJumping => isJumping;

    public void Initialize(int[] assignedPath, Transform[] major, Transform[] minor)
    {
        path = assignedPath;
        majorPetals = major;
        minorPetals = minor;
        currentPathIndex = 0;

        if (path != null && path.Length > 0)
        {
            Transform startPetal = GetPetalTransform(path[0]);
            if (startPetal != null)
            {
                transform.position = startPetal.position;
            }
        }
    }

    private Transform GetPetalTransform(int noteIndex)
    {
        if (noteIndex >= 12)
        {
            int minorIndex = noteIndex - 12;
            if (minorPetals != null && minorIndex < minorPetals.Length)
            {
                return minorPetals[minorIndex];
            }
        }
        else
        {
            if (majorPetals != null && noteIndex < majorPetals.Length)
            {
                return majorPetals[noteIndex];
            }
        }
        return null;
    }

    public void JumpToPetal(int pathIndex)
    {
        if (isJumping || path == null) return;

        int fromNoteIndex = path[currentPathIndex];
        currentPathIndex = pathIndex;
        int toNoteIndex = path[currentPathIndex];

        Transform fromPetal = GetPetalTransform(fromNoteIndex);
        Transform toPetal = GetPetalTransform(toNoteIndex);

        if (fromPetal != null && toPetal != null)
        {
            StartCoroutine(JumpInArc(fromPetal.position, toPetal.position));
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
