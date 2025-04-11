using UnityEngine;
using TMPro;
using System.Collections.Generic;

/*

IMPORTANT:

currently, for testing purposes, the invasive node has a 40% chance of spawning

this is in the 
private BSTNode InsertRecursive(BSTNode node, int value)
method

*/

public class BSTManager : MonoBehaviour
{
    public static BSTManager instance;

    [SerializeField] private GameObject nodePrefab;  // prefab for BST Nodes
    [SerializeField] private Transform treeContainer; // parent transform for nodes
    [SerializeField] private TMP_InputField inputField; // UI Input Field (this is the text input field you'll see at the UI)

    // [SerializeField] private TMP_Text traversalText; // UI Text for traversal output (w.i.p., not completed yet)

    [SerializeField] private TMP_Text scoreText; //UI Text for the # of lives the player has

    private BSTNode root;
    public Dictionary<int, GameObject> nodeObjects = new(); // store node GameObjects

    private int mistakeCount = 0;
    private int maxMistakes = 3;

    private void Awake()
    {
        // checks if manager instance exists
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    void Start()
    {
        // inserts 3 nodes for demo purposes
        Insert(10);
        Insert(5);
        Insert(15);
        UpdateTree();
        scoreText.text = "Lives: " + (maxMistakes - mistakeCount);
    }

    // method responsible for inserting nodes based on text input on the UI
    public void InsertFromUI()
    {
        if (int.TryParse(inputField.text, out int value))
        {
            Insert(value);
            inputField.text = ""; // clear input field
        }
        else
        {
            Debug.LogError("Invalid input! Please enter a number.");
        }
    }

    // this is the insert method used for the BST
    public void Insert(int value)
    {
        // from the root, begin the traversal process for inserting a node
        root = InsertRecursive(root, value);
        // update the tree after insertion to show the newly inserted node
        UpdateTree();

        // play the insert sound
        if (SFXScript.instance != null)
        {
            SFXScript.instance.PlayInsertSound();
        }
    }

    // method that uses recursion to traverse the BST and insert the node
    private BSTNode InsertRecursive(BSTNode node, int value)
    {
        // if node does not exist, instantiate a new node and assign the value described in the parameters to said node
        if (node == null)
        {
            GameObject newNode = Instantiate(nodePrefab, treeContainer);
            var nodeBehavior = newNode.GetComponent<BSTNodeBehavior>();

            nodeBehavior.SetValue(value);

            // 40% chance a node is invasive
            bool makeInvasive = Random.value < 0.4f;
            nodeBehavior.SetInvasive(makeInvasive);

            nodeObjects[value] = newNode;
            return new BSTNode(value);
        }

        // if value to be inserted is less than node value, insert left
        if (value < node.Value)
            node.Left = InsertRecursive(node.Left, value);
        // else, if value to be inserted is greater than or equal to node value, insert right
        else
            node.Right = InsertRecursive(node.Right, value);

        return node;
    }

    // method used to spawn and update the positions of the BST. called after every insert to ensure nodes are properly displayed
    public void UpdateTree()
    {
        treeContainer.GetComponent<BSTVisualizer>().UpdatePositions(root);
    }

    //updates lives
    public void UpdateLives(){
        if ((maxMistakes - mistakeCount) > 0){
            scoreText.text = "Lives: " + (maxMistakes - mistakeCount);
        }
        else
        {
            scoreText.text = "Game Over!";
        }
        

    }

    // // w.i.p. method used to demonstrate traversals
    // public void ShowInorderTraversal()
    // {
    //     List<int> values = new();
    //     InorderTraversal(root, values);
    //     traversalText.text = "Inorder: " + string.Join(", ", values);
    // }

    // // w.i.p. method used to demonstrate traversals
    // private void InorderTraversal(BSTNode node, List<int> values)
    // {
    //     if (node == null) return;
    //     InorderTraversal(node.Left, values);
    //     values.Add(node.Value);
    //     InorderTraversal(node.Right, values);
    // }

    // method used when the player clicks a node to attempt deletion
    public void AttemptDeleteNode(BSTNodeBehavior nodeBehavior)
    {
        if (nodeBehavior.isInvasive)
        {
            // deleted the invasive node
            Debug.Log("Invasive node deleted. :)");

            // 1. remove node from dictionary
            if (nodeObjects.ContainsKey(nodeBehavior.Value))
            {
                nodeObjects.Remove(nodeBehavior.Value);
            }

            // 2. remove node from BST structure
            root = DeleteRecursive(root, nodeBehavior.Value);

            // 3. destroy the visual GameObject
            Destroy(nodeBehavior.gameObject);

            // 4. update tree layout after deletion
            UpdateTree();

            // play the insert sound (until you get a successfully deleted sound or something)
            if (SFXScript.instance != null)
            {
                SFXScript.instance.PlayInsertSound();
            }
        }
        else
        {
            // deleted a healthy node
            Debug.Log("Healthy node deleted. :(");
            mistakeCount++;
            UpdateLives();
            if (mistakeCount >= maxMistakes)
            {
                Debug.Log("Game Over.");
                // TODO: Show vines, trigger lose screen, etc.
            }
        }
    }

    // recursively delete a node from the logical BST structure
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
            //node found
            if (node.Left == null) return node.Right;
            if (node.Right == null) return node.Left;

            //node with two children: get the inorder successor (smallest in the right subtree)
            BSTNode temp = FindMin(node.Right);
            node.Value = temp.Value;
            node.Right = DeleteRecursive(node.Right, temp.Value);
        }
        return node;
    }

    //helper method to find the minimum value node in a subtree
    private BSTNode FindMin(BSTNode node)
    {
        while (node.Left != null)
        {
            node = node.Left;
        }
        return node;
    }
}
