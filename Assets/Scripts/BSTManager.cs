using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BSTManager : MonoBehaviour
{
    public static BSTManager instance;

    [SerializeField] private GameObject nodePrefab;
    [SerializeField] private Transform treeContainer;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text scoreText;

    private BSTNode root;
    public Dictionary<int, GameObject> nodeObjects = new();

    private int mistakeCount = 0;
    private int maxMistakes = 100;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Insert(10);
        Insert(5);
        Insert(15);
        UpdateTree();
        UpdateLives();
    }

    public void InsertFromUI()
    {
        if (int.TryParse(inputField.text, out int value))
        {
            Insert(value);
            inputField.text = "";
        }
        else
        {
            Debug.LogError("Invalid input! Please enter a number.");
        }
    }

    public void Insert(int value)
    {
        root = InsertRecursive(root, value);
        UpdateTree();

        if (SFXScript.instance != null)
        {
            SFXScript.instance.PlayInsertSound();
        }
    }

    private BSTNode InsertRecursive(BSTNode node, int value)
    {
        if (node == null)
        {
            GameObject newNode = Instantiate(nodePrefab, treeContainer);
            var nodeBehavior = newNode.GetComponent<BSTNodeBehavior>();

            nodeBehavior.SetValue(value);

            bool makeInvasive = Random.value < 0.4f;
            nodeBehavior.SetInvasive(makeInvasive);

            nodeObjects[value] = newNode;

            return new BSTNode(value);
        }

        if (value < node.Value)
        {
            node.Left = InsertRecursive(node.Left, value);
        }
        else
        {
            node.Right = InsertRecursive(node.Right, value);
        }

        return node;
    }

    public void UpdateTree()
    {
        treeContainer.GetComponent<BSTVisualizer>().UpdatePositions(root);
        UpdateConnections(); // NEW: Corrects all parent-child lines
    }

    public void UpdateConnections()
{
    foreach (var obj in nodeObjects.Values)
    {
        var behavior = obj.GetComponent<BSTNodeBehavior>();
        if (behavior != null)
        {
            behavior.leftChild = null;
            behavior.rightChild = null;

            if (behavior.targetObjectToColor != null)
            {
                LineRenderer lr = behavior.targetObjectToColor.GetComponent<LineRenderer>();
                if (lr != null)
                {
                    lr.enabled = false;
                }
                // Else: it's fine — it just doesn't have a LineRenderer
            }
        }
    }

    ConnectRecursive(root);
}


    private void ConnectRecursive(BSTNode node)
{
    if (node == null) return;

    if (!nodeObjects.ContainsKey(node.Value))
    {
        // Skip this node because its visual was deleted
        return;
    }

    if (node.Left != null && nodeObjects.ContainsKey(node.Left.Value))
    {
        var parentObj = nodeObjects[node.Value].GetComponent<BSTNodeBehavior>();
        var leftChildObj = nodeObjects[node.Left.Value].GetComponent<BSTNodeBehavior>();
        parentObj.ConnectChild(leftChildObj, isLeft: true);
    }

    if (node.Right != null && nodeObjects.ContainsKey(node.Right.Value))
    {
        var parentObj = nodeObjects[node.Value].GetComponent<BSTNodeBehavior>();
        var rightChildObj = nodeObjects[node.Right.Value].GetComponent<BSTNodeBehavior>();
        parentObj.ConnectChild(rightChildObj, isLeft: false);
    }

    ConnectRecursive(node.Left);
    ConnectRecursive(node.Right);
}


    public void UpdateLives()
    {
        if ((maxMistakes - mistakeCount) > 0)
        {
            scoreText.text = "Lives: " + (maxMistakes - mistakeCount);
        }
        else
        {
            scoreText.text = "Game Over!";
        }
    }

    public void AttemptDeleteNode(BSTNodeBehavior nodeBehavior)
    {
        if (nodeBehavior.isInvasive)
        {
            Debug.Log("Invasive node deleted!");

            foreach (var obj in nodeObjects.Values)
            {
                BSTNodeBehavior parent = obj.GetComponent<BSTNodeBehavior>();
                if (parent != null)
                {
                    parent.DisconnectChild(nodeBehavior);
                }
            }

            if (nodeObjects.ContainsKey(nodeBehavior.Value))
            {
                nodeObjects.Remove(nodeBehavior.Value);
            }

            root = DeleteRecursive(root, nodeBehavior.Value);
            Destroy(nodeBehavior.gameObject);
            UpdateTree();

            if (SFXScript.instance != null)
            {
                SFXScript.instance.PlayInsertSound();
            }
        }
        else
        {
            Debug.Log("Healthy node deleted. Mistake made.");
            mistakeCount++;
            UpdateLives();
            if (mistakeCount >= maxMistakes)
            {
                Debug.Log("Game Over.");
            }
        }
    }

    private BSTNode DeleteRecursive(BSTNode node, int value)
    {
        if (node == null) return node;

        if (value < node.Value)
        {
            node.Left = DeleteRecursive(node.Left, value);
        }
        else if (value > node.Value)
        {
            node.Right = DeleteRecursive(node.Right, value);
        }
        else
        {
            if (node.Left == null) return node.Right;
            if (node.Right == null) return node.Left;

            BSTNode temp = FindMin(node.Right);
            node.Value = temp.Value;
            node.Right = DeleteRecursive(node.Right, temp.Value);
        }
        return node;
    }

    private BSTNode FindMin(BSTNode node)
    {
        while (node.Left != null)
        {
            node = node.Left;
        }
        return node;
    }
}
