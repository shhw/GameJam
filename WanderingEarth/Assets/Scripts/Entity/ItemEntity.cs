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
        public float speed = 1;
        private Rigidbody2D R2D;
        public void ResetSpeed()
        {
            if(R2D == null)
                R2D = GetComponent<Rigidbody2D>();
            R2D.velocity = UnityEngine.Random.Range(1f, 3f)*Vector2.down;
            R2D.angularVelocity = UnityEngine.Random.Range(100f, 300f);
        }
    }
}
