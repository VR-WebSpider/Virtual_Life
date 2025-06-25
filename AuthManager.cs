using UnityEngine;
using UnityEngine.UI;
using Firebase;
using Firebase.Auth;
using Firebase.Firestore;
using System.Threading.Tasks;
using TMPro;
using Firebase.Storage;
using System.IO;
using UnityEngine.Networking;
using System.Collections;
using Firebase.Extensions;

public class AuthManager : MonoBehaviour
{
    private FirebaseAuth auth;
    private FirebaseFirestore db;
    private FirebaseStorage storage;
    private StorageReference storageRef;

    public Image avatarImage;
    public Button selectAvatarButton, uploadAvatarButton;
    private string localAvatarPath;

    public InputField emailInput, passwordInput;
    public TMP_Text profileEmailText, profileNameText;
    public TMP_Text statusText;
    public GameObject loginPanel;

    public TMP_InputField updateNameInput;

    private void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
        db = FirebaseFirestore.DefaultInstance;
        storage = FirebaseStorage.DefaultInstance;
        storageRef = storage.GetReferenceFromUrl("gs://your-firebase-project.appspot.com");
    }

    public async void RegisterUser()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        try
        {
            FirebaseUser newUser = (await auth.CreateUserWithEmailAndPasswordAsync(email, password)).User;
            statusText.text = "Registration Successful!";
            await StoreUserData(newUser.UserId, email);
        }
        catch (System.Exception e)
        {
            statusText.text = "Registration Failed: " + e.Message;
        }
    }

    public async void LoginUser()
    {
        string email = emailInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
        {
            statusText.text = "Please enter email and password!";
            return;
        }

        try
        {
            FirebaseUser loggedInUser = (await auth.SignInWithEmailAndPasswordAsync(email, password)).User;
            statusText.text = "Login Successful!";
            await RetrieveUserData(loggedInUser.UserId);
        }
        catch (System.Exception e)
        {
            statusText.text = "Login Failed: " + e.Message;
        }
    }

    public void LogoutUser()
    {
        auth.SignOut();
        statusText.text = "Logged out.";
        Invoke(nameof(UpdateLogoutUI), 0);
    }

    private void UpdateLogoutUI()
    {
        profileEmailText.text = "Email: Loading...";
        profileNameText.text = "Name: Loading...";
        avatarImage.sprite = null; 
        loginPanel.SetActive(true);
        statusText.text = "Please log in.";
    }

    private async Task StoreUserData(string userId, string email)
    {
        DocumentReference docRef = db.Collection("users").Document(userId);
        var user = new { email = email, displayName = "Guest", avatarUrl = "" };
        await docRef.SetAsync(user);
    }

    private async Task RetrieveUserData(string userId)
    {
        DocumentReference docRef = db.Collection("users").Document(userId);

        try
        {
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                string email = snapshot.GetValue<string>("email");
                string displayName = snapshot.GetValue<string>("displayName");
                string avatarUrl = snapshot.ContainsField("avatarUrl") ? snapshot.GetValue<string>("avatarUrl") : "";

                profileEmailText.text = "Email: " + email;
                profileNameText.text = "Name: " + displayName;

                if (!string.IsNullOrEmpty(avatarUrl))
                {
                    StartCoroutine(LoadAvatar(avatarUrl));
                }

                loginPanel.SetActive(false);
            }
            else
            {
                Debug.LogWarning("No user data found!");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Error retrieving user data: " + e.Message);
        }
    }

    public void UploadAvatar(string filePath)
    {
        if (string.IsNullOrEmpty(filePath))
        {
            Debug.LogError("No image selected for upload.");
            return;
        }

        FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
        if (user == null)
        {
            Debug.LogError("User not logged in.");
            return;
        }

        string fileName = user.UserId + "_avatar.png";
        StorageReference avatarRef = storageRef.Child("avatars/" + fileName);

        byte[] fileBytes = File.ReadAllBytes(filePath);
        avatarRef.PutBytesAsync(fileBytes).ContinueWithOnMainThread(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
            {
                Debug.LogError("Upload failed: " + task.Exception);
            }
            else
            {
                avatarRef.GetDownloadUrlAsync().ContinueWithOnMainThread(urlTask =>
                {
                    if (urlTask.IsCompleted)
                    {
                        string downloadUrl = urlTask.Result.ToString();
                        Debug.Log("Avatar uploaded successfully: " + downloadUrl);
                        SaveAvatarUrlToFirestore(user.UserId, downloadUrl);
                    }
                });
            }
        });
    }

    private async void SaveAvatarUrlToFirestore(string userId, string url)
    {
        DocumentReference docRef = db.Collection("users").Document(userId);
        await docRef.UpdateAsync("avatarUrl", url);
        Debug.Log("Avatar URL updated in Firestore.");
    }

    private IEnumerator LoadAvatar(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            avatarImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
        }
        else
        {
            Debug.LogError("Error loading avatar: " + request.error);
        }
    }
}
