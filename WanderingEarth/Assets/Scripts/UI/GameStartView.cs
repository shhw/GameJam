using System;
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
            RegisterButtonClick(gameObject, OnClick);
        }

        void OnClick(UnityEngine.GameObject obj)
        {
            UIManager.GetInstance().Show("Page_GameHUD");
        }

    }
}
