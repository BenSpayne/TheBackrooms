using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    public Camera cam;
    public NavMeshAgent agent;
    public GameObject questionBox; // Assign the Question UI Panel

    public Transform interactionCameraPosition; // Assign this in the Inspector
    public Transform defaultCameraPosition; // Assign this in the Inspector
    public float interactionOrthoSize = 3f; // Zoom-in size
    public float defaultOrthoSize = 5f; // Default zoom size
    public float cameraMoveSpeed = 2f; // Speed of camera movement
    public float stoppingDistanceOffset = 0.5f; // Buffer to trigger interaction sooner

    private bool isInteracting = false;
    private Transform currentTarget;

    void Update()
    {
        // Prevent movement when the Question UI is active
        if (questionBox != null && questionBox.activeSelf) return;

        // Prevent movement if clicking on UI elements
        if (EventSystem.current.IsPointerOverGameObject()) return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Move the agent
                agent.SetDestination(hit.point);

                // If the object has an interaction tag, set it as the target
                if (hit.collider.CompareTag("Interactable"))
                {
                    currentTarget = hit.collider.transform;
                }
            }
        }

        // Check if player has reached the target
        if (currentTarget != null && !isInteracting)
        {
            float distance = Vector3.Distance(transform.position, currentTarget.position);
            if (distance <= agent.stoppingDistance + stoppingDistanceOffset)
            {
                StartInteraction();
            }
        }
    }

    void StartInteraction()
    {
        isInteracting = true;
        StopAllCoroutines();
        StartCoroutine(MoveCamera(interactionCameraPosition.position, interactionOrthoSize));
    }

    public void EndInteraction()
    {
        isInteracting = false;
        currentTarget = null;
        StopAllCoroutines();
        StartCoroutine(MoveCamera(defaultCameraPosition.position, defaultOrthoSize));
    }

    IEnumerator MoveCamera(Vector3 targetPosition, float targetSize)
    {
        while (Vector3.Distance(cam.transform.position, targetPosition) > 0.05f || 
               Mathf.Abs(cam.orthographicSize - targetSize) > 0.05f)
        {
            cam.transform.position = Vector3.Lerp(cam.transform.position, targetPosition, Time.deltaTime * cameraMoveSpeed);
            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime * cameraMoveSpeed);
            yield return null;
        }

        // Ensure final values are precise
        cam.transform.position = targetPosition;
        cam.orthographicSize = targetSize;
    }
}
