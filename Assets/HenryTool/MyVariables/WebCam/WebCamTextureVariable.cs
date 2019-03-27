using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


namespace HenryTool
{
    [CreateAssetMenu]
    public class WebCamTextureVariable : ScriptableObject
    {
        private int currentWebCamIndex;
        public int webCamIndex;

        public int wcWidth, wcHeight, wcFps;

        public Material[] materials;

        private WebCamTexture _theWebCam;
        public WebCamTexture theWebCam
        {
            get {
                if (currentWebCamIndex != webCamIndex) {
                    if (_theWebCam != null) {
                        _theWebCam.Stop();
                        _theWebCam = null;
                    }
                }

                if (_theWebCam == null) {
                    if (WebCamTexture.devices.Length > 0) {
                        currentWebCamIndex = GetIndex();

                        if ((wcWidth > 0) && (wcHeight > 0))
                            _theWebCam = new WebCamTexture(WebCamTexture.devices[webCamIndex].name, wcWidth, wcHeight, GetFps());
                        else
                            _theWebCam = new WebCamTexture(WebCamTexture.devices[webCamIndex].name);

                    }
#if UNITY_EDITOR
                    else {
                        objectDescription = "No WebCam";
                    }
#endif

                }

                return _theWebCam;
            }
        }

        public bool isReady
        {
            get {
                return (theWebCam != null);
            }
        }

        public bool isPlaying
        {
            get {
                if (theWebCam == null)
                    return false;
                else
                    return theWebCam.isPlaying;
            }
        }

        public int screenWidth
        {
            get {
                if (theWebCam != null)
                    return theWebCam.width;
                else
                    return -1;
            }

        }

        public int screenHeight
        {
            get {
                if (theWebCam != null)
                    return theWebCam.height;
                else
                    return -1;
            }

        }


        public void Pause()
        {
            if (theWebCam != null)
                theWebCam.Pause();
        }

        public void Stop()
        {
            if (theWebCam != null)
                theWebCam.Stop();
        }

        public void Play()
        {
            if (theWebCam != null)
                if (!theWebCam.isPlaying) {
                    foreach (Material mat in materials) {
                        mat.mainTexture = theWebCam;
                    }

                    theWebCam.Play();

#if UNITY_EDITOR
                    objectDescription = "is playing.";
                    width = wcWidth;
                    height = wcHeight;
#endif
                }
        }

        public Texture2D TakePhoto(string _filePath, int _width, int _height)
        {
            return TakePhoto(_filePath, 0, _width, _height);

        }

        public Texture2D TakePhoto(string _filePath, int _offset, int _width, int _height)
        {
            int oriX = 0;
            int oriY = 0;

            if (_width > theWebCam.width)
                _width = theWebCam.width;
            else if (_width < theWebCam.width)
                oriX = (theWebCam.width - _width) / 2;

            if (_height > theWebCam.height)
                _height = theWebCam.height;
            else if (_height < theWebCam.height)
                oriY = (theWebCam.height - _height) / 2;

            int tempI = oriX + _offset;
            if (tempI < 0)
                oriX = 0;
            else if (tempI > (theWebCam.width - _width))
                oriX = (theWebCam.width - _width);
            else
                oriX = oriX + _offset;

            Texture2D thePhoto = new Texture2D(_width, _height);
            //thePhoto.SetPixels(theWebCam.GetPixels(oriX, oriY, thePhoto.width, thePhoto.height));
            thePhoto.SetPixels(theWebCam.GetPixels(oriX, oriY, _width, _height));
            thePhoto.Apply();

#if USE_SHADER
        System.IO.File.WriteAllBytes(filePath + "JPG.jpg", thePhoto.EncodeToPNG());  // 如果你要保存至硬碟or記憶卡的話
        System.IO.File.WriteAllBytes(filePath + "PNG.png", thePhoto.EncodeToJPG());  // 如果你要保存至硬碟or記憶卡的話

        RenderTexture newTexture = new RenderTexture(theWidth, theHeight, 16);

        Graphics.Blit(thePhoto, newTexture, takenPhoto.material);

        RenderTexture savedRt = RenderTexture.active;

        RenderTexture.active = newTexture;

        thePhoto.ReadPixels(new Rect(0, 0, thePhoto.width, thePhoto.height), 0, 0);

        RenderTexture.active = savedRt;
#endif


#if UNITY_IOS
            return FlipTexture(thePhoto);

#else
            System.IO.File.WriteAllBytes(_filePath + "0.jpg", thePhoto.EncodeToJPG());  // 如果你要保存至硬碟or記憶卡的話
            return thePhoto;
#endif

        }

        Texture2D FlipTexture(Texture2D _originalTex)
        {
            Texture2D flipped = new Texture2D(_originalTex.height, _originalTex.width);

            int xN = flipped.width;
            int yN = flipped.height;

            for (int i = 0; i < xN; i++) {
                for (int j = 0; j < yN; j++) {
                    flipped.SetPixel(i, j, _originalTex.GetPixel(yN - j - 1, i));
                }
            }
            flipped.Apply();

            return flipped;
        }


        private int GetIndex()
        {
            if (webCamIndex < 0)
                webCamIndex = 0;
            else if (webCamIndex >= WebCamTexture.devices.Length)
                webCamIndex = WebCamTexture.devices.Length - 1;

            return webCamIndex;
        }

        private int GetFps()
        {
            if (wcFps < 10)
                wcFps = 30;
            else if (wcFps > 60)
                wcFps = 60;

            return wcFps;
        }


#if UNITY_EDITOR
        [Multiline]
        public string objectDescription;

        public int webCamCount;
        //public int webCamIndex;
        public int width;
        public int height;
        public int fps;

#endif

    }
}
