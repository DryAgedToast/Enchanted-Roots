using UnityEngine;

public class HoverMaterialSwap : MonoBehaviour
{
    public Material outlineMaterial;    // Assign in Inspector
    public Material defaultMaterial;    // Assign in Inspector

    private SpriteRenderer bushRenderer;

    private void Awake()
    {
        // Find the Bush child and get its SpriteRenderer
        Transform bush = transform.Find("Bush");
        if (bush != null)
        {
            bushRenderer = bush.GetComponent<SpriteRenderer>();
        }

        if (bushRenderer == null)
        {
            Debug.LogError("Bush SpriteRenderer not found.");
        }
    }

    private void OnMouseEnter()
    {
        if (bushRenderer != null && outlineMaterial != null)
        {
            bushRenderer.material = outlineMaterial;
        }
    }

    private void OnMouseExit()
    {
        if (bushRenderer != null && defaultMaterial != null)
        {
            bushRenderer.material = defaultMaterial;
        }
    }
}
