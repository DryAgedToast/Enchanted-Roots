using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class BSTNodeBehavior : MonoBehaviour
{
    public int Value { get; private set; }
    private TMP_Text nodeText;

    public bool isInvasive = false;
    public Sprite normalSprite;
    public Sprite invasiveSprite;
    public GameObject targetObjectToColor;

    
    private LineRenderer leftLine;
    private LineRenderer rightLine;
    public Transform leftChild;
    public Transform rightChild;

    public GameObject leftDropZone;
    public GameObject rightDropZone;

    // Link to the logical BST node that represents this node in the data structure.
    public BSTNode logicalNode;

    private void Awake()
    {
        leftDropZone = transform.Find("LeftDropZone")?.gameObject;
        rightDropZone = transform.Find("RightDropZone")?.gameObject;

        nodeText = GetComponentInChildren<TMP_Text>();

        Transform leftLineObj = transform.Find("LeftLine");
        Transform rightLineObj = transform.Find("RightLine");

        if (leftLineObj != null && rightLineObj != null)
        {
            leftLine = leftLineObj.GetComponent<LineRenderer>();
            rightLine = rightLineObj.GetComponent<LineRenderer>();
        }

        SetupLineRenderer(leftLine, Color.gray);
        SetupLineRenderer(rightLine, Color.gray);
        ShowDropZones(false);
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
                if (invasive && invasiveSprite != null)
                    {
                        targetRenderer.sprite = invasiveSprite;
                    }
                    else if (!invasive && normalSprite != null)
                    {
                        targetRenderer.sprite = normalSprite;
                    }
            }
        }
    }

    public void ConnectChild(BSTNodeBehavior child, bool isLeft)
    {
        if (child == null) return;

        if (isLeft)
        {
            leftChild = child.transform;
            if (leftLine != null) leftLine.enabled = true;
            if (leftDropZone != null) leftDropZone.SetActive(false);
        }
        else
        {
            rightChild = child.transform;
            if (rightLine != null) rightLine.enabled = true;
            if (rightDropZone != null) rightDropZone.SetActive(false);
        }
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

    public void ShowDropZones(bool show)
    {
        if (leftDropZone != null) leftDropZone.SetActive(show);
        if (rightDropZone != null) rightDropZone.SetActive(show);
    }

    private void OnMouseDown()
    {
        BSTManager.instance.AttemptDeleteNode(this);
    }
}
