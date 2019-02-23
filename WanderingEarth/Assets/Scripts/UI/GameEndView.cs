using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WanderingEarth
{
    /// <summary>
    /// 游戏结算UI。
    /// </summary>
    class GameEndView : BaseUI
    {
        protected override void Start()
        {
            Init(gameObject);
            RegisterButtonClick(UnityEngine.GameObject.Find("BtnRestart"), OnClick);
        }

        void OnClick(UnityEngine.GameObject obj)
        {
            UIManager.GetInstance().Show("Page_GameHUD");
            SceneManager.GetInstance().InitScene();
        }
    }
}
