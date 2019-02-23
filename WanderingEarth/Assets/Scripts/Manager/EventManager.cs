using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WanderingEarth
{
    /// <summary>
    /// 事件 Manager。
    /// </summary>
    class EventManager : BaseManager<EffectManager>
    {
        public override void Init()
        {
        }

        void Update()
        {
            if (Input.GetKey("w"))
            {
                SceneManager.GetInstance().GetEarthEntity().OnAddForwardForce();
            }

            if (Input.GetKey("a"))
            {
                SceneManager.GetInstance().GetEarthEntity().OnAddLeftForce();
            }

            if (Input.GetKey("d"))
            {
                SceneManager.GetInstance().GetEarthEntity().OnAddRightForce();
            }

            if (Input.GetKeyUp("w"))
            {
                SceneManager.GetInstance().GetEarthEntity().OnCancelForwardForce();
            }

            if (Input.GetKeyUp("a"))
            {
                SceneManager.GetInstance().GetEarthEntity().OnCancelLeftForce();
            }

            if (Input.GetKeyUp("d"))
            {
                SceneManager.GetInstance().GetEarthEntity().OnCancelRightForce();
            }
        }

        public override void Final()
        {
        }
    }
}
