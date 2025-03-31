using UnityEngine;
using System.Collections.Generic;

public class BSTVisualizer : MonoBehaviour
{
    public float xSpacing = 50f; 
    public float ySpacing = 100f;

    public void UpdatePositions(BSTNode root)
    {
        if (root == null) return;
        PositionNodes(root, 0, 0, 0);
    }

    private void PositionNodes(BSTNode node, float x, float y, int depth)
    {
        if (node == null) return;

        // Find corresponding GameObject for node and set position
        foreach (Transform child in transform)
        {
            BSTNodeBehavior nodeBehavior = child.GetComponent<BSTNodeBehavior>();
            if (nodeBehavior != null && nodeBehavior.Value == node.Value)
            {
                child.localPosition = new Vector3(x, y, 0);
            }
        }

        // Recursive positioning for left and right children
        PositionNodes(node.Left, x - xSpacing / (depth + 1), y - ySpacing, depth + 1);
        PositionNodes(node.Right, x + xSpacing / (depth + 1), y - ySpacing, depth + 1);
    }
}
