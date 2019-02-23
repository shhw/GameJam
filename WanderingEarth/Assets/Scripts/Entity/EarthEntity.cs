using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace WanderingEarth
{
    /// <summary>
    /// 地球 Entity。
    /// </summary>
    class EarthEntity : BaseEntity
    {
        public float InitForce = 1.0f;

        public float SideForce = 1.0f;

        public Vector2 SideDirection;

        Rigidbody2D thisRb2D;


        void Awake()
        {
            thisRb2D = GetComponent<Rigidbody2D>();
            thisRb2D.AddForce(Vector2.up * InitForce);
        }

        void Start()
        {

        }

        void Update()
        {
            if (Input.GetKey(KeyCode.A))
                OnAddSideForce(true);
            else if (Input.GetKey(KeyCode.D))
                OnAddSideForce(false);
        }


        public Vector2 GetEarthPosition()
        {
            return transform.position;
        }

        public float GetEarthMass()
        {
            return thisRb2D.mass;
        }

        public void OnAddAttractionForce(Vector2 force)
        {
            thisRb2D.AddForce(force);
        }

        public void OnAddSideForce(bool bLeft)
        {
            Vector2 sideDir = SideDirection - Vector2.right;
            sideDir = new Vector2(this.transform.right.x, this.transform.right.y) + sideDir;
            if (bLeft)
            {
                thisRb2D.AddForce(sideDir * new Vector2(-1, 1) * SideForce);
            }
            else
            {
                thisRb2D.AddForce(sideDir * 1 * SideForce);
            }
        }

    }
}
