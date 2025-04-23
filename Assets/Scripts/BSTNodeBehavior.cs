using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class BSTNodeBehavior : MonoBehaviour
{
    public int Value { get; private set; }
    private TMP_Text nodeText;
    private SpriteRenderer spriteRenderer;

    public bool isInvasive = false;
    public Color normalColor = Color.white;
    public Color invasiveColor = Color.red;

    // NEW: Reference to another object whose color should be changed
    public GameObject targetObjectToColor;

    private void Awake()
    {
        nodeText = GetComponentInChildren<TMP_Text>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer missing from BSTNodePrefab!");
        }
    }

    public void SetValue(int value)
    {
        Value = value;
        if (nodeText != null)
        {
            nodeText.text = value.ToString();
        }
    }

    public void SetInvasive(bool invasive)
    {
        isInvasive = invasive;

        // Change color of another object's SpriteRenderer instead of this one
        if (targetObjectToColor != null)
        {
            SpriteRenderer targetRenderer = targetObjectToColor.GetComponent<SpriteRenderer>();
            if (targetRenderer != null)
            {
                targetRenderer.color = invasive ? invasiveColor : normalColor;
            }
            else
            {
                Debug.LogWarning("Target object does not have a SpriteRenderer.");
            }
        }
        else
        {
            Debug.LogWarning("No target object assigned to color.");
        }
    }

    private void OnMouseDown()
    {
        BSTManager.instance.AttemptDeleteNode(this);
    }
}
