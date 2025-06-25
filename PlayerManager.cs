using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager instance;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }
}
