using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    [SerializeField] private string mainMenuScene = "MainMenu";

    public void BackToMainMenu()
    {
        Time.timeScale = 1f; // por si estabas en pausa
        SceneManager.LoadScene(mainMenuScene);
    }

    public void QuitDirect()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
