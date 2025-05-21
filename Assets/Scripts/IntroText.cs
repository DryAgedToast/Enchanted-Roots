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

    public AudioSource audioSource;         // Add this
    public AudioClip characterBlipSound;    // And this

    private int index = 0;
    private bool isTyping = false;
    private static bool DoneOnce = false;
    private Coroutine typingCoroutine;

    void Start()
    {
        playerMovement.canMove = false;
        if (LevelLock.levelComplete > 0 || DoneOnce)
        {
            introBox.SetActive(false);
            playerMovement.canMove = true;
        }
        dialogueText.text = "";
        typingCoroutine = StartCoroutine(TypeLine());
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
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
                    DoneOnce = true;
                }
            }
        }
        if (Input.GetKeyDown(KeyCode.Q))
        {
            playerMovement.canMove = true;
            enabled = false;
            introBox.SetActive(false);
            DoneOnce = true;
        }
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in lines[index])
        {
            dialogueText.text += c;

            // Only play sound for visible characters (not spaces, punctuation if desired)
            if (!char.IsWhiteSpace(c) && audioSource && characterBlipSound)
            {
                audioSource.PlayOneShot(characterBlipSound, 0.6f); // volume can be tweaked
            }

            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }
}
