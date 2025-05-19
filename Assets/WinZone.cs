using UnityEngine;

public class WinZone : MonoBehaviour
{
    public Winning winningScript;
    public float newGravityScale = 0.1f;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = newGravityScale;
            }
            Debug.Log("Triggered Win Zone");
            winningScript.StartFloatingTextSequence(); // ✅ Start floating text externally
            gameObject.SetActive(false); // Optional: prevent retriggering
        }
    }
}

