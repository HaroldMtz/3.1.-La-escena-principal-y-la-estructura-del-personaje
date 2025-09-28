using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class IconHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Refs")]
    [SerializeField] Image target;            // deja vacío y se autollenará
    [Header("Colors")]
    [SerializeField] Color normalColor = Color.white;
    [SerializeField] Color hoverColor = new Color(0.82f, 0.91f, 1f, 1f); // azulito
    [Header("Scale")]
    [SerializeField, Range(1f, 1.2f)] float hoverScale = 1.08f;
    [SerializeField, Range(0.01f, 0.5f)] float animDuration = 0.12f;

    Vector3 _startScale;
    Coroutine _routine;

    void Reset() { target = GetComponent<Image>(); }
    void Awake()
    {
        if (!target) target = GetComponent<Image>();
        _startScale = transform.localScale;
        ApplyInstant(normalColor, _startScale);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        StartAnim(hoverColor, _startScale * hoverScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        StartAnim(normalColor, _startScale);
    }

    void StartAnim(Color toColor, Vector3 toScale)
    {
        if (_routine != null) StopCoroutine(_routine);
        _routine = StartCoroutine(Animate(toColor, toScale));
    }

    IEnumerator Animate(Color toColor, Vector3 toScale)
    {
        Color fromColor = target ? target.color : Color.white;
        Vector3 fromScale = transform.localScale;
        float t = 0f;
        while (t < animDuration)
        {
            t += Time.unscaledDeltaTime; // UI suave aunque haya pausas
            float k = Mathf.Clamp01(t / animDuration);
            if (target) target.color = Color.Lerp(fromColor, toColor, k);
            transform.localScale = Vector3.Lerp(fromScale, toScale, k);
            yield return null;
        }
        ApplyInstant(toColor, toScale);
    }

    void ApplyInstant(Color c, Vector3 s)
    {
        if (target) target.color = c;
        transform.localScale = s;
    }
}
