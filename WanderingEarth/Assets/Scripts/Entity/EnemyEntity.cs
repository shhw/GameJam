using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WanderingEarth
{
    /// <summary>
    /// 敌人 Entity。
    /// </summary>
    class EnemyEntity : BaseEntity
    {
        public float speed = 10;

        void Start()
        {
            GetComponent<Rigidbody2D>().velocity= Vector2.down * speed;
        }
        void Update()
        {
            CheckBarrier();
        }

        void CheckBarrier()
        {
            int radius = 500;
            Collider[] cols = Physics.OverlapSphere(this.transform.position, radius);
            if (cols.Length > 0)
            {
                for (int i = 0; i < cols.Length; i++)
                {
                    Debug.Log("检测到物体" + cols[i].name);
                }
            }
        }
    }
}
