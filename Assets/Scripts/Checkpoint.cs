using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    [Tooltip("Si está activo, cargará una posición guardada al iniciar la escena.")]
    public bool loadOnStart = false;

    Collider2D col;

    void Reset()
    {
        col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    void Awake()
    {
        col = GetComponent<Collider2D>();
        if (col) col.isTrigger = true;
        Debug.Log("[Checkpoint] Awake en objeto: " + name + " (componente habilitado=" + enabled + ")");
    }

    void Start()
    {
        if (!loadOnStart) {
            Debug.Log("[Checkpoint] loadOnStart = false (no cargará al inicio).");
            return;
        }

        var playerGO = GameObject.FindGameObjectWithTag("Player");
        if (!playerGO) { Debug.LogWarning("[Checkpoint] No encontré objeto con Tag=Player."); return; }

        if (SaveManager.TryLoadPosition(out var pos))
        {
            playerGO.transform.position = new Vector3(pos.x, pos.y, playerGO.transform.position.z);
            Debug.Log("[Checkpoint] CARGADO al inicio en: " + pos);
        }
        else
        {
            Debug.Log("[Checkpoint] No hay guardado previo al iniciar.");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("[Checkpoint] Trigger con: " + other.name + " (tag=" + other.tag + ")");
        if (!other.CompareTag("Player")) return;

        SaveManager.SavePosition(other.transform.position);
        Debug.Log("[Checkpoint] GUARDADO en: " + other.transform.position);
    }
}
