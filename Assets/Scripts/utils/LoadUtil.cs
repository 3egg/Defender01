using System.IO;
using UnityEngine;
using Utils;

namespace utils
{
    public class LoadUtil : SingletonUtil<LoadUtil>
    {
        public T load<T>(string path, string name) where T : class
        {
            return Resources.Load(path + name) as T;
        }

        public GameObject loadAndInstaniate(string path, Transform parent)
        {
            var temp = Resources.Load<GameObject>(path);

            if (temp == null)
            {
                Debug.LogError("path :" + path + " is null");
                return null;
            }
            else
            {
                var go = Object.Instantiate(temp, parent);
                return go;
            }
        }

        public T[] loadAll<T>(string path) where T : Object
        {
            return Resources.LoadAll<T>(path);
        }

        public T loadJson<T>(string path)
        {
            string json = "";
            if (File.Exists(path))
            {
                json = File.ReadAllText(path);
            }

            return JsonUtility.FromJson<T>(json);
        }
    }
}