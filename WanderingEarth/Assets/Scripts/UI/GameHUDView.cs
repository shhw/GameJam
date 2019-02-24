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
        Color orange = new Color(1.0f, 0.27f, 0.0f, 1.0f);
        const float limit = 0.1f;
        int times = 0;
        GameObject fill = null;
        GameObject fill2 = null;
        UnityEngine.UI.Slider slider = null;
        UnityEngine.UI.Slider slider2 = null;
        protected override void Update()
        {
            var textEditor = gameObject.GetComponentInChildren<UnityEngine.UI.Text>();
            textEditor.text = "光年:" + (SceneManager.GetInstance().GetTravelDistance() / 9.460e5).ToString("F5");
            if (slider==null)
            {
                slider = GameObject.Find("Slider").GetComponent<UnityEngine.UI.Slider>();
            }
            
            slider.value = SceneManager.GetInstance().GetEarthEntity().GetCurEnergyRate();

            if (fill ==null)
            {
                fill = GameObject.Find("SliderFill");
            }
            
            Color color = Color.Lerp(Color.red, Color.green, slider.value);

            var background = GameObject.Find("SliderBackground");
            if (slider.value < limit && times % 14 < 7)
            {
                background.GetComponent<UnityEngine.UI.Image>().color = orange;
            }
            else
            {
                background.GetComponent<UnityEngine.UI.Image>().color = Color.white;
            }
            times += 1;
            fill.GetComponent<UnityEngine.UI.Image>().color = color;

            if (fill2 ==null)
            {
                fill2 = GameObject.Find("DefenderFill");
            }
            var e =SceneManager.GetInstance().GetEarthEntity().GetSheilCoolDownRate();

            if (slider2==null)
            {
                slider2= GameObject.Find("DefenderSlider").GetComponent<UnityEngine.UI.Slider>();
            }
            slider2.value = e;
            if (e==1.0f)
            {
                fill2.GetComponent<UnityEngine.UI.Image>().color = Color.cyan;
            }
            else
            {
                fill2.GetComponent<UnityEngine.UI.Image>().color = orange;
            }
        }
    }
}
