using UnityEngine;
using TMPro; // Import TextMeshPro namespace

public class BSTNodeBehavior : MonoBehaviour
{
    public int Value { get; private set; }
    private TMP_Text nodeText; // Use TMP_Text instead of Text

    private void Awake()
    {
        nodeText = GetComponentInChildren<TMP_Text>(); // Get the TextMeshPro component
        if (nodeText == null)
        {
            Debug.LogError("TMP_Text component not found");
        }
    }

    public void SetValue(int value)
    {
        Value = value;
        if (nodeText != null)
        {
            nodeText.text = value.ToString(); // Update the text correctly
        }
        else
        {
            Debug.LogError("nodeText is null in BSTNodeBehavior");
        }
    }
}
