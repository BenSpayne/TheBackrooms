using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ButtonToPlay : MonoBehaviour
{
    public Button playButton; // Assign the Play Button
    public CanvasGroup fadeCanvasGroup; // Assign the UI Canvas Group
    public CanvasGroup blackScreenCanvasGroup; // Assign the Black Screen Panel
    public Transform newCameraPosition; // Target camera position
    public Camera mainCamera; // Main Camera

    [Header("Camera Settings")]
    public bool isOrthographic = true;
    public float newOrthographicSize = 7f;
    public float newNearClip = 0.1f;
    public float newFarClip = 100f;
    public float fadeDuration = 1f; // How long the fade lasts

    private void Start()
    {
        playButton.onClick.AddListener(() => StartCoroutine(FadeOutMoveAndChangeCamera()));
        fadeCanvasGroup.alpha = 1; // Ensure UI starts visible
        blackScreenCanvasGroup.alpha = 0; // Ensure screen starts clear
    }

    private IEnumerator FadeOutMoveAndChangeCamera()
    {
        Debug.Log("Fade Out UI Starting...");

        playButton.interactable = false; // Disable button during fade
        fadeCanvasGroup.blocksRaycasts = true; // Prevent clicks during fade

        // Fade Out UI (1 → 0)
        yield return StartCoroutine(FadeTo(fadeCanvasGroup, 0, fadeDuration));

        Debug.Log("UI Hidden. Now Fading Screen to Black...");

        // Fade In Black Screen (0 → 1)
        yield return StartCoroutine(FadeTo(blackScreenCanvasGroup, 1, fadeDuration));

        Debug.Log("Black Screen Active. Moving Camera...");

        // Move the Camera **AFTER** the screen fades black
        mainCamera.transform.position = newCameraPosition.position;
        mainCamera.transform.rotation = newCameraPosition.rotation;

        // Update Camera Settings
        if (isOrthographic && mainCamera.orthographic)
        {
            mainCamera.orthographicSize = newOrthographicSize;
        }
        mainCamera.nearClipPlane = newNearClip;
        mainCamera.farClipPlane = newFarClip;

        // Wait for a short duration while the screen stays black
        yield return new WaitForSeconds(0.5f);

        Debug.Log("Fading Screen Back In...");

        // Fade Out Black Screen (1 → 0)
        yield return StartCoroutine(FadeTo(blackScreenCanvasGroup, 0, fadeDuration));

        fadeCanvasGroup.blocksRaycasts = false; // Re-enable interactions
        playButton.interactable = true; // Re-enable button
    }

    private IEnumerator FadeTo(CanvasGroup canvasGroup, float targetAlpha, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float timer = 0f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, timer / duration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha; // Ensure final value is set
        Debug.Log($"Fade Complete: New Alpha = {canvasGroup.alpha}");
    }
}