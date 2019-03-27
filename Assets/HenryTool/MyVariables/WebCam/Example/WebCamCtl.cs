//# define USE_GUI
using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using HenryTool;

public class WebCamCtl : MonoBehaviour
{
    public LogStringVariable errorLog;
    public WebCamTextureVariable WCTexture;

    private void Start() {
        InitStart();
    }

    public void InitStart() {
        //WCTexture.InitWebCam(0, 1920, 1080, 30, true);

        WCTexture.Play();

    }

    void OnApplicationQuit() {
        if (WCTexture != null) {
            if (WCTexture.isPlaying) {
                errorLog.AddLogLine("OnApplicationQuit: stop webcam.");
                WCTexture.Pause();
            }

            StartCoroutine(WaitForWebCamStop());
        }
    }

    void OnDisable() {
        if (WCTexture != null) {
            WCTexture.Pause();
            WCTexture.Stop();
        }
    }

    IEnumerator CheckWebCamPlaying() {
        yield return null;

        while (!WCTexture.isPlaying) {
            yield return null;
        }

    }

    IEnumerator WaitForWebCamStop() {
        while (WCTexture.isPlaying) {
            yield return null;
        }

        WCTexture.Stop();

    }

    public void PauseWebCam() {

        if (WCTexture != null) {
            WCTexture.Pause();
        }
    }

    public void StopWebCam() {
        /*
        if (theWebCamTexture != null) {
            theWebCamTexture.Stop();
        }
        */

        if (WCTexture != null) {
            if (WCTexture.isPlaying) {
                WCTexture.Pause();
            }

            StartCoroutine(WaitForWebCamStop());
        }

    }

    public Texture2D TakePhoto(string filePath) {
        int theWidth = WCTexture.wcWidth / 2;
        int theHeight = WCTexture.wcHeight;
        errorLog.AddLogLine("r W: " + theWidth + ", H: " + theHeight);
        //theWidth = 720;
        //theHeight = 720;

        theHeight = (WCTexture.wcWidth < WCTexture.wcHeight) ? WCTexture.wcWidth : WCTexture.wcHeight;
        theWidth = theHeight;

        theHeight = 1080;
        theWidth = 1350;

        errorLog.AddLogLine("W: " + theWidth + ", H: " + theHeight);

        //Texture2D thePhoto = new Texture2D(theWidth, theHeight);
        Texture2D thePhoto = new Texture2D(theHeight, theWidth);

        //thePhoto.SetPixels(theWebCamTexture.GetPixels(theWebCamTexture.width / 4, 0, thePhoto.width, thePhoto.height));
        //thePhoto.SetPixels(theWebCamTexture.GetPixels(((theWebCamTexture.width - theWidth) / 2) + 24, 0, thePhoto.width, thePhoto.height));

        //int offset = (int)(ConstObj.CAMERA_ADJ * 1.5f);
        Color[] pixels = WCTexture.theWebCam.GetPixels(((WCTexture.wcWidth - theWidth) / 2) + 0, 0, theWidth, theHeight);

        Color[] pixelsTemp = new Color[theWidth * theHeight];

        for (int i = 0; i < theWidth; i++) {
            for (int j = 0; j < theHeight; j++) {
                int oldInt = (j * theWidth) + i;
                int newInt = ((theWidth - i - 1) * theHeight) + j;

                if (oldInt >= pixels.Length) {
                    errorLog.AddLogLine("old: " + oldInt);
                }
                if (newInt >= pixelsTemp.Length) {
                    errorLog.AddLogLine("new : " + newInt);
                }

                //pixelsTemp[((theWidth - j - 1) * theHeight) + i] = pixels[(i*theWidth) + j];
                pixelsTemp[newInt] = pixels[oldInt];

            }
        }

        //thePhoto.SetPixels(theWebCamTexture.GetPixels(((theWebCamTexture.width - theWidth) / 2) + ConstObj.CAMERA_ADJ, 0, thePhoto.width, thePhoto.height));
        thePhoto.SetPixels(pixelsTemp);

        thePhoto.Apply();

        //theLce.texture2D = thePhoto;
        //takenPhoto.material.mainTexture = thePhoto;

#if USE_SHADER
        //System.IO.File.WriteAllBytes(filePath + "JPG.jpg", thePhoto.EncodeToPNG());  // 保存至硬碟or記憶卡
        //System.IO.File.WriteAllBytes(filePath + "PNG.png", thePhoto.EncodeToJPG());  // 保存至硬碟or記憶卡

        RenderTexture newTexture = new RenderTexture(theWidth, theHeight, 16);

        Graphics.Blit(thePhoto, newTexture, takenPhoto.material);

        RenderTexture savedRt = RenderTexture.active;

        RenderTexture.active = newTexture;

        thePhoto.ReadPixels(new Rect(0, 0, thePhoto.width, thePhoto.height), 0, 0);

        RenderTexture.active = savedRt;
#endif
        //System.IO.File.WriteAllBytes(filePath + "0.jpg", thePhoto.EncodeToJPG());  // 保存至硬碟or記憶卡

        return thePhoto;

    }



#if USE_GUI
    void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 50), "拍照"))
        {
            Texture2D thePhoto = new Texture2D(theWebCamTexture.width, theWebCamTexture.height);
            thePhoto.SetPixels(theWebCamTexture.GetPixels());
            thePhoto.Apply();
            theLce.texture2D = thePhoto;
            //theGo.GetComponent<Renderer>().material.mainTexture = thePhoto;
            takenPhoto.material.mainTexture = thePhoto;
            System.IO.File.WriteAllBytes("imageTest.png", thePhoto.EncodeToPNG());  // 如果你要保存至硬碟or記憶卡的話
        }


        if (GUI.Button(new Rect(130, 0, 100, 50), "start"))
        {
            InitStart();
        }

        if (GUI.Button(new Rect(130, 70, 100, 50), "stop"))
        {
            StopWebCam();
        }
    }

#endif


}



