using UnityEngine;

public class LinkOpener : MonoBehaviour
{
    [SerializeField] private string url;

    public void OpenURL()
    {
        if (!string.IsNullOrWhiteSpace(url))
            Application.OpenURL(url);
        else
            Debug.LogWarning($"URL no asignada en {name}");
    }
}
