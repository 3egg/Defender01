using System;
using UnityEngine;

namespace Menu
{
    public class MenuController : MonoBehaviour
    {
        [SerializeField]
        private GameObject mainImage;

        [SerializeField]
        private GameObject secondMenu;

        [SerializeField]
        private GameObject thirdMenu;

        private Transform[] secondMenuChildren;
        
        private void Awake()
        {
            secondMenuChildren = secondMenu.GetComponentsInChildren<Transform>(true);
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        public void clickMainImageToSecondImage()
        {
            mainImage.SetActive(false);
            setActive(secondMenuChildren,true);
        }

        public void clickSecondMenuReturnButton()
        {
            setActive(secondMenuChildren,false);
            mainImage.SetActive(true);
        }


        private void setActive(Transform[] trans,bool isActive)
        {
            foreach (var child in trans)
            {
                child.gameObject.SetActive(isActive);
            }
        }
    }
}
