using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

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
    [SerializeField] private List<bool> initialInvasiveFlags; // New: parallel to initialValues

    [SerializeField] private Button submitButton;

    public Transform nodeParent;
    public BSTNode root;
    public Dictionary<int, GameObject> nodeObjects = new();

    private int mistakeCount = 0;
    private int maxMistakes = 100;
    private int nodesLeft = 2;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        for (int i = 0; i < initialValues.Count; i++)
        {
            int value = initialValues[i];
            bool isInvasive = (i < initialInvasiveFlags.Count) ? initialInvasiveFlags[i] : false;
            Insert(value, isInvasive);
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
            Insert(value, false); // Always non-invasive from UI
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

        if (SFXScript.instance != null)
        {
            SFXScript.instance.PlayInsertSound();
        }
    }

    private BSTNode InsertRecursive(BSTNode node, int value, bool isInvasive)
    {
        if (node == null)
        {
            GameObject newNode = Instantiate(nodePrefab, treeContainer);
            var nodeBehavior = newNode.GetComponent<BSTNodeBehavior>();
            nodeBehavior.SetValue(value);
            nodeBehavior.SetInvasive(isInvasive);

            nodeObjects[value] = newNode;
            return new BSTNode(value);
        }

        if (value < node.Value)
        {
            node.Left = InsertRecursive(node.Left, value, isInvasive);
        }
        else
        {
            node.Right = InsertRecursive(node.Right, value, isInvasive);
        }

        return node;
    }

    public GameObject InsertAt(BSTNodeBehavior parentNode, int value, bool isLeft)
    {
        if (currentPhase != GamePhase.Insertion)
        {
            Debug.Log("You must remove invasive nodes before inserting.");
            return null;
        }

        GameObject newNodeObj = Instantiate(nodePrefab, treeContainer);
        var newBehavior = newNodeObj.GetComponent<BSTNodeBehavior>();
        newBehavior.SetValue(value);
        newBehavior.SetInvasive(false); // Dragged nodes are always healthy

        nodeObjects[value] = newNodeObj;

        Vector3 offset = new Vector3(isLeft ? -1.5f : 1.5f, -1.5f, 0f);
        newNodeObj.transform.position = parentNode.transform.position + offset;

        parentNode.ConnectChild(newBehavior, isLeft);
        nodesLeft--;

        if (nodesLeft == 0)
        {
            currentPhase = GamePhase.Submission;
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
            {
                var parent = obj.GetComponent<BSTNodeBehavior>();
                parent?.DisconnectChild(nodeBehavior);
            }

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

        if (IsValidBST(root, int.MinValue, int.MaxValue))
        {
            MessagePopup.instance.ShowMessage("Tree is valid! You saved the tree :)");
            winScreen?.SetActive(true);
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
            Destroy(obj);

        nodeObjects.Clear();
        root = null;
        mistakeCount++;
        UpdateLives();

        if (mistakeCount < maxMistakes)
        {
            currentPhase = GamePhase.Deletion;
            for (int i = 0; i < initialValues.Count; i++)
            {
                int value = initialValues[i];
                bool isInvasive = (i < initialInvasiveFlags.Count) ? initialInvasiveFlags[i] : false;
                Insert(value, isInvasive);
            }

            UpdateTree();
        }
        else
        {
            MessagePopup.instance.ShowMessage("Game Over. No more lives.");
        }
    }
}
