using UnityEngine;
using UnityEngine.UI;

public class LevelLock : MonoBehaviour
{
    public static int levelComplete = 0;
    public GameObject level2Barrier;
    public GameObject level3Barrier;
    public GameObject outsideBarrier;
    public Image tree1;
    public Image tree2;
    public Image tree3;
    public Sprite fixedTree;
    void Start()
    {
        if (levelComplete >= 3)
        {
            outsideBarrier.SetActive(false);
            level3Barrier.SetActive(false);
            level2Barrier.SetActive(false);
            tree1.sprite = fixedTree;
            tree2.sprite = fixedTree;
            tree3.sprite = fixedTree;
        }
        else if (levelComplete == 2)
        {
            level3Barrier.SetActive(false);
            level2Barrier.SetActive(false);
            tree1.sprite = fixedTree;
            tree2.sprite = fixedTree;
        }
        else if (levelComplete == 1)
        {
            level2Barrier.SetActive(false);
            tree1.sprite = fixedTree;
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
