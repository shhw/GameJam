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
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer))]
    class EarthEntity : BaseEntity
    {
        public float BaseSpeed = 1.0f;

        public float AccForce = 1.0f;

        public Vector2 SideDirection;

        float CurForce = 0.0f;
        Rigidbody2D thisRb2D;

        public enum ForceType
        {
            eForward,
            eLeft,
            eRight
        };

        struct AccForceParams
        {
            public float CurForce;
            public float TargetForce;
        }

        AccForceParams[] ForceState = new AccForceParams[3];

        void Awake()
        {
            SideDirection.Normalize();
            thisRb2D = GetComponent<Rigidbody2D>();
            thisRb2D.velocity = Vector2.up * BaseSpeed;
        }

        void Start()
        {

        }

        void Update()
        {
            {
                Vector2 v = thisRb2D.velocity;
                v.Normalize();
                CurForce = Mathf.Lerp(CurForce, 0, Time.deltaTime);
                thisRb2D.velocity = Vector2.Lerp(thisRb2D.velocity, v * BaseSpeed, Time.deltaTime);
            }

            //获取引力
            //PlanetManager.GetInstance().
            // thisRb2D.AddForce(force)
        }

        public Vector2 GetPosition()
        {
            return new Vector2(transform.position.x, transform.position.y);
        }

        void OnAddSideForce(ForceType eType)
        {
            Vector2 sideDir = SideDirection - Vector2.right;
            sideDir = new Vector2(this.transform.right.x, this.transform.right.y) + sideDir;
            if (eType == ForceType.eLeft)
            {
                thisRb2D.AddForce(sideDir * new Vector2(-1, 1) * CurForce);
            }
            else if (eType == ForceType.eRight)
            {
                thisRb2D.AddForce(sideDir * 1 * CurForce);
            }
            else if (eType == ForceType.eForward)
            {
                thisRb2D.AddForce(new Vector2(this.transform.forward.x,
                    this.transform.forward.y)
                    * CurForce);
            }
        }


        public void OnAddLeftForce()
        {
            ForceState[1].CurForce = Mathf.Lerp(ForceState[1].CurForce, ForceState[1].TargetForce, Time.deltaTime);

        }
        public void OnCancelLeftForce()
        {

        }

        public void OnAddRightForce()
        {

        }
        public void OnCancelRightForce()
        {

        }

        public void OnAddForwardForce()
        {

        }
        public void OnCancelForwardForce()
        {

        }

    }
}
