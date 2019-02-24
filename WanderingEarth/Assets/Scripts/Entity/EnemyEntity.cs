﻿using System;
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

        void Start()
        {
            R2D = GetComponent<Rigidbody2D>();
            R2D.velocity= Vector2.left * speed;
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
                    speed *= -1;
                    R2D.velocity = Vector2.left * speed;
                    break;
                }
            }
        }
    }
}
