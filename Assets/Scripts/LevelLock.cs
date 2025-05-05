using UnityEngine;

public class LevelLock : MonoBehaviour
{
    public static int levelComplete = 0;
    public GameObject level2Barrier;
    public GameObject level3Barrier;
    public GameObject outsideBarrier;
    void Start()
    {
        if(levelComplete >= 3){
            outsideBarrier.SetActive(false);
            level3Barrier.SetActive(false);
            level2Barrier.SetActive(false);
        } else if(levelComplete == 2){
            level3Barrier.SetActive(false);
            level2Barrier.SetActive(false);
        } else if(levelComplete == 1){
            level2Barrier.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
