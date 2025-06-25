using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public void LoadHomeScene()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("HomeScene"); 
    }

    public void LoadSceneOne()
    {
        Time.timeScale = 1f; 
        SceneManager.LoadScene("SceneOne"); 
    }
}
