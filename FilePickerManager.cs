using UnityEngine;
using UnityEngine.UI;
using System.IO;
using SFB; 

public class FilePickerManager : MonoBehaviour
{
    public Image previewImage;
    private string selectedFilePath;
    private AuthManager authManager;

    private void Start()
    {
        authManager = FindObjectOfType<AuthManager>();
    }

    public void OpenFilePicker()
    {

        string[] paths = StandaloneFileBrowser.OpenFilePanel("Select an Image", "", "png,jpg,jpeg", false);

        if (paths.Length > 0 && !string.IsNullOrEmpty(paths[0]))
        {
            selectedFilePath = paths[0]; 
            DisplayImage(selectedFilePath);
        }
        else
        {
            Debug.LogError("No image selected.");
        }
    }

    private void DisplayImage(string filePath)
    {
        if (string.IsNullOrEmpty(filePath)) return;

        byte[] imageBytes = File.ReadAllBytes(filePath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(imageBytes);
        previewImage.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));

        
        authManager.UploadAvatar(filePath);
    }
}
