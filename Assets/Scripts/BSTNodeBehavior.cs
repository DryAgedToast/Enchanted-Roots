using UnityEngine;
using TMPro;

public class BSTNodeBehavior : MonoBehaviour
{
    //this code is responsible for editing the TextMeshPro object on the node (displays node value), and not really anything more than that
    
    public int Value { get; private set; }
    private TMP_Text nodeText;

    private void Awake()
    {
        nodeText = GetComponentInChildren<TMP_Text>(); // Find TextMeshPro text component
        if (nodeText == null)
        {
            Debug.LogError("TMP_Text component not found in BSTNodePrefab!");
        }
    }

    public void SetValue(int value)
    {
        Value = value;
        if (nodeText != null)
        {
            nodeText.text = value.ToString(); // Update text to display node value
        }
        else
        {
            Debug.LogError("nodeText is null in BSTNodeBehavior! Check prefab setup.");
        }
    }
}
