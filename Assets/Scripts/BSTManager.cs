using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BSTManager : MonoBehaviour
{
    public static BSTManager instance;

    public enum GamePhase { Deletion, Insertion, Submission }
    public GamePhase currentPhase = GamePhase.Deletion;

    [SerializeField] public GameObject nodePrefab;
    [SerializeField] public Transform treeContainer;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private GameObject winScreen;

    [SerializeField] private List<int> initialValues;
    [SerializeField] private List<bool> initialInvasiveFlags;

    [SerializeField] private Button submitButton;

    public BSTNode root;
    public Dictionary<int, GameObject> nodeObjects = new Dictionary<int, GameObject>();

    private int mistakeCount = 0;
    private int maxMistakes = 100;
    private int nodesLeft = 2; // For additional insertions

    [SerializeField] private GameObject winLosePopupPrefab;
    private GameObject activePopup;

    private void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        // Create the initial BST nodes using the logical insertion routine.
        for (int i = 0; i < initialValues.Count; i++)
        {
            int value = initialValues[i];
            bool isInvasive = (i < initialInvasiveFlags.Count) ? initialInvasiveFlags[i] : false;
            root = InsertRecursive(root, value, isInvasive);
        }

        UpdateTree();
        UpdateLives();
    }

    public void InsertFromUI()
    {
        if (currentPhase != GamePhase.Insertion)
        {
            Debug.Log("You must delete all invasive nodes first.");
            return;
        }

        if (int.TryParse(inputField.text, out int value))
        {
            Insert(value, false);
            currentPhase = GamePhase.Submission;
            submitButton.interactable = true;
            inputField.text = "";
        }
        else
        {
            Debug.LogError("Invalid input! Please enter a number.");
        }
    }

    public void Insert(int value, bool isInvasive)
    {
        root = InsertRecursive(root, value, isInvasive);
        UpdateTree();

        SFXScript.instance?.PlayInsertSound();
    }

    // When inserting initially, create both a logical node and its visual
    private BSTNode InsertRecursive(BSTNode node, int value, bool isInvasive)
    {
        if (node == null)
        {
            GameObject newNode = Instantiate(nodePrefab, treeContainer);
            var nodeBehavior = newNode.GetComponent<BSTNodeBehavior>();
            nodeBehavior.SetValue(value);
            nodeBehavior.SetInvasive(isInvasive);

            // Create the corresponding logical node and store a link to its visual component.
            BSTNode newLogicalNode = new BSTNode(value, isInvasive);
            newLogicalNode.Behavior = nodeBehavior;
            nodeBehavior.logicalNode = newLogicalNode;

            nodeObjects[value] = newNode;
            return newLogicalNode;
        }

        if (value < node.Value)
            node.Left = InsertRecursive(node.Left, value, isInvasive);
        else
            node.Right = InsertRecursive(node.Right, value, isInvasive);

        return node;
    }

    // In the drag‐and‐drop insertion path, update the logical tree as well as the visual tree.
    
public BSTNode FindNode(BSTNode node, int value)
{
    if (node == null)
        return null;

    if (node.Value == value)
        return node;

    BSTNode found = FindNode(node.Left, value);
    if (found != null)
        return found;

    return FindNode(node.Right, value);
}

