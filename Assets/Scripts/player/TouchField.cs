using UnityEngine;
using UnityEngine.EventSystems;

namespace player
{
    public class TouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [HideInInspector] public Vector2 touchDist;
        [HideInInspector] public Vector2 pointerOld;
        [HideInInspector] public int pointerId;
        [HideInInspector] public bool pressed;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            if (pressed)
            {
                if (pointerId >= 0 && pointerId < Input.touches.Length)
                {
                    touchDist = Input.touches[pointerId].position - pointerOld;
                    pointerOld = Input.touches[pointerId].position;
                }
                else
                {
                    touchDist = new Vector2(Input.mousePosition.x, Input.mousePosition.y) - pointerOld;
                    pointerOld = Input.mousePosition;
                }
            }
            else
            {
                touchDist = new Vector2();
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            pressed = true;
            pointerId = eventData.pointerId;
            pointerOld = eventData.position;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            pressed = false;
        }
    }
}