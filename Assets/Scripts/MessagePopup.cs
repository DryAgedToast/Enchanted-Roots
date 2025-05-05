using TMPro;
using UnityEngine;

public class MessagePopup : MonoBehaviour
{
    public static MessagePopup instance;

    private TMP_Text popupText;
    private CanvasGroup canvasGroup;
    private float displayTime = 2f;
    private float fadeSpeed = 2f;
    private float timer;
    void Awake()
    {
        instance = this;
        popupText = GetComponent<TMP_Text>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (canvasGroup.alpha > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, 0, fadeSpeed * Time.deltaTime);
            }
        }
    }

    public void ShowMessage(string message) 
    {
        popupText.text = message;
        canvasGroup.alpha = 1;
        timer = displayTime;
    }
}
