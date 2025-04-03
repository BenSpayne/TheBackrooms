using UnityEngine;

public class ExitGame : MonoBehaviour
{
    // This method can be assigned to a UI Button's OnClick event
    public void QuitGame()
    {
        Debug.Log("Exiting Game..."); // Log to confirm button press (only in the editor)
        Application.Quit(); // Closes the built application

        // Exit play mode if running inside the Unity Editor
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
