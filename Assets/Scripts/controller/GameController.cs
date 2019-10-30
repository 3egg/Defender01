using System;
using constant;
using UnityEngine;
using utils;

namespace controller
{
    public class GameController : MonoBehaviour
    {
        private AudioSource _audioSource;
        private Transform _musicBtn;

        static GameController _instance = null;

        public static GameController Instance
        {
            get { return _instance; }
        }

        private void Start()
        {
            _audioSource = GetComponent<AudioSource>();
            addMusicListener();
        }

        private void addMusicListener()
        {
            _musicBtn = GameObject.Find(Constant.MusicButton).transform;
            _musicBtn.AddBtnListener(playOrPauseMusic);
        }

        private void playOrPauseMusic()
        {
            if (_audioSource.isPlaying)
            {
                _audioSource.Pause();
            }
            else
            {
                _audioSource.Play();
            }
        }

        private void Awake()
        {
            loadInit();
            donDestory();
        }

        private void donDestory()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                _instance = this;
            }

            DontDestroyOnLoad(this.gameObject); //使对象目标在加载新场景时不被自动销毁
        }

        private static void loadInit()
        {
            var player = LoadUtil.Single.loadAndInstaniate(Constant.Player,
                GameObject.Find(Constant.Player).transform);
            player.transform.position = Vector3.zero;
            var touch = LoadUtil.Single.loadAndInstaniate(Constant.TouchField,
                GameObject.Find(Constant.TouchField).transform);
            //touch.transform.position = Vector3.zero; // local position
        }
    }
}