public GameObject InsertAt(BSTNodeBehavior parentNode, int value, bool isLeft)
{
    if (currentPhase != GamePhase.Insertion)
    {
        Debug.Log("You must remove invasive nodes before inserting.");
        return null;
    }

    if ((isLeft && parentNode.leftChild != null) || (!isLeft && parentNode.rightChild != null))
    {
        MessagePopup.instance.ShowMessage("This position is already occupied.");
        return null;
    }

    if (nodeObjects.ContainsKey(value))
    {
        MessagePopup.instance.ShowMessage("This value has already been inserted.");
        return null;
    }

    GameObject newNodeObj = Instantiate(nodePrefab, treeContainer);
    var newBehavior = newNodeObj.GetComponent<BSTNodeBehavior>();
    newBehavior.SetValue(value);
    newBehavior.SetInvasive(false);

    // Create a new logical node and associate it with the visual node.
    BSTNode newLogicalNode = new BSTNode(value);
    newLogicalNode.Behavior = newBehavior;
    newBehavior.logicalNode = newLogicalNode;

    // Find the parent's node in the logical tree (which starts at 'root').
    BSTNode parentLogical = FindNode(root, parentNode.Value);
    if (parentLogical == null)
    {
        Debug.LogError("Parent's logical node not found in the tree!");
        return null;
    }

    // Update the parent's pointer according to the chosen side.
    if (isLeft)
        parentLogical.Left = newLogicalNode;
    else
        parentLogical.Right = newLogicalNode;

    nodeObjects[value] = newNodeObj;

    Vector3 offset = new Vector3(isLeft ? -1.5f : 1.5f, -1.5f, 0f);
    newNodeObj.transform.position = parentNode.transform.position + offset;

    parentNode.ConnectChild(newBehavior, isLeft);
    nodesLeft--;

    // If no additional nodes are to be inserted, change phase to Submission.
    if (nodesLeft <= 0)
    {
        currentPhase = GamePhase.Submission;
        submitButton.interactable = true;
    }

    // Update visual positions based on the updated logical tree.
    UpdateTree();
    return newNodeObj;
}


    public void UpdateTree()
    {
        treeContainer.GetComponent<BSTVisualizer>().UpdatePositions(root);
        UpdateConnections();
    }

    public void UpdateConnections()
    {
        // First, reset all visual connections.
        foreach (var obj in nodeObjects.Values)
        {
            var behavior = obj.GetComponent<BSTNodeBehavior>();
            behavior.leftChild = null;
            behavior.rightChild = null;

            if (behavior.targetObjectToColor != null)
            {
                var lr = behavior.targetObjectToColor.GetComponent<LineRenderer>();
                if (lr != null) lr.enabled = false;
            }
        }
        ConnectRecursive(root);
    }

    // Walk the logical BST and update visual connections.
    private void ConnectRecursive(BSTNode node)
    {
        if (node == null || !nodeObjects.ContainsKey(node.Value)) return;

        var parentObj = nodeObjects[node.Value].GetComponent<BSTNodeBehavior>();

        if (node.Left != null && nodeObjects.ContainsKey(node.Left.Value))
            parentObj.ConnectChild(nodeObjects[node.Left.Value].GetComponent<BSTNodeBehavior>(), true);

        if (node.Right != null && nodeObjects.ContainsKey(node.Right.Value))
            parentObj.ConnectChild(nodeObjects[node.Right.Value].GetComponent<BSTNodeBehavior>(), false);

        ConnectRecursive(node.Left);
        ConnectRecursive(node.Right);
    }

    public void UpdateLives()
    {
        scoreText.text = (maxMistakes - mistakeCount) > 0
            ? $"Lives: {maxMistakes - mistakeCount}"
            : "Game Over!";
    }

    private bool AnyInvasiveNodesRemaining()
    {
        foreach (var obj in nodeObjects.Values)
        {
            var behavior = obj.GetComponent<BSTNodeBehavior>();
            if (behavior != null && behavior.isInvasive)
                return true;
        }
        return false;
    }

    public void AttemptDeleteNode(BSTNodeBehavior nodeBehavior)
    {
        if (currentPhase != GamePhase.Deletion)
        {
            MessagePopup.instance.ShowMessage("Finish inserting!");
            return;
        }

        if (nodeBehavior.isInvasive)
        {
            MessagePopup.instance.ShowMessage("Invasive node deleted!");

            foreach (var obj in nodeObjects.Values)
                obj.GetComponent<BSTNodeBehavior>()?.DisconnectChild(nodeBehavior);

            nodeObjects.Remove(nodeBehavior.Value);
            root = DeleteRecursive(root, nodeBehavior.Value);
            Destroy(nodeBehavior.gameObject);
            UpdateTree();

            SFXScript.instance?.PlayInsertSound();

            if (!AnyInvasiveNodesRemaining())
            {
                currentPhase = GamePhase.Insertion;
                MessagePopup.instance.ShowMessage("All invasive nodes removed! Now insert healthy nodes.");
            }
        }
        else
        {
            MessagePopup.instance.ShowMessage("Healthy node deleted. You lost a life!");
            mistakeCount++;
            UpdateLives();

            if (mistakeCount >= maxMistakes)
                MessagePopup.instance.ShowMessage("Game Over.");
        }
    }

    private BSTNode DeleteRecursive(BSTNode node, int value)
    {
        if (node == null) return node;

        if (value < node.Value)
            node.Left = DeleteRecursive(node.Left, value);
        else if (value > node.Value)
            node.Right = DeleteRecursive(node.Right, value);
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
            node = node.Left;
        return node;
    }

    public void OnSubmitTree()
{
    Debug.Log("Submit button clicked!");

    if (currentPhase != GamePhase.Submission)
    {
        MessagePopup.instance.ShowMessage("Finish inserting before submitting.");
        return;
    }
    
    // Option 1: Validate the logical tree
    if (IsValidBST(root, int.MinValue, int.MaxValue))
    {
        MessagePopup.instance.ShowMessage("Tree is valid! You saved the tree :)");
        if (winScreen != null)
            winScreen.SetActive(true);
        ShowWinLosePopup(true);
    }
    else
    {
        MessagePopup.instance.ShowMessage("Invalid BST. Resetting tree.");
        ResetTree();
    }
}


