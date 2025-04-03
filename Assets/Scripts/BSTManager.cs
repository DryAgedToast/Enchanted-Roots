using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BSTManager : MonoBehaviour
{
    public static BSTManager instance;

    [SerializeField] private GameObject nodePrefab;  // Prefab for BST Nodes
    [SerializeField] private Transform treeContainer; // Parent transform for nodes
    [SerializeField] private TMP_InputField inputField; // UI Input Field
    [SerializeField] private TMP_Text traversalText; // UI Text for traversal output

    private BSTNode root;
    public Dictionary<int, GameObject> nodeObjects = new(); // Store node GameObjects

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        Insert(10);
        Insert(5);
        Insert(15);
        UpdateTree();
    }

    public void InsertFromUI()
    {
        if (int.TryParse(inputField.text, out int value))
        {
            Insert(value);
            inputField.text = ""; // Clear input field
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
    }

    private BSTNode InsertRecursive(BSTNode node, int value)
    {
        if (node == null)
        {
            GameObject newNode = Instantiate(nodePrefab, treeContainer);
            newNode.GetComponent<BSTNodeBehavior>().SetValue(value);
            nodeObjects[value] = newNode; // Store reference to Unity object
            return new BSTNode(value);
        }

        if (value < node.Value)
            node.Left = InsertRecursive(node.Left, value);
        else
            node.Right = InsertRecursive(node.Right, value);

        return node;
    }

    public void UpdateTree()
    {
        treeContainer.GetComponent<BSTVisualizer>().UpdatePositions(root);
    }

    public void ShowInorderTraversal()
    {
        List<int> values = new();
        InorderTraversal(root, values);
        traversalText.text = "Inorder: " + string.Join(", ", values);
    }

    private void InorderTraversal(BSTNode node, List<int> values)
    {
        if (node == null) return;
        InorderTraversal(node.Left, values);
        values.Add(node.Value);
        InorderTraversal(node.Right, values);
    }
}
