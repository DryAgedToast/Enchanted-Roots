using UnityEngine;
using UnityEngine.UI; //SR
using TMPro;
using System.Collections.Generic;

public class BSTManager : MonoBehaviour
{
    public static BSTManager instance;
    // CREATION OF WIN STATE STARTS HERE V V
    public enum GamePhase { Deletion, Insertion, Submission} // SR
    public GamePhase currentPhase = GamePhase.Deletion; // SR
    
    [SerializeField] public GameObject nodePrefab;
    [SerializeField] public Transform treeContainer;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject winScreen;

    [SerializeField] private List<int> initialValues; // Added this field to the inspector
    [SerializeField] private Button submitButton; // SR

    public Transform nodeParent;
    public BSTNode root;
    public Dictionary<int, GameObject> nodeObjects = new();

    private int mistakeCount = 0;
    private int maxMistakes = 100;
    private int nodesLeft = 2; //SR

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        // Insert values from the initialValues list
        foreach (var value in initialValues)
        {
            Insert(value);
        }

        UpdateTree();
        UpdateLives();
    }

    public void InsertFromUI()
    {
        if (currentPhase != GamePhase.Insertion) { // Make sure all invasive nodes gone
            Debug.Log("You must delete all invasive nodes first."); // SR
            return; // SR
        }
        if (int.TryParse(inputField.text, out int value))
        {
            Insert(value);
            currentPhase = GamePhase.Submission; //SR Changes GamePhase to know be submission
            submitButton.interactable = true;
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

    public GameObject InsertAt(BSTNodeBehavior parentNode, int value, bool isLeft)
    {
        if (currentPhase != GamePhase.Insertion) // SR
        {
            Debug.Log("You must remove invasive nodes before inserting."); // SR
            return null;
        }
        GameObject newNodeObj = Instantiate(nodePrefab, treeContainer);
        var newBehavior = newNodeObj.GetComponent<BSTNodeBehavior>();
        newBehavior.SetValue(value);
        newBehavior.SetInvasive(false);

        nodeObjects[value] = newNodeObj;

        Vector3 offset = new Vector3(isLeft ? -1.5f : 1.5f, -1.5f, 0f);
        newNodeObj.transform.position = parentNode.transform.position + offset;

        parentNode.ConnectChild(newBehavior, isLeft);
        nodesLeft = nodesLeft - 1; //SR
        if (nodesLeft == 0) { // SR
            currentPhase = GamePhase.Submission; // Makes so now you can submit SR
        }
        return newNodeObj;
    }

    public void UpdateTree()
    {
        treeContainer.GetComponent<BSTVisualizer>().UpdatePositions(root);
        UpdateConnections();
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
                }
            }
        }

        ConnectRecursive(root);
    }

    private void ConnectRecursive(BSTNode node)
    {
        if (node == null) return;
        if (!nodeObjects.ContainsKey(node.Value)) return;

        var parentObj = nodeObjects[node.Value].GetComponent<BSTNodeBehavior>();

        if (node.Left != null && nodeObjects.ContainsKey(node.Left.Value))
        {
            var leftChildObj = nodeObjects[node.Left.Value].GetComponent<BSTNodeBehavior>();
            parentObj.ConnectChild(leftChildObj, true);
        }

        if (node.Right != null && nodeObjects.ContainsKey(node.Right.Value))
        {
            var rightChildObj = nodeObjects[node.Right.Value].GetComponent<BSTNodeBehavior>();
            parentObj.ConnectChild(rightChildObj, false);
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
    // Helper Function for AttemptDeleteNode (checks if all invasive nodes are gone)
    private bool AnyInvasiveNodesRemaining() // SR
    {
        foreach (var obj in nodeObjects.Values) { // SR
            var behavior = obj.GetComponent<BSTNodeBehavior>(); // SR
            if (behavior != null && behavior.isInvasive) //SR
                return true; //SR
        }
        return false; //SR
    }

    public void AttemptDeleteNode(BSTNodeBehavior nodeBehavior)
    {
        if (currentPhase != GamePhase.Deletion) //SR
        {
            MessagePopup.instance.ShowMessage("Finish inserting!"); //SR
            return;
        }
        if (nodeBehavior.isInvasive)
        {
            MessagePopup.instance.ShowMessage("Invasive node deleted!"); // ALL DEBUG MESSAGES NOW POPUP

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
            if(!AnyInvasiveNodesRemaining()) //SR
            { 
                currentPhase = GamePhase.Insertion; //SR
                MessagePopup.instance.ShowMessage("All invasive nodes removed! Now insert healthy nodes."); //SR
            }
        }
        else
        {
            MessagePopup.instance.ShowMessage("Healthy node deleted. You lost a life!");
            mistakeCount++;
            UpdateLives();
            if (mistakeCount >= maxMistakes)
            {
                MessagePopup.instance.ShowMessage("Game Over.");
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

    public void OnSubmitTree()
    {
        Debug.Log("Submit button clicked!");
        if (currentPhase != GamePhase.Submission) //SR
        {
            MessagePopup.instance.ShowMessage("Finish inserting before submitting."); //SR
            return;
        }
        if (IsValidBST(root, int.MinValue, int.MaxValue))
        {
            MessagePopup.instance.ShowMessage("Tree is valid! You saved the tree :)");
            if (winScreen != null) winScreen.SetActive(true);
        }
        else
        {
            MessagePopup.instance.ShowMessage("Invalid BST. Resetting tree.");
            ResetTree();
        }
    }

    private bool IsValidBST(BSTNode node, int min, int max)
    {
        if (node == null) return true;
        if (node.Value <= min || node.Value >= max) return false;

        return IsValidBST(node.Left, min, node.Value) &&
               IsValidBST(node.Right, node.Value, max);
    }

    public void ResetTree()
    {
        submitButton.interactable = false;
        foreach (var obj in nodeObjects.Values)
        {
            Destroy(obj);
        }
        nodeObjects.Clear();
        root = null;
        mistakeCount++;
        UpdateLives();

        if (mistakeCount < maxMistakes) //SR
        {
            currentPhase = GamePhase.Deletion; //SR
            foreach (var value in initialValues) //SR
                Insert(value);

            UpdateTree();
        }
        else
        {
            MessagePopup.instance.ShowMessage("Game Over. No more lives.");
        }
    }
}
