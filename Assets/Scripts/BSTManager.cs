using UnityEngine;
using System.Collections.Generic;
public class BSTManager : MonoBehaviour
{
    public static BSTManager instance;  // Singleton reference
    [SerializeField] public GameObject nodePrefab;
    [SerializeField] public Transform treeContainer;
    private BSTNode root;
    public Dictionary<int, GameObject> nodeObjects = new(); // Store nodes

    void Awake()
    {
        if (instance == null)
            instance = this;  // Assign instance on awake
        else
            Destroy(gameObject);  // Prevent duplicate managers
    }

    void Start()
    {
        Insert(10);
        Insert(5);
        Insert(15);
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
}
