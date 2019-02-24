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
        public List<GameObject> enemysPool;

        public override void Init()
        {
            enemys = new List<GameObject>();
            enemysPool = new List<GameObject>();
            GameObject obj = Resources.Load<GameObject>("Prefabs/enemy/1");
            GameObject enemy = Instantiate(obj);
            GameObject enemyNodes = GameObject.Find("EnemyNodes");
            enemy.transform.parent = enemyNodes.transform;
            enemy.SetActive(false);
            enemysPool.Add(enemy);
        }

        public override void Final()
        {
            foreach (GameObject enemy in enemys)
                GameObject.Destroy(enemy);
            foreach (GameObject enemy in enemysPool)
                GameObject.Destroy(enemy);
        }

        public void ShowEnemy(Vector2 pos, float coffi)
        {
            int length = enemysPool.Count();
            if (length < 1)
                return;
            int index = UnityEngine.Random.Range(0, length);
            GameObject enemy = enemysPool[index];
            enemysPool.RemoveAt(index);
            enemy.SetActive(true);
            enemy.transform.position = new Vector3(pos.x, pos.y, 0);
            enemy.GetComponent<EnemyEntity>().ResetSpeed(coffi);
            enemys.Add(enemy);
        }

        public void HideEnemy(GameObject enemy)
        {
            for (int i = 0; i < enemys.Count(); ++i)
            {
                if (enemys[i] == enemy)
                {
                    enemy.SetActive(false);
                    enemys.RemoveAt(i);
                    break;
                }
            }
            enemysPool.Add(enemy);
        }
    }
}
