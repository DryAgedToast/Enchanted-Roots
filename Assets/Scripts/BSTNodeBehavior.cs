using UnityEngine;
using UnityEngine.UI;

public class BSTNodeBehavior : MonoBehaviour
{
    public int Value { get; private set; }
    private Text nodeText;

    private void Awake()
    {
        nodeText = GetComponentInChildren<Text>(); // Find the Text component
    }

    public void SetValue(int value)
    {
        Value = value;
        nodeText.text = value.ToString(); // Update text dynamically
    }
}
