using System.Collections;
using UnityEngine;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public TMP_Text dialogueText;
    public string[] lines;
    public float typingSpeed = 0.04f;
    public PlayerMovement playerMovement;
    public GameObject introBox;

    private int index = 0;
    private bool isTyping = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        dialogueText.text = "";
        playerMovement.canMove = false;
        typingCoroutine = StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // Skip typing and show full line
                StopCoroutine(typingCoroutine);
                dialogueText.text = lines[index];
                isTyping = false;
            }
            else
            {
                index++;
                if (index < lines.Length)
                {
                    typingCoroutine = StartCoroutine(TypeLine());
                }
                else
                {
                    dialogueText.text = "";
                    playerMovement.canMove = true;
                    enabled = false;
                    introBox.SetActive(false);
                }
            }
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in lines[index])
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
