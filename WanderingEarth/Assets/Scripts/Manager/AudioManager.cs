using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WanderingEarth
{
    /// <summary>
    /// 音频 Manager。
    /// </summary>
    class AudioManager : BaseManager<AudioManager>
    {
        public enum SoundGroupType
        {
            None = -1,
            Sound = 1,        // 背景音
            Music = 0,   // 音乐
        }

        private Dictionary<string, AudioClip> audioClipDic = new Dictionary<string, AudioClip>();
        private List<AudioClip> playingAudio = new List<AudioClip>();

        private AudioSource musicSource;
        private AudioSource soundSource;

        public override void Init()
        {
            musicSource = GameObject.Find("AudioMusic").GetComponent<AudioSource>();
            soundSource = GameObject.Find("AudioSound").GetComponent<AudioSource>();

            audioClipDic.Add("bg", Resources.Load<AudioClip>("Audio/bg"));
            audioClipDic.Add("boom", Resources.Load<AudioClip>("Audio/boom"));
            audioClipDic.Add("engine", Resources.Load<AudioClip>("Audio/engine"));
            audioClipDic.Add("fire", Resources.Load<AudioClip>("Audio/fire"));
            audioClipDic.Add("getProtect", Resources.Load<AudioClip>("Audio/getProtect"));
            audioClipDic.Add("protect", Resources.Load<AudioClip>("Audio/protect"));
            audioClipDic.Add("protect1", Resources.Load<AudioClip>("Audio/protect1"));
            audioClipDic.Add("protect2", Resources.Load<AudioClip>("Audio/protect2"));
            audioClipDic.Add("protect3", Resources.Load<AudioClip>("Audio/protect3"));
            audioClipDic.Add("speed", Resources.Load<AudioClip>("Audio/speed"));
        }

        private void Update()
        {
            if(!soundSource.isPlaying)
            {
                playingAudio.Clear();
            }
        }

        public void Play(string name)
        {
            AudioClip audio;
            audioClipDic.TryGetValue(name, out audio);

            for (int i = 0; i < playingAudio.Count(); ++i)
            {
                if (playingAudio[i] == audio)
                {
                    return;
                }
            }

            soundSource.PlayOneShot(audio);
            playingAudio.Add(audio);
        }

        public void Stop()
        {
            playingAudio.Clear();

            soundSource.Stop();
        }

        public override void Final()
        {
        }
    }
}
