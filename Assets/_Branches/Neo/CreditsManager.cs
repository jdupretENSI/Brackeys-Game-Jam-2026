using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsManager : MonoBehaviour
{
    [Header("Crédits")]
    public RectTransform creditsPanel;

    void Start()
    {
        StartCoroutine(ScrollCredits());
    }

    IEnumerator ScrollCredits()
    {
        // Scroll pendant 60s
        float duration = 60f;
        Vector2 startPos = creditsPanel.anchoredPosition;
        Vector2 endPos = startPos + Vector2.up * 3500f;

        float time = 0;
        while (time < duration)
        {
            time += Time.deltaTime;
            float normalizedTime = time / duration;
            creditsPanel.anchoredPosition = Vector2.Lerp(startPos, endPos, normalizedTime);
            yield return null;
        }

        // Retour MainMenu après 3s
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("MainMenu");
    }
}
