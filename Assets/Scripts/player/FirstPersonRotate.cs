using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace player
{
    public class FirstPersonRotate: MonoBehaviour
    {
        public TouchField toucField;

        private RigidbodyFirstPersonController fps;
        void Start()
        {
            fps = GetComponent<RigidbodyFirstPersonController>();
        }

        // Update is called once per frame
        void Update()
        {
            fps.mouseLook.lookAxis = toucField.touchDist;
        }
    }
}