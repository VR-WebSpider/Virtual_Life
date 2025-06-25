using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class TravelManager : MonoBehaviour
{
    public GameObject travelMenu; 
                
    public Image loadingIcon; 
    public float fadeDuration = 0.5f; 

    public void OpenTravelMenu()
    {
        StartCoroutine(ShowPanel(travelMenu));
    }

    public void CloseTravelMenu()
    {
        StartCoroutine(HidePanel(travelMenu));
    }

    public void TravelToScene(string sceneName)
    {
        StartCoroutine(HidePanel(travelMenu, () => StartCoroutine(LoadSceneAsync(sceneName))));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        StartCoroutine(AnimateLoadingIcon());

        yield return new WaitForSeconds(1);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            if (operation.progress >= 0.9f)
            {
                yield return new WaitForSeconds(1); 
                operation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    IEnumerator AnimateLoadingIcon()
    {
        {
            loadingIcon.transform.Rotate(0, 0, -200 * Time.deltaTime);
            yield return null;
        }
    }

    IEnumerator ShowPanel(GameObject panel)
    {
        panel.SetActive(true);
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = panel.AddComponent<CanvasGroup>();

        float elapsedTime = 0;
        float duration = 0.3f;

        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0, 1, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1;
    }


    IEnumerator HidePanel(GameObject panel, System.Action onComplete = null)
    {
        CanvasGroup canvasGroup = panel.GetComponent<CanvasGroup>();
        if (canvasGroup == null) canvasGroup = panel.AddComponent<CanvasGroup>();

        float elapsedTime = 0;
        float duration = 0.3f;

        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(1, 0, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0;
        panel.SetActive(false);
        onComplete?.Invoke();
    }

}