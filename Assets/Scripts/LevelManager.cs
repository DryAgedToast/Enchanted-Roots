using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameObject levelClearPanel;

    // Call this when the level is complete
    public void OnLevelComplete()
    {
        Time.timeScale = 0f; // Optional: pause the game
        levelClearPanel.SetActive(true);
    }
}