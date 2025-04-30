using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class QueueItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private TMP_Text valueText;

    private int value;
    private Transform originalParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        valueText = GetComponentInChildren<TMP_Text>();
    }

    public void Setup(int v)
    {
        value = v;
        if (valueText != null)
            valueText.text = v.ToString();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
        originalParent = transform.parent;
        transform.SetParent(transform.root); // Move to top layer
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
        transform.SetParent(originalParent);
        transform.localPosition = Vector3.zero; // Snap back if not dropped
    }

    public int GetValue()
    {
        return value;
    }
}
