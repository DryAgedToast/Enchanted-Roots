using UnityEngine;
using UnityEngine.UI;

public class TreeSelectRange : MonoBehaviour
{
    public Button Tree;
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player entered the trigger!");
            Tree.interactable = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Tree.interactable = false;
        }
    }
}
