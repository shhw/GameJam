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
        private GameObject earthObject;
        private EarthEntity earthEntity;

        public override void Init()
        {
            
        }

        public override void Final()
        {
        }

        public EarthEntity GetEarthEntity()
        {
            if (earthEntity == null)
                earthEntity = GameObject.Find("EarthObject").GetComponent<EarthEntity>();
            return earthEntity;
        }
    }
}
