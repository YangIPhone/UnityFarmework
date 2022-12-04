using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class VideoPlayerDemo : MonoBehaviour
{
    public VideoClip video1;
    private VideoPlayer videoPlayer;
    // Start is called before the first frame update
    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.isLooping = true;//设置循环播放
        videoPlayer.clip = video1;
        videoPlayer.playbackSpeed = 1.5f;//设置播放速度
        videoPlayer.Play();//开始播放
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (videoPlayer.isPlaying)
            {
                videoPlayer.Pause();
            }
            else
            {
                videoPlayer.Play();
            }
        }
    }
}
