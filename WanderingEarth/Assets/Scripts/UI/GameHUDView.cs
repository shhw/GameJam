using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WanderingEarth
{
    /// <summary>
    /// 游戏场景UI。
    /// </summary>
    class GameHUDView : BaseUI
    {
        Color orange = new Color(1.0f, 0.27f, 0.0f,1.0f);
        const float limit = 0.1f;
        int times = 0;
        protected override void Update()
        {
            var textEditor = gameObject.GetComponentInChildren<UnityEngine.UI.Text>();
            textEditor.text = "光年:" + (SceneManager.GetInstance().GetTravelDistance()/9.4607e5).ToString("#0.00");

            var slider = gameObject.GetComponentInChildren<UnityEngine.UI.Slider>();
            slider.value = SceneManager.GetInstance().GetEarthEntity().GetCurEnergyRate();

            var fill = GameObject.Find("SliderFill");
            Color color = Color.Lerp(Color.red, Color.green, slider.value);

            var background = GameObject.Find("SliderBackground");
            if (slider.value < limit && times%14<7)
            {
                background.GetComponent<UnityEngine.UI.Image>().color = orange;
            }
            else
            {
                background.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            }
            times += 1;
            fill.GetComponent<UnityEngine.UI.Image>().color=color;
        }
    }
}
