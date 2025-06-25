using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public GameObject profilePanel; 
    public GameObject blurPanel; 
    public Button profileButton;
    public Button closeButton; 
    private CanvasGroup canvasGroup;

    void Start()
    {
        
        canvasGroup = profilePanel.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
            canvasGroup = profilePanel.AddComponent<CanvasGroup>();

        
        canvasGroup.alpha = 0f;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        blurPanel.SetActive(false); 

       
        profileButton.onClick.AddListener(() => StartCoroutine(FadeInPanel()));
        closeButton.onClick.AddListener(() => StartCoroutine(FadeOutPanel()));
    }

    IEnumerator FadeInPanel()
    {
        profilePanel.SetActive(true);
        blurPanel.SetActive(true); 
        float elapsedTime = 0f;
        float duration = 0.3f;

        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
        canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
    }

    IEnumerator FadeOutPanel()
    {
        float elapsedTime = 0f;
        float duration = 0.3f;

        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;

        while (elapsedTime < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(1f, 0f, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
        profilePanel.SetActive(false);
        blurPanel.SetActive(false); 
    }
}
