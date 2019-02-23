using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System;
using System.Threading;

namespace WanderingEarth
{
    /// <summary>
    /// 游戏主循环。
    /// </summary>
    public class GameCore : MonoBehaviour
    {
        void Awake()
        {
            
        }

        void Start()
        {
            // Init Manager
            EventManager.GetInstance().Init();
            UIManager.GetInstance().Init();
            SceneManager.GetInstance().Init();
            PlanetManager.GetInstance().Init();
            ControlManager.GetInstance().Init();
            EnemyManager.GetInstance().Init();
            EffectManager.GetInstance().Init();
            AudioManager.GetInstance().Init();
            ItemManager.GetInstance().Init();

            UIManager.GetInstance().Show("Page_MainMenu");
        }

        void Update()
        {
            // 每帧更新
        }

        private void OnDestroy()
        {
            // 销毁
            EventManager.GetInstance().Final();
            UIManager.GetInstance().Final();
            SceneManager.GetInstance().Final();
            PlanetManager.GetInstance().Final();
            ControlManager.GetInstance().Final();
            EnemyManager.GetInstance().Final();
            EffectManager.GetInstance().Final();
            AudioManager.GetInstance().Final();
            ItemManager.GetInstance().Final();
        }
    }
}
