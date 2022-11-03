using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChangQFramework{
    public class AudioManager : ManagerBase<AudioManager>
    {
        //环境音
        private AudioSource envPlayer;

        //音效
        private AudioSource sePlayer;

        //音乐
        private AudioSource player;

        private void Start()
        {
            //音效播放器
            sePlayer = gameObject.AddComponent<AudioSource>();
            //音乐播放器
            player = gameObject.AddComponent<AudioSource>();
            player.loop = true;
            //环境音播放器
            envPlayer = gameObject.AddComponent<AudioSource>();
            //切换场景不销毁物体
            GameObject.DontDestroyOnLoad(gameObject);
        }

        #region 音乐
        //播放音乐
        //方式一:文件名播放
        public void PlayMusic(string musicName, float volum = 1)
        {
            AudioClip clip = Resources.Load<AudioClip>(musicName);
            PlayMusic(clip, volum);
        }
        //方式二:直接播放AudioClip
        public void PlayMusic(AudioClip clip,float volume = 1)
        {
            if (player.isPlaying)
            {
                player.Stop();
            }
            player.clip = clip;
            player.volume = volume;
            player.Play();
        }

        //停止播放音乐
        public void StopMusic()
        {
            player.Stop();
        }

        //改变音乐音量
        public void ChangeMusicVolume(float volume)
        {
            player.volume = volume;
        }

        #endregion

        #region 环境音
        public void PlayEnvMusic(string envMusicname, float volum = 1)
        {
            AudioClip clip = Resources.Load<AudioClip>(envMusicname);
            PlayEnvMusic(clip, volum);
        }
        public void PlayEnvMusic(AudioClip clip, float volume = 1)
        {
            if (envPlayer.isPlaying)
            {
                envPlayer.Stop();
            }
            envPlayer.clip = clip;
            envPlayer.volume = volume;
            envPlayer.Play();
        }

        //停止播放音乐
        public void StopEnvMusic()
        {
            envPlayer.Stop();
        }

        //改变音乐音量
        public void ChangeEnvMusicVolume(float volume)
        {
            envPlayer.volume = volume;
        }
        #endregion

        #region 音效
        public void PlaySeSound(string seName, float volume)
        {
            AudioClip clip = Resources.Load<AudioClip>(seName);
            PlaySeSound(clip, volume);
        }
        public void PlaySeSound(AudioClip clip,float volume)
        {
            sePlayer.PlayOneShot(clip, volume);
        }

        //在指定物体上播放(3D空间内播放)
        public void PlaySeSoundOnObject(string seName,GameObject gameObject,float volume)
        {
            AudioClip clip = Resources.Load<AudioClip>(seName);
            PlaySeSoundOnObject(clip, gameObject, volume);
        }
        public void PlaySeSoundOnObject(AudioClip clip, GameObject gameObject, float volume)
        {
            AudioSource player = gameObject.GetComponent<AudioSource>();
            if(player == null)
            {
                player = gameObject.AddComponent<AudioSource>();
            }
            player.PlayOneShot(clip, volume);
        }
        #endregion


    }
}
