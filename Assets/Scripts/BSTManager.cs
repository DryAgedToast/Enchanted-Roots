using UnityEngine;
using System.Collections.Generic;

public class BSTManager : MonoBehaviour
{
    [SerializeField] public GameObject nodePrefab; 
    [SerializeField] public Transform treeContainer; 
    private BSTNode root;

//testing insert
    void Start()
{
    Insert(10);
    Insert(5);
    Insert(15);
}


    public BSTNode Search(int value)
    {
        return SearchRecursive(root, value);
    }

    private BSTNode SearchRecursive(BSTNode node, int value)
    {
        if (node == null || node.Value == value) return node;
        return value < node.Value ? SearchRecursive(node.Left, value) : SearchRecursive(node.Right, value);
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
        // Call BSTVisualizer to reposition nodes
        treeContainer.GetComponent<BSTVisualizer>().UpdatePositions(root);
    }
}
