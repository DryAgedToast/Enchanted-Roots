using UnityEngine;
using UnityEngine.UI;

public class TutorialPopup : MonoBehaviour
{
    
    public Image tutorialImage;
    public Sprite[] tutorialPages;
    public GameObject popupUI; // parent container for canvas group
    public GameObject nodeCanvas;
    private int currentPage = 0;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ShowPage(0);
        if (nodeCanvas != null) {
            nodeCanvas.SetActive(false);
        }
    }

    // Update is called once per frame
    void ShowPage(int pageIndex)
    {
        if(pageIndex >= 0 && pageIndex < tutorialPages.Length) {
            tutorialImage.sprite = tutorialPages[pageIndex];
            currentPage = pageIndex;
        }
    }

    public void NextPage() 
    {
        int nextPage = currentPage + 1;
        if (nextPage < tutorialPages.Length) {
            ShowPage(nextPage);
        }
        else {
            CloseTutorial();
        }
    }
    public void CloseTutorial()
    {
        popupUI.SetActive(false);
        nodeCanvas.SetActive(true);
        Time.timeScale = 1f;
    }
}
