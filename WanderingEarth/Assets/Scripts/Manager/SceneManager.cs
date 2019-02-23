using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WanderingEarth
{
    /// <summary>
    /// 场景 Manager。
    /// </summary>
    class SceneManager : BaseManager<SceneManager>
    {
        public GameObject earthObject;
        public EarthEntity earthEntity;

        public override void Init()
        {
        }

        public override void Final()
        {
        }

        public EarthEntity GetEarthEntity()
        {
            return earthEntity;
        }
    }
}
