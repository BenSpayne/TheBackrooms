using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameStartManager : MonoBehaviour
{
    [Header("Game Objects")]
    public GameObject virtualCamera; // Assign your virtual camera (menu camera)
    public GameObject player; // Assign your player GameObject
    public GameObject playerCamera; // Assign the player camera (enabled when the game starts)

    [Header("UI Elements")]
    public GameObject menuUI; // Assign your menu UI
    public GameObject gameUI; // Assign your game UI
    public Button startButton; // Assign the Start button

    [Header("Lighting Settings")]
    public LightAudioPair[] lightAudioPairs; // Array of Light & AudioSource pairs
    public float lightTurnOffDelay = 0.5f; // Delay between each light turning off
    public Animator flickerAnimator; // Assign the flickering light's animator
    public float darknessDelay = 2f; // Time the player stays in darkness before the transition happens

    [Header("Player Camera Animation")]
    public Animator playerCameraAnimator; // Animator for the player camera (wake-up effect)
    public string wakeUpAnimationTrigger = "WakeUp"; // Name of the trigger for the wake-up animation

    void Start()
    {
        startButton.onClick.AddListener(StartGame);
    }

    public void StartGame()
    {
        // Disable cursor immediately
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // Prevent button spam by disabling the start button
        startButton.interactable = false;

        StartCoroutine(GameStartSequence());
    }

    private IEnumerator GameStartSequence()
    {
        // Disable flickering animation
        if (flickerAnimator != null)
        {
            flickerAnimator.enabled = false;
        }

        // Turn off hallway lights and play corresponding audio
        foreach (LightAudioPair lightAudioPair in lightAudioPairs)
        {
            // Turn off the light and stop the emission sound
            lightAudioPair.TurnOffLight();

            // Wait a short moment before playing the turn-off sound
            yield return new WaitForSeconds(0.1f);

            // Play the light turn-off sound from the corresponding audio source
            lightAudioPair.PlayTurnOffSound();

            yield return new WaitForSeconds(lightTurnOffDelay); // Wait before turning off the next light
        }

        if (playerCameraAnimator != null)
        {
            playerCameraAnimator.SetTrigger(wakeUpAnimationTrigger); // Trigger the animation
        }

        // Wait for a bit to keep the player in darkness before the transition happens
        yield return new WaitForSeconds(darknessDelay);

        // Disable the virtual camera and menu UI
        virtualCamera.SetActive(false);
        menuUI.SetActive(false);

        // Enable the player and game UI
        player.SetActive(true);
        gameUI.SetActive(true);

        // Enable the player camera
        playerCamera.SetActive(true);
    }
}

[System.Serializable]
public class LightAudioPair
{
    public GameObject light; // The light GameObject to turn off
    public AudioSource audioSource; // The corresponding audio source for the light
    public AudioClip turnOffSound; // The sound to play when the light turns off

    // Stops the light emission sound when turning off the light
    public void TurnOffLight()
    {
        if (light != null)
        {
            light.SetActive(false); // Disable the light

            if (audioSource != null)
            {
                audioSource.Stop(); // Stop any ongoing emission sound
            }
        }
    }

    // Plays the light turn-off sound
    public void PlayTurnOffSound()
    {
        if (audioSource != null && turnOffSound != null)
        {
            audioSource.PlayOneShot(turnOffSound);
        }
    }
}

