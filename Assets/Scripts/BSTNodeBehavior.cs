using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class BSTNodeBehavior : MonoBehaviour
{
    public int Value { get; private set; }
    private TMP_Text nodeText;
    private SpriteRenderer spriteRenderer;

    public bool isInvasive = false; // NEW: Flag to track if the node is invasive
    public Color normalColor = Color.white;
    public Color invasiveColor = Color.red;

    //this code is responsible for editing the TextMeshPro object on the node (displays node value), and making the invasive nodes red

    private void Awake()
    {
        nodeText = GetComponentInChildren<TMP_Text>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (spriteRenderer == null)
        {
            Debug.LogError("SpriteRenderer missing from BSTNodePrefab!");
        }
    }

    //method used to set the value of the node
    public void SetValue(int value)
    {
        Value = value;
        if (nodeText != null)
        {
            nodeText.text = value.ToString();
        }
    }

    //method used to set the invasive boolean
    public void SetInvasive(bool invasive)
    {
        isInvasive = invasive;

        if (spriteRenderer != null)
        {
            spriteRenderer.color = invasive ? invasiveColor : normalColor;
        }
    }

    private void OnMouseDown()
    {
        //delete node with pointer click
        BSTManager.instance.AttemptDeleteNode(this);
    }

}
