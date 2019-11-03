using UnityEngine;

namespace controller
{
    public class MusicController : MonoBehaviour
    {
        static MusicController _instance = null;

        public static MusicController Instance
        {
            get { return _instance; }
        }

        void Awake()
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