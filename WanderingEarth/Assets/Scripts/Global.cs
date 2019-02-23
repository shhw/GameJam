using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace WanderingEarth
{
    /// <summary>
    /// 通用类。注册静态通用函数，全局变量。
    /// </summary>
    public class Global
    {
        public static void RegisterButtonClick(GameObject obj, Action<GameObject> func, bool isIgnoreDrag = true)
        {
            if (obj == null)
            {
#if UNITY_EDITOR
                Debug.LogError("RegisterButtonClick : obj is Null");
#endif
                return;
            }
            //EventTriggerClick.Register(obj, func, isIgnoreDrag);
        }

        public static T GetComponent<T>(GameObject obj, string name) where T : UnityEngine.Component
        {
            if (obj == null)
            {
#if UNITY_EDITOR
                Debug.LogError("GetComponent main gameobject == null");
#endif
                return null;
            }

            Transform[] transforms = obj.GetComponentsInChildren<Transform>(true);
            if (transforms != null)
            {
                foreach (Transform item in transforms)
                {
                    if (item != null && item.gameObject != null)
                    {
                        if (item.name.Equals(name))
                        {
                            return item.GetComponent<T>();
                        }
                    }
                }
            }
            return null;
        }
        public static void ShowItem(GameObject item, bool show)
        {
            if (item == null)
            {
                Debug.LogError("Global ShowItem param item is NULL!!!");
                return;
            }
            if (item.activeSelf != show)
            {
                item.SetActive(show);
            }
        }
    }
}
