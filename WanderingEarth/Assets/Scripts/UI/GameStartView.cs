﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WanderingEarth
{
    /// <summary>
    /// 游戏开始UI。
    /// </summary>
    class GameStartView : BaseUI
    {
        
        protected override void Start()
        {
            Init(gameObject);
            RegisterButtonClick(UnityEngine.GameObject.Find("BtnStart"), OnClick);
        }

        void OnClick(UnityEngine.GameObject obj)
        {
            SceneManager.GetInstance().InitScene();
            UIManager.GetInstance().Show("Page_GameHUD");
        }

    }
}
