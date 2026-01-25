using UnityEngine;
using UnityEngine.EventSystems;

public class DragImage : MonoBehaviour, IDragHandler
{
    private RectTransform rectTransform;
    private bool isDragging = false;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData eventData)
    {
        var move = FindObjectOfType<ReaderScript>().move;
        if (move && rectTransform != null && isDragging)
        {
            rectTransform.anchoredPosition += eventData.delta;
        }
    }
}
