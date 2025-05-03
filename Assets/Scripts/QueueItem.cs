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

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.position = eventData.position;
    }

public void OnBeginDrag(PointerEventData eventData)
{
    canvasGroup.alpha = 0.5f; // Make it transparent
    canvasGroup.blocksRaycasts = false;

    foreach (var obj in BSTManager.instance.nodeObjects.Values)
    {
        var behavior = obj.GetComponent<BSTNodeBehavior>();
        behavior.ShowDropZones(true);
    }
}

public void OnEndDrag(PointerEventData eventData)
{
    canvasGroup.alpha = 1f; // Restore opacity
    canvasGroup.blocksRaycasts = true;

    foreach (var obj in BSTManager.instance.nodeObjects.Values)
    {
        var behavior = obj.GetComponent<BSTNodeBehavior>();
        behavior.ShowDropZones(false);
    }
}


    public int GetValue()
    {
        return value;
    }
}
