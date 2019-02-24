using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WanderingEarth
{
    /// <summary>
    /// 道具 Entity。
    /// </summary>
    class ItemEntity : BaseEntity
    {
        public float speed = 10;
        void Start()
        {
            GetComponent<Rigidbody2D>().velocity = Vector2.down * speed;
        }
    }
}
