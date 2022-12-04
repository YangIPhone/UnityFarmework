using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuditDemo : MonoBehaviour
{
    //音频源
    public AudioClip music;
    public AudioClip se;

    //播放器,AudioSource同时只能播放一个背景音乐，但可以播放无数个音效(SE)
    private AudioSource audioPlayer;
    // Start is called before the first frame update
    void Start()
    {
        audioPlayer = GetComponent<AudioSource>();
        audioPlayer.clip = music;//设置播放的音频片段
        audioPlayer.loop = true;//设置循环播放
        audioPlayer.volume = 0.5f;//设置音量
        audioPlayer.Play();//开始播放
    }

    // Update is called once per frame
    void Update()
    {
        //按空格切换播放/暂停
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //audioPlayer.isPlaying返回播放状态
            //如果正在播放
            if (audioPlayer.isPlaying)
            {
                audioPlayer.Pause();//暂停播放
            }
            else
            {
                audioPlayer.UnPause();//继续播放(非暂停)，Play()方法会从头播放
            }
        }
        //按P键播放一次重击音效
        if (Input.GetKeyDown(KeyCode.P))
        {
            audioPlayer.PlayOneShot(se);//播放指定音频一次
        }
    }
}
