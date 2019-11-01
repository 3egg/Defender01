using System;
using System.Collections;
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
        private AudioSource _audioSource;
        static GameController _instance = null;
        private GameObject _player;
        private GameObject _touchField;
        private GameObject _musicBtn;
        private Transform _uiParent;
        private Transform _playerParent;
        private Transform _tfParent;

        public static GameController Instance
        {
            get { return _instance; }
        }

        private void Awake()
        {
            _audioSource = GetComponent<AudioSource>();
            donDestroy();
        }

        private void Start()
        {
            loadPlayer();
            loadMusic();
        }

        public void loadPlayer()
        {
            if (_player == null)
            {
                _playerParent = GameObject.Find(Constant.Player).transform;
                _player = LoadUtil.Single.loadAndInstaniate(Constant.OnUi + Constant.Player,
                    _playerParent);
            }

            _player.transform.position = Vector3.zero;
            setParent(_player.transform, _playerParent);
            if (_touchField == null)
            {
                _tfParent = GameObject.Find(Constant.TouchField).transform;
                _touchField = LoadUtil.Single.loadAndInstaniate(Constant.OnUi + Constant.TouchField,
                    _tfParent);
            }

            setParent(_touchField.transform, _tfParent);
        }

        public void loadMusic()
        {
            if (_musicBtn == null)
            {
                _uiParent = GameObject.Find(Constant.Ui).transform;
                _musicBtn = LoadUtil.Single.loadAndInstaniate(Constant.OnUi + Constant.MusicButton,
                    _uiParent);
            }

            var musicBtn = _musicBtn.transform;
            setParent(musicBtn, _uiParent);
            musicBtn.AddBtnListener(pauseOrPlay);
        }

        private void setParent(Transform obj, Transform parent)
        {
            obj.SetParent(parent);
        }

        public void pauseOrPlay()
        {
            var audioSource = _instance._audioSource;
            if (audioSource.isPlaying)
            {
                audioSource.Pause();
            }
            else
            {
                audioSource.Play();
            }
        }


        public void loadScene(string sceneName)
        {
            StartCoroutine(startLoading(sceneName));
        }

        private IEnumerator startLoading(string sceneName)
        {
            AsyncOperation op = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
            op.allowSceneActivation = false; //这个变量是手动赋值
            while (!op.isDone)
            {
                yield return new WaitForSeconds(0.01f);
                if (op.progress >= 0.5f) //加载进度大于等于0.9时说明加载完毕
                {
                    op.allowSceneActivation = true; //手动赋值为true（此值为true时，isDone自动会跟着变
                }

                if (op.isDone)
                {
                    loadPlayer();
                    loadMusic();
                    break;
                }
            }

            yield return null;
        } //在此之后说明完全加载完毕，做你想做的事情

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