using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeLevel : MonoBehaviour
{
    public string LevelName;
    public GameObject player;

    public void LoadLevel()
    {
        Vector3 clickPosition = player.transform.position;
        PlayerMovement.spawnPosition = clickPosition;

        SceneManager.LoadScene(LevelName);
    }

}
