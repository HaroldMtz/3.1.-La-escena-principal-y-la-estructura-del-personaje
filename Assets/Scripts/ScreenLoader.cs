using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] string[] orderedScenes;

    public void LoadScene(string sceneName) => SceneManager.LoadScene(sceneName);

    public void LoadNext()
    {
        string current = SceneManager.GetActiveScene().name;
        int idx = System.Array.IndexOf(orderedScenes, current);
        if (idx >= 0 && idx < orderedScenes.Length - 1)
            SceneManager.LoadScene(orderedScenes[idx + 1]);
    }

    public void LoadPrev()
    {
        string current = SceneManager.GetActiveScene().name;
        int idx = System.Array.IndexOf(orderedScenes, current);
        if (idx > 0)
            SceneManager.LoadScene(orderedScenes[idx - 1]);
    }

    public void QuitApp() => Application.Quit();
}
