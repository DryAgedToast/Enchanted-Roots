using System.Collections.Generic;
using UnityEngine;

public class QueueManager : MonoBehaviour
{
    public static QueueManager instance;

    [SerializeField] private Transform queueParent;
    [SerializeField] private GameObject queueItemPrefab;
    [SerializeField] private List<int> predeterminedQueueValues;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Initially populate the queue.
        ResetQueue();
    }
    
    // Repopulate the queue with the predetermined list of values.
    public void ResetQueue()
    {
        // Destroy existing queue items.
        foreach (Transform child in queueParent)
        {
            Destroy(child.gameObject);
        }
        // Recreate queue items from the predetermined list.
        foreach (int value in predeterminedQueueValues)
        {
            GameObject item = Instantiate(queueItemPrefab, queueParent);
            item.GetComponent<QueueItem>().Setup(value);
        }
    }

    // Return true if there are no items in the queue.
    public bool QueueEmpty()
    {
        return queueParent.childCount == 0;
    }

    public void CheckLevelComplete()
    {
        bool queueEmpty = queueParent.childCount == 0;


        foreach (var obj in BSTManager.instance.nodeObjects.Values)
        {
            var behavior = obj.GetComponent<BSTNodeBehavior>();
            if (behavior != null && behavior.isInvasive)
            {

                break;
            }
        }

    // (Your existing CheckLevelComplete and other methods follow.)
}

}


