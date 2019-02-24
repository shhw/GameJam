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
        public float speed = 5;
        private Rigidbody2D R2D;

        public void ResetSpeed(float coffi)
        {
            if (R2D == null)
                R2D = GetComponent<Rigidbody2D>();
            R2D.velocity = UnityEngine.Random.Range(1f, 3f) * Vector2.left * coffi;
        }

        void Update()
        {
            CheckBarrier();
        }

        void CheckBarrier()
        {
            float radius = GetComponent<CircleCollider2D>().radius+1.0f;
            Collider2D[] cols = Physics2D.OverlapCircleAll(this.transform.position, radius);
            for(int i=0;i<cols.Length;++i)
            {
                if (cols[i].gameObject != gameObject)
                {
                    R2D.velocity = R2D.velocity * (-1);
                    break;
                }
            }
        }
    }
}
