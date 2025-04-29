using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class BSTNodeGlow : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private static Bloom bloom;
    private static float defaultIntensity;
    private static bool bloomInitialized = false;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (!bloomInitialized)
        {
            Volume bloomVolume = FindAnyObjectByType<Volume>();

            if (bloomVolume != null && bloomVolume.profile.TryGet(out bloom))
            {
                defaultIntensity = bloom.intensity.value;
                bloomInitialized = true;
            }
            else
            {
                Debug.LogWarning("Could not find Bloom in Volume profile!");
            }
        }
    }

    public void EnableGlow()
    {
        if (spriteRenderer != null)
        {
            Debug.Log($"{gameObject.name}: Glow ENABLED");
        }

        if (bloom != null)
        {
            bloom.intensity.value += 3f; // <<< Set intensity lower to "remove" bloom
        }
    }

    public void DisableGlow()
    {
        if (spriteRenderer != null)
        {
            Debug.Log($"{gameObject.name}: Glow DISABLED");
        }

        if (bloom != null)
        {
            bloom.intensity.value = defaultIntensity; // <<< Restore original bloom intensity
        }
    }
}