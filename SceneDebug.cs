using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneDebug : MonoBehaviour
{
    void Start()
    {
        Debug.Log("Current Scene: " + SceneManager.GetActiveScene().name);
    }
}
