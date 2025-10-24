using UnityEngine;
using UnityEngine.SceneManagement;
using Backend;

public class LevelGoal : MonoBehaviour
{
    [SerializeField] private int thisLevelIndex = 1;
    [SerializeField] private string nextSceneName = "Level_02";
    [SerializeField] private int nextLevelIndex = 2;

    private async void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        GameDataService.Instance.SetCurrentLevel(nextLevelIndex);
        await GameDataService.Instance.SaveAsync();

        SceneManager.LoadScene(nextSceneName);
    }
}
