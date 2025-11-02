using UnityEngine;
using UnityEngine.SceneManagement;
using System.Threading.Tasks;
using Backend;

[RequireComponent(typeof(Collider2D))]
public class LevelGoal : MonoBehaviour
{
    [Header("Requisito")]
    [SerializeField] private int requiredCoins = 3;     
    [Header("Siguiente nivel")]
    [SerializeField] private string nextSceneName = "Level_02";
    [SerializeField] private int    nextLevelIndex = 2;
    [SerializeField] private float  delayBeforeLoad = 0.4f; 

    [Header("Transición (opcional)")]
    [SerializeField] private GameObject fadePanel;      
    private int sessionCoins = 0;   
    private bool loading = false;

    void Awake()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
        var sr = GetComponent<SpriteRenderer>();
        if (sr) sr.enabled = false;
    }

    void OnEnable()  { Coin.OnCoinCollected += OnCoin; }
    void OnDisable() { Coin.OnCoinCollected -= OnCoin; }

    void OnCoin(int amount) => sessionCoins += amount;

    private async void OnTriggerEnter2D(Collider2D other)
    {
        if (loading || !other.CompareTag("Player")) return;

        int total = sessionCoins;
        try
        {
            // total = GameDataService.Instance.Coins;
            // total = GameDataService.Instance.GetCoins();
        } catch {}

        if (total < requiredCoins)
        {
            Debug.Log($"Faltan {requiredCoins - total} moneda(s) para abrir la meta.");
            return;
        }

        loading = true;
        if (fadePanel) fadePanel.SetActive(true);

        try {
            GameDataService.Instance?.SetCurrentLevel(nextLevelIndex);
            await GameDataService.Instance?.SaveAsync();
        } catch (System.Exception e) {
            Debug.LogWarning($"[LevelGoal] Error al guardar: {e.Message}");
        }

        if (delayBeforeLoad > 0f)
            await Task.Delay(Mathf.RoundToInt(delayBeforeLoad * 1000f));

        SceneManager.LoadScene(nextSceneName);
    }
}
