using System;
using System.Linq;
using constant;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using utils;

namespace Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField] private GameObject mainImage;

        [SerializeField] private GameObject secondMenu;

        [SerializeField] private GameObject thirdMenu;

        private Transform[] secondMenuChildren;
        private GameObject thirdMenuReturnBtn;
        private GameObject thirdMenuVideoBtn;
        private GameObject thirdMenuVideoObj;
        private VideoClip thirdMenuYuAn;
        private VideoClip thirdMenuBaoZhang;
        private GameObject thirdMenuGameInfo;
        private GameObject thirdImgs;

        private void Awake()
        {
            secondMenuChildren = secondMenu.GetComponentsInChildren<Transform>(true);
            thirdMenuReturnBtn = thirdMenu.GetComponentsInChildren<Transform>(true)
                .First(t => t.gameObject.name.Equals(Constant.ReturnButton))
                .gameObject;
            thirdMenuVideoBtn = thirdMenu.GetComponentsInChildren<Transform>(true)
                .First(t => t.gameObject.name.Equals(Constant.VideoButtons))
                .gameObject;
            thirdMenuVideoObj = thirdMenu.GetComponentsInChildren<Transform>(true)
                .First(t => t.gameObject.name.Equals(Constant.VideoObj))
                .gameObject;
            thirdMenuGameInfo = thirdMenu.GetComponentsInChildren<Transform>(true)
                .First(t => t.gameObject.name.Equals(Constant.GameInfo))
                .gameObject;
            thirdImgs = thirdMenu.GetComponentsInChildren<Transform>(true)
                .First(t => t.gameObject.name.Equals(Constant.Images))
                .gameObject;
            thirdMenuYuAn = Resources.Load(Constant.Video + "/Menu/YuAn") as VideoClip;
            thirdMenuBaoZhang = Resources.Load(Constant.Video + "/Menu/BaoZhang") as VideoClip;
        }

        public void clickMainImageToSecondImage()
        {
            mainImage.SetActive(false);
            setActive(secondMenuChildren, true);
        }

        public void clickSecondMenuReturnButton()
        {
            setActive(secondMenuChildren, false);
            mainImage.SetActive(true);
        }

        public void clickVideoButtonToThirdMenuVideoBtns()
        {
            setActive(secondMenuChildren, false);
            thirdMenuVideoBtn.SetActive(true);
            setThirdMenuBtnActive(true);
        }

        public void clickReturnButtonToSecondMenu()
        {
            setActive(secondMenuChildren, true);
            thirdMenuVideoBtn.SetActive(false);
            setThirdMenuBtnActive(false);
            thirdMenuGameInfo.SetActive(false);
            thirdImgs.SetActive(false);
        }

        public void clikThirdMenuVideoToPlay(string videoName)
        {
            thirdMenuReturnBtn.SetActive(false);
            thirdMenuVideoObj.SetActive(true);
            setVideoObjActive(thirdMenuVideoObj);
            var pause = thirdMenuVideoObj.GetComponent<VideoController>().pause;
            thirdMenuVideoObj.GetComponentsInChildren<Button>().First(t => t.name.Equals("Button"))
                .GetComponent<Image>()
                .sprite = pause;

            var clip = videoName.Equals("YuAn") ? thirdMenuYuAn : thirdMenuBaoZhang;

            thirdMenuVideoObj.GetComponentInChildren<VideoPlayer>().clip = clip;
        }

        public void clickThirdMenuGameInfo()
        {
            thirdMenuReturnBtn.SetActive(false);
            thirdMenuGameInfo.SetActive(true);
        }
        public void clickThirdMenuImages()
        {
            setActive(secondMenuChildren, false);
            thirdMenuReturnBtn.SetActive(false);
            thirdImgs.SetActive(true);
        }

        public void startGameLoadGameScene()
        {
            Debug.LogWarning("loading game scene");
        }


        private void setVideoObjActive(GameObject obj)
        {
            var closeTran = obj.GetComponentsInChildren<Image>()
                .First(t => t.name.Equals(Constant.Close)).transform;
            closeTran.AddBtnListener(() =>
            {
                obj.SetActive(false);
                thirdMenuReturnBtn.SetActive(true);
            });
        }

        private void setActive(Transform[] trans, bool isActive)
        {
            foreach (var child in trans)
            {
                child.gameObject.SetActive(isActive);
            }
        }

        private void setThirdMenuBtnActive(bool isActive)
        {
            thirdMenuReturnBtn.SetActive(isActive);
        }
    }
}