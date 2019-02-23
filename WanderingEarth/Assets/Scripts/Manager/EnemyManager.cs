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
    class EnemyManager : BaseManager<EnemyManager>
    {
        public List<GameObject> enemys;
        public override void Init()
        {
            enemys = new List<GameObject>();
            GameObject obj = Resources.Load<GameObject>("Prefabs/enemy/1");
            GameObject enemy = Instantiate(obj);
            GameObject enemyNodes = GameObject.Find("EnemyNodes");
            enemy.transform.parent = enemyNodes.transform;
            enemy.transform.position = new Vector3(50, 50, 0);
            enemys.Add(enemy);
        }

        public override void Final()
        {
            foreach (GameObject enemy in enemys)
                GameObject.Destroy(enemy);
        }
    }
}
