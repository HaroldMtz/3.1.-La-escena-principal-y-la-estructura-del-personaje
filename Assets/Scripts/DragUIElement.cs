using UnityEngine;
using UnityEngine.EventSystems;

[DisallowMultipleComponent]
[RequireComponent(typeof(RectTransform))]
public class DragUIElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    RectTransform rt;
    Canvas canvas;
    CanvasGroup group;

    void Awake()
    {
        rt = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        group = GetComponent<CanvasGroup>();
        if (group == null) group = gameObject.AddComponent<CanvasGroup>(); // <-- evita el error
    }

    public void OnBeginDrag(PointerEventData e)
    {
        if (group != null) group.blocksRaycasts = false; // facilita soltar sobre otros
    }

    public void OnDrag(PointerEventData e)
    {
        if (canvas == null) return;
        rt.anchoredPosition += e.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData e)
    {
        if (group != null) group.blocksRaycasts = true;
    }
}
