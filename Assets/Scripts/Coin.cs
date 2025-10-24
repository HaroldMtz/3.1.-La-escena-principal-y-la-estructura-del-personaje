using UnityEngine;
using Backend; // <-- para ver GameDataService

public class Coin : MonoBehaviour
{
    [SerializeField] private int amount = 1;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        GameDataService.Instance.AddCoins(amount);
        Destroy(gameObject);
    }
}
