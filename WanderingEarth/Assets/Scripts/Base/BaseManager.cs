using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WanderingEarth
{
    /// <summary>
    /// Manager父类。
    /// </summary>
    /// 
    public abstract class BaseManager<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;
        private static readonly object syslock = new object();
        private static bool _isQuit = false;

        public static T GetInstance()
        {
            if (_instance == null && !_isQuit)
            {
                lock (syslock)
                {
                    if (_instance == null && !_isQuit)
                    {
                        GameObject obj = new GameObject();
                        obj.name = typeof(T).ToString();
                        GameObject.DontDestroyOnLoad(obj);
                        _instance = obj.AddComponent<T>();
                    }
                }
            }
            return _instance;
        }

        private void OnApplicationQuit()
        {
            _isQuit = true;
        }

        public abstract void Init();

        public abstract void Final();
    }
}