private bool IsValidBST(BSTNode node, int min, int max)
{
    if (node == null)
        return true;
    
    Debug.Log($"Validating node {node.Value} in range ({min}, {max})");
    if (node.Value <= min || node.Value >= max)
    {
        Debug.Log($"Validation failed at node {node.Value}: not in ({min}, {max})");
        return false;
    }

    return IsValidBST(node.Left, min, node.Value) && 
           IsValidBST(node.Right, node.Value, max);
}


    // Finds the root in the visual tree – a node with no parent.
    private BSTNodeBehavior FindVisualRoot()
    {
        foreach (var obj in nodeObjects.Values)
        {
            var behavior = obj.GetComponent<BSTNodeBehavior>();
            bool hasParent = false;

            foreach (var otherObj in nodeObjects.Values)
            {
                var otherBehavior = otherObj.GetComponent<BSTNodeBehavior>();
                if (otherBehavior != null && (otherBehavior.leftChild == behavior || otherBehavior.rightChild == behavior))
                {
                    hasParent = true;
                    break;
                }
            }

            if (!hasParent)
                return behavior;
        }

        return null;
    }

    // Recursively validate the visual tree using the standard BST constraints.
    private bool IsValidVisualBST(BSTNodeBehavior node, int min, int max)
    {
        if (node == null) return true;

        if (node.Value <= min || node.Value >= max)
            return false;

        BSTNodeBehavior leftBehavior = node.leftChild != null ? node.leftChild.GetComponent<BSTNodeBehavior>() : null;
        BSTNodeBehavior rightBehavior = node.rightChild != null ? node.rightChild.GetComponent<BSTNodeBehavior>() : null;

        return IsValidVisualBST(leftBehavior, min, node.Value) &&
               IsValidVisualBST(rightBehavior, node.Value, max);
    }

    public void ResetTree()
{
    // Disable submit button while resetting.
    submitButton.interactable = false;

    // Destroy all current node GameObjects.
    foreach (var obj in nodeObjects.Values)
    {
        Destroy(obj);
    }
    nodeObjects.Clear();
    root = null;

    // Increment mistake count and update UI.
    mistakeCount++;
    UpdateLives();

    // Reset the insertion queue.
    QueueManager.instance.ResetQueue();

    // Reset the additional insertion counter.
    nodesLeft = 2;  // (Adjust this value if needed for your game design.)

    // Rebuild the initial tree in "Deletion" phase so that the player can delete invasive nodes.
    currentPhase = GamePhase.Deletion;
    for (int i = 0; i < initialValues.Count; i++)
    {
        int value = initialValues[i];
        bool isInvasive = (i < initialInvasiveFlags.Count) ? initialInvasiveFlags[i] : false;
        root = InsertRecursive(root, value, isInvasive);
    }
    UpdateTree();

    // Show a message that the tree has been reset.
    MessagePopup.instance.ShowMessage("Tree reset. Delete invasive nodes before inserting healthy ones.");
}




    public void ShowWinLosePopup(bool won)
    {
        if (winLosePopupPrefab == null) return;

        activePopup = Instantiate(winLosePopupPrefab, transform);
        var popupScript = activePopup.GetComponent<WinLosePopup>();
        popupScript?.Setup(won);
    }
}

public class BSTNode
{
    public int Value;
    public BSTNode Left;
    public BSTNode Right;
    public BSTNodeBehavior Behavior;  // Link to the visual component
    public bool isInvasive;

    public BSTNode(int value, bool isInvasive = false)
    {
        Value = value;
        this.isInvasive = isInvasive;
        Left = null;
        Right = null;
    }
}
