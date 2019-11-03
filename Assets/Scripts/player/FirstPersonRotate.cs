using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

namespace player
{
    public class FirstPersonRotate : MonoBehaviour
    {
        private TouchField toucField;

        private RigidbodyFirstPersonController fps;

        void Start()
        {
            fps = GetComponent<RigidbodyFirstPersonController>();
            toucField = FindObjectOfType<TouchField>();
        }

        // Update is called once per frame
        void Update()
        {
            fps.mouseLook.lookAxis = toucField.touchDist;
        }
    }
}