using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotCamera : MonoBehaviour
{
    public KeyCode snapKey = KeyCode.Mouse0;
    private PlayerCam playerCam;
    public Camera smallCamera;
    public RenderTexture renderTexture;
    public Image screenshotDisplay; // Drag the UI Image here in the Inspector

    private string lastScreenshotPath;

    void Start()
    {
        playerCam = FindObjectOfType<PlayerCam>();
    }

    void Update()
    {
        if (playerCam != null && Input.GetKeyDown(snapKey) && playerCam.isCameraRaised)
        {
            TakeScreenshot();
        }
    }

    void TakeScreenshot()
    {
        if (smallCamera == null || renderTexture == null)
        {
            Debug.LogError("Small Camera or RenderTexture is not assigned!");
            return;
        }

        // Temporarily enable the small camera for rendering
        smallCamera.enabled = true;
        RenderTexture.active = renderTexture;
        smallCamera.targetTexture = renderTexture; // left bottom picture saver
        smallCamera.Render();

        Texture2D screenshot = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        screenshot.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        byte[] bytes = screenshot.EncodeToPNG();

        //makes screenshot in file
        lastScreenshotPath = Path.Combine(Application.persistentDataPath, "screenshot-" + System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png"); //saves picture
        File.WriteAllBytes(lastScreenshotPath, bytes);
        Debug.Log("Screenshot saved: " + lastScreenshotPath);
        // Show the screenshot in UI
        ShowScreenshot();
    }

    void ShowScreenshot()
    {
        StartCoroutine(LoadScreenshotCoroutine());
    }

    System.Collections.IEnumerator LoadScreenshotCoroutine()
    {
        yield return new WaitForEndOfFrame();

        byte[] fileData = File.ReadAllBytes(lastScreenshotPath);
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(fileData);

        screenshotDisplay.sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f, 0.5f));
    }
}