using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MouseSensitivityManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider sensitivitySlider; // Assign the UI Slider in Inspector
    public TMP_Text sensitivityText; // Assign the TMP Text to display value

    [Header("Player Reference")]
    public PlayerMovement playerMovement; // Assign the PlayerMovement script

    private const string SensitivityPref = "MouseSensitivity"; // Key for saving sensitivity
    private const float minSensitivity = 0.5f; // New minimum sensitivity
    private const float maxSensitivity = 5f; // New max sensitivity (was 10, now 5)

    void Start()
    {
        // Load saved sensitivity or set default (50)
        float savedSliderValue = PlayerPrefs.GetFloat(SensitivityPref, 50f);
        sensitivitySlider.value = savedSliderValue;
        UpdateSensitivity(savedSliderValue);

        // Add listener to detect slider changes
        sensitivitySlider.onValueChanged.AddListener(UpdateSensitivity);
    }

    public void UpdateSensitivity(float sliderValue)
    {
        // Convert slider value (1-100) to sensitivity range (0.5 - 5)
        float mappedSensitivity = Mathf.Lerp(minSensitivity, maxSensitivity, sliderValue / 100f);

        // Update player movement sensitivity
        if (playerMovement != null)
        {
            playerMovement.mouseSensitivity = mappedSensitivity;
        }

        // Update displayed text
        sensitivityText.text = sliderValue.ToString("F1"); // Show 1 decimal place

        // Save sensitivity setting
        PlayerPrefs.SetFloat(SensitivityPref, sliderValue);
        PlayerPrefs.Save();
    }
}