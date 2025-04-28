using UnityEngine;

public class BSTVisualizer : MonoBehaviour
{
    public float xSpacing = 2.5f;
    public float ySpacing = 2.0f;

    public void UpdatePositions(BSTNode root, float x = 0, float y = 0, int depth = 0)
    {
        if (root == null) return;

        Vector3 position = new Vector3(x, y, 0);
        if (BSTManager.instance.nodeObjects.TryGetValue(root.Value, out GameObject nodeObj))
        {
            nodeObj.transform.position = position;
        }

        UpdatePositions(root.Left, x - xSpacing / (depth + 1), y - ySpacing, depth + 1);
        UpdatePositions(root.Right, x + xSpacing / (depth + 1), y - ySpacing, depth + 1);
    }
}
