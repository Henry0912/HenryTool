using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class ServerCtl : MonoBehaviour
{

    public VideoPlayer videoPlayer;


    // Use this for initialization
    public void InitStart() {
        //videoPlayer.aspectRatio = VideoAspectRatio.NoScaling;
        //videoPlayer.url = "file://E:/Projects/UNet/Assets/HenryTool/TestFolder/tears_of_steel_720p.mov";
        //videoPlayer.Play();

    }

    // Update is called once per frame
    void Update() {

    }

    public void PlayVideo(string _path) {
        if (videoPlayer.isPlaying) {
            videoPlayer.Stop();
        }

        videoPlayer.url = _path;

        videoPlayer.Play();

    }


}
