using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Cambia por el nombre EXACTO de tu escena de juego (Level1, SampleScene, etc.)
    [SerializeField] private string gameplaySceneName = "Level1";

    public void PlayGame()
    {
        // Carga la escena principal del juego
        SceneManager.LoadScene(gameplaySceneName);
    }

    public void QuitGame()
    {
        // Funciona en build. En el Editor no sale; puedes simular con UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
