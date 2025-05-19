using System.Collections;
using UnityEngine;
using TMPro;

public class Winning : MonoBehaviour
{
    public GameObject floatingTextPrefab; // Prefab with TMP_Text and optional CanvasGroup
    public Transform spawnPoint;          // Starting position off-screen
    public Vector3 targetMidpoint = new Vector3(0, 0, 0); // Midpoint pause
    public float riseSpeed = 30f;
    public float pauseTime = 1.5f;
    public float extraRiseDistance = 100f;

    public string[] lines;
    public float startDelay = 1f;         // Delay before first line
    public float timeBetweenLines = 1.25f; // Delay between each line

    private bool hasStarted = false;

    public void StartFloatingTextSequence()
    {
        if (!hasStarted)
        {
            hasStarted = true;
            StartCoroutine(PlayFloatingTextSequence());
        }
    }

    IEnumerator PlayFloatingTextSequence()
    {
        yield return new WaitForSeconds(startDelay);

        foreach (string line in lines)
        {
            GameObject textObj = Instantiate(floatingTextPrefab, spawnPoint.position, Quaternion.identity, spawnPoint.parent);
            TMP_Text tmp = textObj.GetComponent<TMP_Text>();
            tmp.text = line;

            CanvasGroup canvasGroup = textObj.GetComponent<CanvasGroup>();
            StartCoroutine(MoveText(textObj.transform, canvasGroup));

            yield return new WaitForSeconds(timeBetweenLines);
        }
    }

    IEnumerator MoveText(Transform textTransform, CanvasGroup canvasGroup)
    {
        Vector3 midTarget = targetMidpoint;
        Vector3 finalTarget = midTarget + Vector3.up * extraRiseDistance;

        // Step 1: Move to midpoint
        while (Vector3.Distance(textTransform.localPosition, midTarget) > 1f)
        {
            textTransform.localPosition = Vector3.MoveTowards(textTransform.localPosition, midTarget, riseSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(pauseTime);

        // Step 2: Continue rising and fade
        float fadeDuration = 1.5f;
        float t = 0f;

        while (Vector3.Distance(textTransform.localPosition, finalTarget) > 1f)
        {
            textTransform.localPosition += Vector3.up * riseSpeed * Time.deltaTime;

            if (canvasGroup != null)
            {
                t += Time.deltaTime / fadeDuration;
                canvasGroup.alpha = 1 - Mathf.Clamp01(t);
            }

            yield return null;
        }

        Destroy(textTransform.gameObject);
    }
}
