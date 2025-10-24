using System.Threading.Tasks;
using UnityEngine;
using Backend;

public class GameManager : MonoBehaviour
{
    [SerializeField] private int levelIndexThisScene = 1;

    private async void Start()
    {
        if (GameDataService.Instance == null)
        {
            Debug.LogError("Agrega el prefab con GameDataService a una escena inicial/persistente.");
            return;
        }

        await GameDataService.Instance.Initialize();
        await GameDataService.Instance.LoadAsync();

        GameDataService.Instance.SetCurrentLevel(levelIndexThisScene);
        Debug.Log($"[GM] Coins={GameDataService.Instance.GetTotalCoins()} Level={GameDataService.Instance.GetCurrentLevel()}");
    }
}
