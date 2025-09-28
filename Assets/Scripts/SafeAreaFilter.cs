using UnityEngine;

[ExecuteAlways]
public class SafeAreaFitter : MonoBehaviour
{
    RectTransform rt;
    Rect lastSafe;

    void OnEnable() { rt = GetComponent<RectTransform>(); Apply(); }
    void Update() { if (Screen.safeArea != lastSafe) Apply(); }

    void Apply()
    {
        lastSafe = Screen.safeArea;
        var min = lastSafe.position;
        var max = lastSafe.position + lastSafe.size;

        min.x /= Screen.width;  min.y /= Screen.height;
        max.x /= Screen.width;  max.y /= Screen.height;

        rt.anchorMin = min;
        rt.anchorMax = max;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
    }
}
