using System;
using System.Timers;
using constant;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using utils;

namespace controller
{
    public class GameController : MonoBehaviour
    {
        [FormerlySerializedAs("_btnController")]
        public BtnController btnController;

        [FormerlySerializedAs("_loadController")]
        public LoadController loadController;

        [FormerlySerializedAs("_audioSource")] public AudioSource audioSource;
        static GameController _instance = null;

        public static GameController Instance
        {
            get { return _instance; }
        }

        private void Awake()
        {
            btnController = GetComponent<BtnController>();
            loadController = GetComponent<LoadController>();
            audioSource = GetComponent<AudioSource>();
            donDestroy();
        }

        private void Start()
        {
            init();
        }

        public void init()
        {
            loadController.loadInit();
            loadController.loadMusicListener();   
        }

        private void donDestroy()
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
    }
}