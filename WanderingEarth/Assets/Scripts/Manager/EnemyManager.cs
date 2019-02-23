using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WanderingEarth
{
    /// <summary>
    /// 敌人 Manager。
    /// </summary>
    class EnemyManager : BaseManager<EffectManager>
    {
        public List<GameObject> enemys;
        public override void Init()
        {
            GameObject obj = Resources.Load<GameObject>("Prefabs/enemy/1");
            GameObject enemy = Instantiate(obj);
            enemys.Add(enemy);
        }

        public override void Final()
        {
        }
    }
}
