//#define DEBUG_EVENT_CLICK
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace WanderingEarth
{
    /// <summary>
    /// 事件触发。
    /// </summary>
    public class EventTriggerClick : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IMoveHandler
    {
        [SerializeField]
        private float m_requireInterval = 0.3f;     // 点击事件响应要求间隔
        private float m_lastAccessTime = -10000f;   // 最后一次响应点击时刻
        private bool m_interactable = true;         // 是否激活可以响应点击
        private Button m_btn = null;                // 按钮控件（非必须）

        public bool IgnoreInteractable = false;     // Interactabel也要求响应事件

        public bool isIgnoreDrag = false;       //屏蔽按下后移动一段距离的情况
        private float mDeltaMagnitudeThreshold = 5f;

        public Action<GameObject> onClick;

        public Action onPlayClickSound;

        private PointerEventData eventData = null;
        public PointerEventData GetPointerEventData()
        {
            return eventData;
        }

        private void Awake()
        {
            m_btn = GetComponent<Button>();

            int width = Screen.width;
            int height = Screen.height;

            float offsetRadio = 1f / 200f;
            Vector2 deltaSize = new Vector2(width * offsetRadio, height * offsetRadio);
            mDeltaMagnitudeThreshold = deltaSize.magnitude;
        }

        public bool interactable
        {
            get
            {
                if (m_btn != null)
                    return m_btn.interactable;
                else
                    return m_interactable;
            }
            set
            {
                if (m_btn != null)
                    m_btn.interactable = value;
                else
                    m_interactable = value;
            }
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (onClick == null)
            {
#if UNITY_EDITOR
                Debug.LogFormat("button '{0}' EventTriggerClick.onClick is null", gameObject.name);
#endif
                return;
            }


            if (!interactable)
            {
                if (!IgnoreInteractable)
                {
                    Debug.LogWarning("current button interactable is false.");
                    return;
                }
            }

            if (Time.realtimeSinceStartup - m_lastAccessTime < m_requireInterval)
            {
#if UNITY_EDITOR
                Debug.Log(string.Format("Ignore click : require = {0}, interval = {1}, current = {2}, last access = {3}",
                    m_requireInterval,
                    Time.realtimeSinceStartup - m_lastAccessTime,
                    Time.realtimeSinceStartup,
                    m_lastAccessTime));
#endif
                return;
            }

            Vector2 delta = eventData.pressPosition - eventData.position;
            if (delta.magnitude > mDeltaMagnitudeThreshold && isIgnoreDrag)
            {//拖拽
                return;
            }

            if (onPlayClickSound != null)
                onPlayClickSound();
            this.eventData = eventData;
            m_lastAccessTime = Time.realtimeSinceStartup;

            onClick(gameObject);
        }

        void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
        {

        }

        public virtual void OnMove(AxisEventData eventData)
        {

        }

        #region static functions
        // 可以在 Prefab 上挂脚本指定时间
        public static EventTriggerClick Register(GameObject obj, System.Action<GameObject> callback, bool isIgnoreDrag = true)
        {
            if (obj == null)
            {
                Debug.LogError("GetEventTrigger paramer is null!!!");
                return null;
            }

            EventTriggerClick listen = Global.AddComponentIfNotExist<EventTriggerClick>(obj);
            if (listen != null)
            {
                listen.onClick = callback;
                listen.isIgnoreDrag = isIgnoreDrag;
            }
            return listen;
        }

        // 程序指定响应间隔
        public static EventTriggerClick Register(GameObject obj, System.Action<GameObject> callback, float clickInterval, bool isIgnoreDrag = true)
        {
            EventTriggerClick listen = Register(obj, callback, isIgnoreDrag);
            if (listen != null)
            {
                listen.m_requireInterval = clickInterval;
            }
            return listen;
        }

        // 获取点击响应组件（没有会自动创建）
        public static EventTriggerClick GetEventTrigger(GameObject obj, bool isToCreate = true)
        {
            if (obj == null)
            {
                Debug.LogError("GetEventTrigger paramer is null!!!");
                return null;
            }
            EventTriggerClick listen = obj.GetComponent<EventTriggerClick>();
            if (listen == null && isToCreate)
            {
                object objs = new object();
                lock (objs)
                {
                    EventTriggerClick retlisten = obj.AddComponent<EventTriggerClick>();
                    return retlisten;
                }
            }
            return listen;
        }

        public static void UnRegisterButtonClick(GameObject gobj, bool destroyImmediately)
        {
            if (gobj == null)
            {
                Debug.LogWarningFormat("EventTriggerClick.UnRegisterButtonClick param gobj is null.");
                return;
            }

            EventTriggerClick eventclick = gobj.GetComponent<EventTriggerClick>();
            if (eventclick != null)
            {
                eventclick.onClick = null;
                if (destroyImmediately)
                {
                    GameObject.DestroyImmediate(eventclick);
                }
                else
                {
                    GameObject.Destroy(eventclick);
                }
            }
        }
        #endregion
    }
}

