using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace WanderingEarth
{
    /// <summary>
    /// UI父类。
    /// </summary>
    class BaseUI : MonoBehaviour
    {

        private bool visible
        {
            get { return gameObject.activeSelf; }
            set { gameObject.SetActive(value); }
        }

        Dictionary<string, List<GameObject>> mItemDic = new Dictionary<string, List<GameObject>>();
        protected object mInitParam;

        protected virtual void OnEnable()
        {

        }

        protected virtual void Awake()
        {
            mItemDic.Clear();
            AddItem(gameObject);
        }

        public virtual void Init(object param)
        {
            mInitParam = param;
        }

        protected virtual void Start()
        {

        }

        protected virtual void Update()
        {
        }

        protected virtual void LateUpdate()
        {

        }

        protected virtual void OnDisable()
        {
            
        }

        protected virtual void OnDestroy()
        {
            Close();
        }

        public virtual void Close()
        {
            if (!visible)
                return;
            visible = false;
        }

        internal virtual void _OnClose()
        {
            
        }

        protected _Ty GetComponent<_Ty>(GameObject obj) where _Ty : Component
        {
            _Ty ty = obj.GetComponent<_Ty>();
            if (ty == null)
            {
                Debug.LogError("Can not Find Component!!");
                return ty;
            }
            return ty;
        }

        public static T AddComponentIfNotExist<T>(GameObject go) where T : Component
        {
            if (go == null)
            {
                Debug.LogError("AddComponentIfNotExist param 'go' is null.");
                return null;
            }
            T t = go.GetComponent<T>();
            if (t == null)
            {
                t = go.AddComponent<T>();
            }
            return t;
        }

        public static T GetComponentInParents<T>(GameObject go) where T : Component
        {
            if (go == null)
                return null;

            Transform trans = go.transform.parent;
            T t = null;
            while (trans != null)
            {
                t = trans.GetComponent<T>();
                if (t != null)
                {
                    break;
                }
                else
                {
                    trans = trans.parent;
                }
            }
            return t;
        }

        public static T CreateObject<T>(GameObject templete, GameObject parent) where T : Component
        {
            GameObject go = GameObject.Instantiate(templete);
            go.SetActive(true);
            go.transform.SetParent(parent.transform);
            go.transform.localScale = Vector3.one;
            go.transform.localPosition = Vector3.zero;
            return AddComponentIfNotExist<T>(go);
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

        public static void SetItemText(GameObject obj, string text)
        {
            if (obj == null)
            {
                Debug.LogError("SetItemText param obj is NULL!!!");
                return;
            }

            Text ui_text = obj.GetComponent<Text>();
            if (ui_text != null)
            {
                ui_text.text = text;
            }
            else
            {
                Debug.LogError(obj.name + " is not text!!!");
            }
        }

        public static string GetItemText(GameObject obj)
        {
            if (obj == null)
            {
                Debug.LogError("@@@GetItemText err: obj == null");
                return string.Empty;
            }
            Text textCom = obj.GetComponent<Text>();
            if (textCom == null)
            {
                Debug.LogError("@@@GetItemText err: textCom == null");
                return string.Empty;
            }
            return textCom.text;
        }

        protected void AddItem(GameObject go)
        {
            if (go == null || go.transform == null)
            {
                return;
            }
            {
                List<GameObject> list = GetAllItem(go.name, true);
                if (list != null)
                {
                    list.Add(go);
                }
            }

            Transform[] transforms = go.GetComponentsInChildren<Transform>(true);
            if (transforms != null)
            {
                foreach (Transform transform in transforms)
                {
                    if (transform != null && transform.gameObject != null)
                    {
                        List<GameObject> list = GetAllItem(transform.gameObject.name, true);
                        if (list != null)
                        {
                            list.Add(transform.gameObject);
                        }
                    }
                }
            }
        }

        protected List<GameObject> GetAllItem(string name, bool autoCreate = false)
        {
            if (name == null || name.Length == 0)
            {
                return null;
            }
            List<GameObject> list = null;
            mItemDic.TryGetValue(name, out list);
            if (list == null && autoCreate)
            {
                list = new List<GameObject>();
                mItemDic[name] = list;
            }
            if ((list == null || list.Count == 0) && !autoCreate)
            {
                Debug.LogError(gameObject.name + ": Missing " + name);
            }
            return list;
        }

        protected GameObject GetItem(GameObject obj, string name)
        {
            if (obj == null || string.IsNullOrEmpty(name))
            {
                Debug.LogError("Failed to get " + name);
                return null;
            }
            Transform[] trans = obj.GetComponentsInChildren<Transform>(true);
            if (trans == null)
            {
                Debug.LogError("Find nothing in the child!!!");
                return null;
            }
            int len = trans.Length;
            for (int i = 0; i < len; ++i)
            {
                if (trans[i].gameObject.name == name)
                {
                    return trans[i].gameObject;
                }
            }
            return null;
        }

        protected GameObject GetItem(string itemName)
        {
            List<GameObject> list = GetAllItem(itemName);
            if (list != null && list.Count > 0)
            {
                if (list.Count > 1)
                {
                    Debug.LogWarning(string.Format("@ UIDialog:GetItem error, finds more than one nodes by name: {0} in {1}", itemName, this.name));
                }
                return list[0];
            }
            return null;
        }

        protected T GetComponentByType<T>(string gameObjectName) where T : Component
        {
            GameObject go = GetItem(gameObjectName);
            if (go == null)
                return null;

            T comp = go.GetComponent<T>();
            if (comp == null)
            {
                string proto = "Error! Can not find component {0} at {1}";
                Debug.LogError(string.Format(proto, typeof(T).Name, gameObjectName));
            }
            return comp;
        }

        protected bool HasItem(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                Debug.LogError("Note!: UIDialog.HasItem name == null");
                return false;
            }

            List<GameObject> list = null;
            mItemDic.TryGetValue(name, out list);
            if (list != null && list.Count > 0)
            {
                if (list.Count > 1)
                {
                    Debug.LogError(" Note!: UIDialog.GetItem finds more than one nodes by name=" + name);
                }
                return true;
            }
            return false;
        }

        protected void RegisterButtonClick(GameObject obj, Action<GameObject> func)
        {
            Global.RegisterButtonClick(obj, func);
        }
    }
}
