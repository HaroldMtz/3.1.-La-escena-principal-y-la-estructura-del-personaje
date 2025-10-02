using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class KillZone : MonoBehaviour
{
    [Header("Respawn")]
    public bool useCheckpointIfAny = true;                // usa SaveManager si hay guardado
    public Transform respawnPoint;                        // opcional: un vacío en escena
    public Vector3 fallbackSpawn = new Vector3(0, -1, 0); // si no hay guardado ni respawnPoint

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        // 1) Detén velocidad
        var rb = other.attachedRigidbody;
        if (rb) rb.linearVelocity = Vector2.zero;

        // 2) Decide destino
        Vector3 target = respawnPoint ? respawnPoint.position : fallbackSpawn;

        if (useCheckpointIfAny && SaveManager.TryLoadPosition(out Vector2 pos2D))
            target = new Vector3(pos2D.x, pos2D.y, other.transform.position.z);

        // 3) Teletransporta
        other.transform.position = target;

        // 4) Log para confirmar
        Debug.Log($"[KillZone] TRIGGER con {other.name} → respawn en {target}");
    }
}
