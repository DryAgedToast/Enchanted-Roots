using UnityEngine;

public class BSTVisualizer : MonoBehaviour
{
    private float xSpacing = 2.0f;
    private float ySpacing = 2.5f;

    public void UpdatePositions(BSTNode root)
    {
        if (root == null) return;
        PositionNodes(root, 0, 0, xSpacing);
    }

    private void PositionNodes(BSTNode node, float x, float y, float spacing)
    {
        if (node == null) return;

        // Move the corresponding GameObject in the scene
        if (BSTManager.instance.nodeObjects.ContainsKey(node.Value))
        {
            BSTManager.instance.nodeObjects[node.Value].transform.position = new Vector3(x, y, 0);
        }

        // Position left and right children
        PositionNodes(node.Left, x - spacing, y - ySpacing, spacing / 1.5f);
        PositionNodes(node.Right, x + spacing, y - ySpacing, spacing / 1.5f);
    }
}
