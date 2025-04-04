using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class BSTManager : MonoBehaviour
{
    public static BSTManager instance;

    [SerializeField] private GameObject nodePrefab;  // Prefab for BST Nodes
    [SerializeField] private Transform treeContainer; // Parent transform for nodes
    [SerializeField] private TMP_InputField inputField; // UI Input Field (this is the text input field you'll see at the UI)
    [SerializeField] private TMP_Text traversalText; // UI Text for traversal output (w.i.p., not completed yet)

    private BSTNode root;
    public Dictionary<int, GameObject> nodeObjects = new(); // Store node GameObjects

    private void Awake()
    {
    //checks if manager instance exists
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
    //inserts 3 nodes for demo purposes
        Insert(10);
        Insert(5);
        Insert(15);
        UpdateTree();
    }

    //method responsible for inserting nodes based on text input on the UI
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

//This is the insert method used for the BST
public void Insert(int value)
{
    root = InsertRecursive(root, value);
    UpdateTree();

    // Play the insert sound
    if (SFXScript.instance != null)
    {
        SFXScript.instance.PlayInsertSound();
    }
}

    //Method that uses recursion to traverse the BST and insert the node
    private BSTNode InsertRecursive(BSTNode node, int value)
    {
        //if node does not exist, instantiate a new node and assign the value described in the parameters to said node.
        if (node == null)
        {
            GameObject newNode = Instantiate(nodePrefab, treeContainer);
            newNode.GetComponent<BSTNodeBehavior>().SetValue(value);
            nodeObjects[value] = newNode; // Store reference to Unity object
            return new BSTNode(value);
        }

        //if value to be inserted is less than node value, insert left
        if (value < node.Value)
            node.Left = InsertRecursive(node.Left, value);
        //else, if value to be inserted is greater than or equal to node value, insert right
        else
            node.Right = InsertRecursive(node.Right, value);

        return node;
    }

    //method used to spawn and update the positions of the BST. called after every insert to ensure nodes are properly displayed
    public void UpdateTree()
    {
        treeContainer.GetComponent<BSTVisualizer>().UpdatePositions(root);
    }

    //w.i.p. method used to demonstrate traversals
    public void ShowInorderTraversal()
    {
        List<int> values = new();
        InorderTraversal(root, values);
        traversalText.text = "Inorder: " + string.Join(", ", values);
    }
    
    //w.i.p. method used to demonstrate traversals
    private void InorderTraversal(BSTNode node, List<int> values)
    {
        if (node == null) return;
        InorderTraversal(node.Left, values);
        values.Add(node.Value);
        InorderTraversal(node.Right, values);
    }
}
