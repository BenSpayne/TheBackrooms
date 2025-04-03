using System.Collections;
using UnityEngine;

public class CameraTeleport : MonoBehaviour
{
    public Transform newCameraPosition; // Drag the target camera position
    public Camera mainCamera; // Assign the main camera
    public bool isOrthographic = true;
    public float newOrthographicSize = 7f;
    public float transitionDuration = 1f; // Time for smooth movement

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Check if the player enters
        {
            StartCoroutine(MoveCamera());
        }
    }

    private IEnumerator MoveCamera()
    {
        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;
        float startOrthoSize = mainCamera.orthographicSize;
        float time = 0f;

        while (time < transitionDuration)
        {
            time += Time.deltaTime;
            float t = time / transitionDuration;

            // Smoothly interpolate position & rotation
            mainCamera.transform.position = Vector3.Lerp(startPos, newCameraPosition.position, t);
            mainCamera.transform.rotation = Quaternion.Lerp(startRot, newCameraPosition.rotation, t);

            // Adjust Orthographic Size if enabled
            if (isOrthographic && mainCamera.orthographic)
            {
                mainCamera.orthographicSize = Mathf.Lerp(startOrthoSize, newOrthographicSize, t);
            }

            yield return null;
        }

        // Ensure final values are set
        mainCamera.transform.position = newCameraPosition.position;
        mainCamera.transform.rotation = newCameraPosition.rotation;
        if (isOrthographic && mainCamera.orthographic)
        {
            mainCamera.orthographicSize = newOrthographicSize;
        }
    }
}
