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

            //40% chance a node is invasive
            bool makeInvasive = Random.value < 0.4f;
            nodeBehavior.SetInvasive(makeInvasive);

            //Store the GameObject immediately
            nodeObjects[value] = newNode;

            return new BSTNode(value);
        }

        if (value < node.Value)
        {
            node.Left = InsertRecursive(node.Left, value);

            var parentObj = nodeObjects[node.Value].GetComponent<BSTNodeBehavior>();
            var childObj = nodeObjects[value].GetComponent<BSTNodeBehavior>();
            parentObj.ConnectChild(childObj, isLeft: true);
        }
        else
        {
            node.Right = InsertRecursive(node.Right, value);

            var parentObj = nodeObjects[node.Value].GetComponent<BSTNodeBehavior>();
            var childObj = nodeObjects[value].GetComponent<BSTNodeBehavior>();
            parentObj.ConnectChild(childObj, isLeft: false);
        }

        return node;
    }

    public void UpdateTree()
    {
        treeContainer.GetComponent<BSTVisualizer>().UpdatePositions(root);
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

        // Disconnect parent lines
        foreach (var obj in nodeObjects.Values)
        {
            BSTNodeBehavior parent = obj.GetComponent<BSTNodeBehavior>();
            if (parent != null)
            {
                parent.DisconnectChild(nodeBehavior);
            }
        }

        // Remove from dictionary
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
        if (node == null)
            return node;

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