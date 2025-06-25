using UnityEngine;
using Firebase;
using Firebase.Auth;
using System.Collections;

public class FirebaseManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseUser user;

    private void Start()
    {
        StartCoroutine(InitializeFirebase());
    }

    private IEnumerator InitializeFirebase()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                auth = FirebaseAuth.DefaultInstance;
                Debug.Log("Firebase Initialized");
            }
            else
            {
                Debug.LogError("Firebase Dependencies Error: " + task.Result);
            }
        });
        yield return null;
    }
}
