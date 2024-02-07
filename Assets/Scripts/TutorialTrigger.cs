using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialTrigger : MonoBehaviour
{
    [SerializeField] private GameObject tutorialImage;
    private CanvasGroup tutorialCanvasGroup;
    public AudioSource tutOpen;
    public AudioSource tutClose;

    private void Awake()
    {
        tutorialCanvasGroup = tutorialImage.GetComponent<CanvasGroup>();
        if (tutorialCanvasGroup == null)
        {
            tutorialCanvasGroup = tutorialImage.AddComponent<CanvasGroup>();
        }
        tutorialCanvasGroup.alpha = 0;
        tutorialImage.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ShowTutorial();
        }
    }

    private void ShowTutorial()
    {
        tutOpen.Play();
        tutorialImage.SetActive(true);
        StartCoroutine(FadeCanvasGroup(tutorialCanvasGroup, 0, 1, 0.5f));
        Time.timeScale = 0f;
        StartCoroutine(WaitForInput());
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup canvasGroup, float start, float end, float duration)
    {
        tutClose.Play();
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(start, end, elapsedTime / duration);
            elapsedTime += Time.unscaledDeltaTime;
            yield return null;
        }
        canvasGroup.alpha = end;
    }

    private IEnumerator WaitForInput()
    {
        bool inputReceived = false;
        while (!inputReceived)
        {
            if (Input.anyKeyDown)
            {
                inputReceived = true;
            }
            yield return null;
        }

        StartCoroutine(FadeCanvasGroup(tutorialCanvasGroup, 1, 0, 0.5f));
        yield return new WaitForSecondsRealtime(0.5f);
        tutorialImage.SetActive(false);
        Time.timeScale = 1f;
        GameManager.Instance.tutorialHitboxes.Remove(gameObject);
        Destroy(gameObject);
    }
}
