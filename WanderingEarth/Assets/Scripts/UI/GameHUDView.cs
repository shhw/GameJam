using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WanderingEarth
{
    /// <summary>
    /// 游戏场景UI。
    /// </summary>
    class GameHUDView : BaseUI
    {
        protected override void Update()
        {
            var textEditor = gameObject.GetComponentInChildren<UnityEngine.UI.Text>();
            textEditor.text = "光年" + 1;
        }
    }
}
