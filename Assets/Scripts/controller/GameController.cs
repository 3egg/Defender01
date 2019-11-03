using System;
using System.Collections;
using System.IO;
using System.Linq;
using System.Timers;
using constant;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Experimental.PlayerLoop;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;
using UnityEngine.Video;
using utils;

namespace controller
{
    public class GameController : MonoBehaviour
    {
        private static GameController _instance = null;
        private AudioSource _audioSource;
        private GameObject _player;
        private GameObject _touchField;
        private GameObject _musicBtn;
        private Transform _uiParent;
        private Transform _playerParent;
        private Transform _tfParent;
        private Transform _sceneMenu;
        private GameObject _sceneLoadImg;
        private GameObject _imgObj;
        private GameObject _videoObj;
        private MusicPlayer _musicPlayer;

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
            loadSceneBtns();
            loadImageAndVideoObj();
        }

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hitInfo;
                if (Physics.Raycast(ray, out hitInfo))
                {
                    var tran = hitInfo.transform;
                    if (tran.name.Equals("Plane")) return;
                    //strs格式为 Image-First-1 或者 Video-First-1. 通过不同的名称打开不同的对象
                    var strs = tran.name.Split('-');
                    switch (strs[0])
                    {
                        case Constant.Image:
                            clickImage(strs);
                            break;
                        case Constant.Video:
                            clickVideo(strs);
                            break;
                        case Constant.Scene:
                            clickArrowToScene(strs);
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        private void clickArrowToScene(string[] strs)
        {
            loadScene(strs[1]);
        }
        private void clickVideo(string[] strs)
        {
            setClickOption(_videoObj);
            var pause = _videoObj.GetComponent<VideoController>().pause;
            _videoObj.GetComponentsInChildren<Button>().First(t => t.name.Equals("Button")).GetComponent<Image>()
                .sprite = pause;
            var clip = Resources.Load(Constant.Video + "/" + strs[1] + "/" + strs[2]) as VideoClip;
            _videoObj.GetComponentInChildren<VideoPlayer>().clip = clip;
        }


        private void clickImage(string[] strs)
        {
            setClickOption(_imgObj);
            //str[0] image or video str[1] first second  str[2] index
            var text = Resources.Load(Constant.Word + "/" + strs[1] + "/" + "Text" + strs[2]) as TextAsset;
            _imgObj.GetComponentInChildren<Text>().text = text != null ? text.text : "";
            _imgObj.GetComponentInChildren<Image>().sprite =
                Resources.Load<Sprite>(Constant.Image + "/" + strs[1] + "/" + strs[0] + strs[2]);
            _musicPlayer = _imgObj.GetComponentInChildren<MusicPlayer>();
            _musicPlayer.audioClips[0] = Resources.Load(Constant.Music + "/" + strs[1] + "/" + strs[2]) as AudioClip;
        }

        private void setClickOption(GameObject obj)
        {
            if (_audioSource.isPlaying) pauseOrPlay();
            setRaycasTarget(false);
            obj.SetActive(true);
            var closeTran = obj.GetComponentsInChildren<Image>()
                .First(t => t.name.Equals(Constant.Close)).transform;
            closeTran.AddBtnListener(() =>
            {
                obj.SetActive(false);

                if (!_audioSource.isPlaying) pauseOrPlay();

                setRaycasTarget(true);
            });
        }

        private void setRaycasTarget(bool isTouch)
        {
            _touchField.GetComponent<Image>().raycastTarget = isTouch;
            _musicBtn.GetComponent<Image>().raycastTarget = isTouch;
            foreach (var button in _sceneMenu.GetComponentsInChildren<Button>())
            {
                button.interactable = isTouch;
            }
        }

        public void loadImageAndVideoObj()
        {
            if (_imgObj == null)
            {
                _imgObj = LoadUtil.Single.loadAndInstaniate(Constant.Prefab + Constant.ImgObj, _uiParent);
            }

            _imgObj.SetActive(false);
            if (_videoObj == null)
            {
                _videoObj = LoadUtil.Single.loadAndInstaniate(Constant.Prefab + Constant.VideoObj, _uiParent);
            }

            _videoObj.SetActive(false);
        }

        public void loadSceneBtns()
        {
            //Use SceneManager.sceneCount and SceneManager.GetSceneAt(int index) to loop the all scenes instead.
            var sceneCount = SceneManager.sceneCountInBuildSettings;
            var scenes = new string[sceneCount];
            if (_sceneMenu == null)
            {
                _sceneMenu = LoadUtil.Single.loadAndInstaniate(Constant.OnUi + Constant.SceneMenu, _uiParent).transform;
            }

            for (int i = 0; i < scenes.Length; i++)
            {
                scenes[i] = Path.GetFileNameWithoutExtension(SceneUtility.GetScenePathByBuildIndex(i));
                var sceneName = scenes[i];
                var sceneBtn = LoadUtil.Single.loadAndInstaniate(Constant.OnUi + Constant.SceneButton, _sceneMenu);
                sceneBtn.GetComponent<Button>().GetComponentInChildren<Text>().text = sceneName;
                sceneBtn.transform.AddBtnListener(() => { loadScene(sceneName); });
            }
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
            if (_audioSource.isPlaying)
            {
                _audioSource.Pause();
            }
            else
            {
                _audioSource.Play();
            }
        }

        //通过场景名称跳转到不同的场景,scenName是当前场景
        public void loadScene(string sceneName)
        {
            if (_sceneLoadImg == null)
            {
                _sceneLoadImg = LoadUtil.Single.loadAndInstaniate(Constant.OnUi + Constant.SceneLoadImg, _uiParent);
            }

            var image = _sceneLoadImg.GetComponent<Image>();
            var sprite = Resources.Load<Sprite>(Constant.SceneImg + sceneName);
            image.sprite = sprite;
            image.DOKill();
            image.DOColor(new Color(1f, 1f, 1f, 1f), 1);
            var trans = _player.GetComponentInChildren<Camera>().transform;
            trans.DOKill();
            trans.DOLocalMove(Vector3.forward * 5, 1);
            StartCoroutine(loadSceneCoroutine(sceneName));
        }

        private IEnumerator loadSceneCoroutine(string sceneName)
        {
            yield return new WaitForSeconds(1);
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
                    loadSceneBtns();
                    loadImageAndVideoObj();
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