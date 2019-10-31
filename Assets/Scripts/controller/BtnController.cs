using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace controller
{
    public class BtnController : MonoBehaviour
    {
        public void clickToLoadScene(string sceneName)
        {
            GameController.Instance.loadController.loadScene(sceneName);
            Invoke(nameof(initScene), .1f);
        }

        private void initScene()
        {
            GameController.Instance.init();
        }
    }
}