using UnityEngine;
using UnityEngine.SceneManagement;
public class SceneJump : MonoBehaviour
{
    public string nextSceneName; // exacto, sin .unity
    public void GoNext(){ if(!string.IsNullOrEmpty(nextSceneName)) SceneManager.LoadScene(nextSceneName); }
}
