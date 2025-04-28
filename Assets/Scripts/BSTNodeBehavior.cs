using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class BSTNodeBehavior : MonoBehaviour, IPointerClickHandler
{
    public int Value { get; private set; }
    private TMP_Text nodeText;
    private SpriteRenderer spriteRenderer;

    public bool isInvasive = false;
    public Color normalColor = Color.white;
    public Color invasiveColor = Color.red;

    public GameObject targetObjectToColor; // Object to color (external circle sprite)

    // Line connections
    private LineRenderer leftLine;
    private LineRenderer rightLine;
    public Transform leftChild;
    public Transform rightChild;

    private void Awake()
{
    nodeText = GetComponentInChildren<TMP_Text>();
    spriteRenderer = GetComponent<SpriteRenderer>();

    // Find LineRenderers on child objects
    Transform leftLineObj = transform.Find("LeftLine");
    Transform rightLineObj = transform.Find("RightLine");

    if (leftLineObj != null && rightLineObj != null)
    {
        leftLine = leftLineObj.GetComponent<LineRenderer>();
        rightLine = rightLineObj.GetComponent<LineRenderer>();
    }
    else
    {
        Debug.LogError("LeftLine or RightLine child object is missing on BSTNodePrefab!");
    }

    SetupLineRenderer(leftLine, Color.gray);
    SetupLineRenderer(rightLine, Color.gray);
}

    private void SetupLineRenderer(LineRenderer lr, Color color)
    {
        if (lr == null) return;

        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startWidth = 0.05f;
        lr.endWidth = 0.05f;
        lr.positionCount = 2;
        lr.useWorldSpace = true;
        lr.startColor = color;
        lr.endColor = color;
        lr.enabled = false;
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

        if (targetObjectToColor != null)
        {
            SpriteRenderer targetRenderer = targetObjectToColor.GetComponent<SpriteRenderer>();
            if (targetRenderer != null)
            {
                targetRenderer.color = invasive ? invasiveColor : normalColor;
            }
            else
            {
                Debug.LogWarning("Target object to color missing SpriteRenderer!");
            }
        }
        else
        {
            Debug.LogWarning("No target object assigned to color.");
        }
    }

    public void ConnectChild(BSTNodeBehavior child, bool isLeft)
    {
        if (child == null) return;

        if (isLeft)
        {
            leftChild = child.transform;
            leftLine.enabled = true;
        }
        else
        {
            rightChild = child.transform;
            rightLine.enabled = true;
        }
    }

    private void Update()
{
    if (leftChild != null && leftLine != null)
    {
        leftLine.SetPosition(0, transform.position);
        leftLine.SetPosition(1, leftChild.position);
    }

    if (rightChild != null && rightLine != null)
    {
        rightLine.SetPosition(0, transform.position);
        rightLine.SetPosition(1, rightChild.position);
    }
}


    public void OnPointerClick(PointerEventData eventData)
    {
        BSTManager.instance.AttemptDeleteNode(this);
    }

    public void DisconnectChild(BSTNodeBehavior child)
{
    if (child == null) return;

    if (leftChild == child.transform)
    {
        leftChild = null;
        if (leftLine != null) leftLine.enabled = false;
    }
    else if (rightChild == child.transform)
    {
        rightChild = null;
        if (rightLine != null) rightLine.enabled = false;
    }
}

}