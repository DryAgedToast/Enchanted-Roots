using System.Collections.Generic;
using UnityEngine;
using TMPro;

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
        foreach (var value in predeterminedQueueValues)
        {
            GameObject item = Instantiate(queueItemPrefab, queueParent);
            item.GetComponent<QueueItem>().Setup(value);
        }
    }

    public void CheckLevelComplete()
    {
        bool queueEmpty = queueParent.childCount == 0;
        bool noInvasives = true;

        foreach (var obj in BSTManager.instance.nodeObjects.Values)
        {
            var behavior = obj.GetComponent<BSTNodeBehavior>();
            if (behavior != null && behavior.isInvasive)
            {
                noInvasives = false;
                break;
            }
        }

        if (queueEmpty && noInvasives)
        {
            Debug.Log("Level Complete!");
            // TODO: Transition to next level
        }
    }
}
