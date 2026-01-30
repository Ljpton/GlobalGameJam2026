using UnityEngine;
using System.Collections;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class ArcJump : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;
    public float jumpHeight = 2.0f;
    public float duration = 1.0f;

    public Transform[] petals;

    private void Start()
    {
        startPoint = petals[0];
        endPoint = petals[0];
    }

    void Update()
    {
        if (Keyboard.current[Key.Space].wasPressedThisFrame)
        {
            List<Transform> tempPetals = new List<Transform>(petals);

            Debug.Log(tempPetals.Count);

            startPoint = endPoint;

            tempPetals.Remove(startPoint);

            endPoint = tempPetals[Random.Range(0, tempPetals.Count - 1)].transform;

            StartCoroutine(JumpInArc());
        }
    }

    IEnumerator JumpInArc()
    {
        float time = 0;
        Vector3 startPos = startPoint.position;
        Vector3 endPos = endPoint.position;

        while (time < duration)
        {
            time += Time.deltaTime;
            float linearT = time / duration; // 0 to 1

            // Horizontal linear interpolation
            Vector3 currentPos = Vector3.Lerp(startPos, endPos, linearT);

            // Add arc height using a parabola: height * 4 * t * (1 - t)
            currentPos.y += Mathf.Sin(linearT * Mathf.PI) * jumpHeight;

            transform.position = currentPos;
            yield return null;
        }

        transform.position = endPos; // Ensure precise end point
    }
}
