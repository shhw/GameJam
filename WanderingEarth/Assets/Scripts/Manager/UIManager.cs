using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WanderingEarth
{
    /// <summary>
    /// UI Manager。
    /// </summary>
    class UIManager : BaseManager<UIManager>
    {
        private GameObject m_Root = null;
        private GameObject m_mainUI = null;

        public override void Init()
        {
            GameObject uiGobj = GameObject.Find("UI");
            if (uiGobj != null)
            {
                m_Root = uiGobj;
            }
        }

        public override void Final()
        {
            if (m_mainUI != null)
            {
                m_mainUI.SetActive(false);
                m_mainUI = null;
            }
        }

        public void Show(string dlgName, object param = null)
        {
            if (m_mainUI != null)
            {
                m_mainUI.SetActive(false);
            }

            m_mainUI = Global.GetItem(m_Root, dlgName);
            m_mainUI.SetActive(true);

            BaseUI uidialog = m_mainUI.GetComponent<BaseUI>();
            uidialog.Init(param);
        }

        public void Close(string dlgName)
        {
            if (m_mainUI != null)
            {
                m_mainUI.SetActive(false);
            }

            m_mainUI = Global.GetItem(m_Root, dlgName);
            m_mainUI.SetActive(false);
        }
    }
}
