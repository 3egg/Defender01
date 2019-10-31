using System;
using constant;
using UnityEngine;
using UnityEngine.SceneManagement;
using utils;

namespace controller
{
    public class LoadController : MonoBehaviour
    {
        private Transform _musicBtn;
        private BtnController _btnController;

        private void Start()
        {
            _btnController = GameController.Instance.btnController;
        }

        public void loadInit()
        {
            var player = LoadUtil.Single.loadAndInstaniate(Constant.OnUi + Constant.Player,
                GameObject.Find(Constant.Player).transform);
            player.transform.position = Vector3.zero;
            LoadUtil.Single.loadAndInstaniate(Constant.OnUi + Constant.TouchField,
                GameObject.Find(Constant.TouchField).transform);
            LoadUtil.Single.loadAndInstaniate(Constant.OnUi + Constant.MusicButton,
                GameObject.Find(Constant.Ui).transform);
        }

        public void loadMusicListener()
        {
            _musicBtn = GameObject.Find(Constant.MusicButton + Constant.Clone).transform;
            _musicBtn.AddBtnListener(pauseOrPlay);
        }

        private void pauseOrPlay()
        {
            var audioSource = GameController.Instance.audioSource;
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
            SceneManager.LoadScene(sceneName);
        }
    }
}