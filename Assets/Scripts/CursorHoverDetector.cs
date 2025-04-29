using UnityEngine;

public class CursorHoverDetector : MonoBehaviour
{
    private BSTNodeGlow lastGlowingNode;

    void Update()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent(out BSTNodeGlow nodeGlow))
            {
                if (lastGlowingNode != nodeGlow)
                {
                    if (lastGlowingNode != null)
                    {
                        lastGlowingNode.DisableGlow();
                    }

                    nodeGlow.EnableGlow();
                    lastGlowingNode = nodeGlow;
                }
            }
            else
            {
                if (lastGlowingNode != null)
                {
                    lastGlowingNode.DisableGlow();
                    lastGlowingNode = null;
                }
            }
        }
        else
        {
            if (lastGlowingNode != null)
            {
                lastGlowingNode.DisableGlow();
                lastGlowingNode = null;
            }
        }
    }
} 