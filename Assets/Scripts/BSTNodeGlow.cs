using UnityEngine;

public class BSTNodeGlow : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private Material materialInstance;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            // Make sure we have a unique instance of the material
            materialInstance = Instantiate(spriteRenderer.material);
            spriteRenderer.material = materialInstance;
        }
    }

    public void EnableGlow()
    {
        if (materialInstance != null)
        {
            materialInstance.SetFloat("_OutlineThickness", 1.5f); // Or your custom shader property
            materialInstance.SetColor("_OutlineColor", Color.white);
        }
    }

    public void DisableGlow()
    {
        if (materialInstance != null)
        {
            materialInstance.SetFloat("_OutlineThickness", 0f);
        }
    }

    void OnMouseEnter()
{
    EnableGlow();
}

void OnMouseExit()
{
    DisableGlow();
}
}
