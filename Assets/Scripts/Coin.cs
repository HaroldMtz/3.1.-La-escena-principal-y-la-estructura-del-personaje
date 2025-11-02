using UnityEngine;
using Backend;
using System;

public class Coin : MonoBehaviour
{
    [SerializeField] private int amount = 1;
    public static event Action<int> OnCoinCollected;

    private void Awake()
    {
        var col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        try { GameDataService.Instance?.AddCoins(amount); } catch {}
        OnCoinCollected?.Invoke(amount);                   // <- avisar
        Destroy(gameObject);
    }
}
