using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class QuestionTrigger : MonoBehaviour
{
    public GameObject questionBox;  // Assign the QuestionBox UI Panel
    public Animator questionBoxAnimator;  // Assign the Animator component
    public TextMeshProUGUI questionText;  // Assign the Question Text
    public Button correctAnswerButton;  // Assign the correct answer button
    public Button wrongAnswerButton;  // Assign the wrong answer button

    private bool playerInside = false;

    void Start()
    {
        questionBox.SetActive(false);  // Hide the question box initially

        // Add listeners to buttons
        correctAnswerButton.onClick.AddListener(CorrectAnswer);
        wrongAnswerButton.onClick.AddListener(WrongAnswer);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            ShowQuestion();
        }
    }

    void ShowQuestion()
    {
        questionBox.SetActive(true);
        questionBoxAnimator.Play("FadeIn"); // Play the fade-in animation
    }

    void CorrectAnswer()
    {
        questionBoxAnimator.SetTrigger("FadeOutTrigger"); // Play fade-out animation
        Invoke("DisableQuestionBox", 0.5f); // Give time for fade-out before disabling
    }

    void DisableQuestionBox()
    {
        questionBox.SetActive(false);
        playerInside = false;
    }

    void WrongAnswer()
    {
        Debug.Log("Wrong Answer! Try Again.");
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
        }
    }
}
