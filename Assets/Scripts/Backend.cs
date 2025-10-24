using UnityEngine;

public class BackendRoot : MonoBehaviour
{
    public static BackendRoot Instance;
    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject); // lo verás bajo el encabezado "DontDestroyOnLoad" al dar Play
    }
}
