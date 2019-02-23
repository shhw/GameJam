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
    [RequireComponent(typeof(Rigidbody2D), typeof(SpriteRenderer), typeof(CircleCollider2D))]
    class EarthEntity : BaseEntity
    {
        public float BaseSpeed = 1.0f;

        public float AccForce = 1.0f;

        [Range(0.0f, 1.5707963f)]
        public float SideDirectionRadius = 0.1f;

        Vector2[] vecSideDirection = new Vector2[2];

        Rigidbody2D thisRb2D;
        float Distance = 0.0f;
        Vector2 LastPos;

        public enum ForceType
        {
            eForward,
            eLeft,
            eRight
        };

        struct AccForceParams
        {
            public bool bAdd;
            public float CurForce;
            public float TargetForce;
        }

        AccForceParams[] ForceState = new AccForceParams[3];

        void Awake()
        {
            thisRb2D = GetComponent<Rigidbody2D>();
            thisRb2D.velocity = Vector2.up * BaseSpeed;
        }

        void Start()
        {
            LastPos = transform.position;
        }

        void Update()
        {
            for (int n = 0; n < 3; n++)
            {
                ForceState[n].CurForce = Mathf.Lerp(ForceState[n].CurForce, ForceState[n].TargetForce, Time.deltaTime);
            }
            if (ForceState[1].bAdd)
            {
                Vector2 sideDir = Utility.Vector2Rotate(transform.up, SideDirectionRadius);
                sideDir.Normalize();
                thisRb2D.AddForce(sideDir * ForceState[1].CurForce);
            }
            else if (ForceState[2].bAdd)
            {
                Vector2 psideDir = Utility.Vector2Rotate(transform.up, -SideDirectionRadius);
                psideDir.Normalize();
                thisRb2D.AddForce(psideDir * ForceState[2].CurForce);
            }
            else if (ForceState[0].bAdd)
            {
                thisRb2D.AddForce(new Vector2(this.transform.up.x, this.transform.up.y) * ForceState[0].CurForce);
            }
            else
            {
                Vector2 v = thisRb2D.velocity;
                v.Normalize();
                thisRb2D.velocity = Vector2.Lerp(thisRb2D.velocity, v * BaseSpeed, Time.deltaTime);
            }

            Vector2 curPos = transform.position;
            Distance += (curPos - LastPos).magnitude;
            //获取引力
            Vector2 force = PlanetManager.GetInstance().GetPlanetsForce(curPos, thisRb2D.mass);
            thisRb2D.AddForce(force);
        }

        public Vector2 GetPosition()
        {
            return new Vector2(transform.position.x, transform.position.y);
        }

        public void OnAddLeftForce()
        {
            ForceState[1].bAdd = true;
            ForceState[1].TargetForce = AccForce;
        }
        public void OnCancelLeftForce()
        {
            ForceState[1].bAdd = false;
            ForceState[1].TargetForce = 0;
        }

        public void OnAddRightForce()
        {
            ForceState[2].bAdd = true;
            ForceState[2].TargetForce = AccForce;
        }
        public void OnCancelRightForce()
        {
            ForceState[2].bAdd = false;
            ForceState[2].TargetForce = 0;
        }

        public void OnAddForwardForce()
        {
            ForceState[0].bAdd = true;
            ForceState[0].TargetForce = AccForce;
        }
        public void OnCancelForwardForce()
        {
            ForceState[0].bAdd = false;
            ForceState[0].TargetForce = 0;
        }

        public float GetTravelDistance()
        {
            return Distance;
        }

        void OnDrawGizmos()
        {
            Vector2 sideDir = Utility.Vector2Rotate(transform.up, SideDirectionRadius);
            sideDir.Normalize();
            Vector2 psideDir = Utility.Vector2Rotate(transform.up, -SideDirectionRadius);
            psideDir.Normalize();

            Debug.DrawLine(transform.position, transform.position + new Vector3(psideDir.x, psideDir.y, 0));
            Debug.DrawLine(transform.position, transform.position + new Vector3(sideDir.x, sideDir.y, 0));
        }
    }
}
