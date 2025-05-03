using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class DropTarget : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    public BSTNodeBehavior parentNode;
    public bool isLeft;

    private SpriteRenderer indicator;

    private void Awake()
    {
        indicator = GetComponent<SpriteRenderer>();
        indicator.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag?.GetComponent<QueueItem>() != null)
            indicator.enabled = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        indicator.enabled = false;
    }

public void OnDrop(PointerEventData eventData)
{
    var draggedItem = eventData.pointerDrag?.GetComponent<QueueItem>();
    if (draggedItem == null || parentNode == null) return;

    // ✅ Prevent overwriting an existing child
    if ((isLeft && parentNode.leftChild != null) || (!isLeft && parentNode.rightChild != null))
    {
        Debug.LogWarning("Child already exists in this position.");
        return;
    }

    int value = draggedItem.GetValue();
    GameObject inserted = BSTManager.instance.InsertAt(parentNode, value, isLeft);
    var behavior = inserted.GetComponent<BSTNodeBehavior>();
    behavior.SetInvasive(Random.value < 0.4f);

    Destroy(draggedItem.gameObject);
    QueueManager.instance.CheckLevelComplete();
    parentNode.ShowDropZones(false);
}

}
