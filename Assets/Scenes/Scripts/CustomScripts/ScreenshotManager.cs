using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

// Generate a screenshot and save it to disk with the name SomeLevel.png.

public class ScreenshotManager : MonoBehaviour
{
    public GameObject CapturePanel;

    public Button OKButton;

    public string file_path = "";

    public void Update()
    {
        if (file_path != "")
        {
            if(File.Exists(file_path))
            {
                CapturePanel.SetActive(true);
                file_path = "";
            }
        }
    }

    public void CaptureScreen()
    {
        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");
        string fileName = "[PaintTheCity] Screenshot " + timestamp + ".png";
        
        #if UNITY_IPHONE || UNITY_ANDROID
        CaptureScreenForMobile(fileName);
        #else
        CaptureScreenForPC(fileName);
        #endif
    }

    public void OKButtonClick()
    {
        CapturePanel.SetActive(false);
    }

    private void CaptureScreenForPC(string fileName)
    {
        file_path = $"{Application.dataPath}/" + fileName;
        ScreenCapture.CaptureScreenshot(file_path);
        Debug.Log("[PC] 저장 경로 = " + file_path);
    }

    private void CaptureScreenForMobile(string fileName)
    {
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();
        
        // do something with texture
        NativeGallery.Permission permission = NativeGallery.CheckPermission(NativeGallery.PermissionType.Write, NativeGallery.MediaType.Image);
        if (permission == NativeGallery.Permission.Denied)
        {
            if (NativeGallery.CanOpenSettings())
            {
                NativeGallery.OpenSettings();
            }
        } 
        
        string albumName = "PaintTheCity";
        NativeGallery.SaveImageToGallery(texture, albumName, fileName, (success, path) =>
        {
            Debug.Log(success);
            Debug.Log(path);
            file_path = path;
            Debug.Log("[MOBILE] 저장할 경로 = " + file_path);
        });
        
        // cleanup
        Object.Destroy(texture);
    }
}