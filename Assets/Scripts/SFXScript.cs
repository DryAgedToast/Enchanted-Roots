using UnityEngine;

public class SFXScript : MonoBehaviour
{
    public static SFXScript instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip insertSound;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayInsertSound()
    {
        if (audioSource != null && insertSound != null)
        {
            audioSource.PlayOneShot(insertSound);
        }
    }
}
