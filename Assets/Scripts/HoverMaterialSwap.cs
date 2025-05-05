using UnityEngine;

public class HoverMaterialSwap : MonoBehaviour
{
    [Header("Materials")]
    public Material outlineMaterial;    // Assign this to your outline material
    public Material defaultMaterial;    // Assign this to your normal material

    private SpriteRenderer bushRenderer;

    private void Awake()
    {
        // Find the "Bush" child object and get its SpriteRenderer
        Transform bush = transform.Find("Bush");
        if (bush != null)
        {
            bushRenderer = bush.GetComponent<SpriteRenderer>();
        }

        if (bushRenderer == null)
        {
            Debug.LogError($"{gameObject.name}: Bush SpriteRenderer not found.");
            return;
        }

        // Assign a unique copy of the default material to avoid shared reference issues
        if (defaultMaterial != null)
        {
            bushRenderer.material = new Material(defaultMaterial);
        }
        else
        {
            // If no material is assigned, clone the current one
            bushRenderer.material = new Material(bushRenderer.material);
        }
    }

    private void OnMouseEnter()
    {
        if (bushRenderer != null && outlineMaterial != null)
        {
            bushRenderer.material = new Material(outlineMaterial);
        }
    }

    private void OnMouseExit()
    {
        if (bushRenderer != null && defaultMaterial != null)
        {
            bushRenderer.material = new Material(defaultMaterial);
        }
    }
}
