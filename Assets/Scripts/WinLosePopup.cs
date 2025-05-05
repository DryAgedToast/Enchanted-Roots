using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class WinLosePopup : MonoBehaviour
{
    [SerializeField] private TMP_Text titleText;
    [SerializeField] private GameObject winImage;
    [SerializeField] private GameObject loseImage;

    public void Setup(bool won)
    {
        titleText.text = won ? "You Won!" : "Game Over!";
        winImage.SetActive(won);
        loseImage.SetActive(!won);
    }

    public void OnRetryPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnLevelSelectPressed()
    {
        SceneManager.LoadScene("LevelSelect"); // Adjust as needed
    }
}
