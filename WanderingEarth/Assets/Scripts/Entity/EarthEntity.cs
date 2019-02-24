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
        public float MaxEnergy = 100.0f;
        public float DeltaEnergy = 1.0f;
        float CurEnergy;

        public int SheilCount = 1;
        public float SheilTime = 1.0f;
        float CurSheilTime = 0.0f;
        bool bWithSheil = false;

        public float FireScale = 1.5f;

        public float BaseSpeed = 1.0f;
        public float MaxSpeed = 10.0f;
        public float AccForce = 1.0f;

        [Range(0.0f, 1.5707963f)]
        public float SideDirectionRadius = 0.1f;

        Vector2[] vecSideDirection = new Vector2[2];

        Rigidbody2D thisRb2D;
        float Distance = 0.0f;
        Vector2 OrgPos;

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

            public Transform FireObj;
        }

        AccForceParams[] ForceState = new AccForceParams[3];

        void Awake()
        {
            thisRb2D = GetComponent<Rigidbody2D>();
            thisRb2D.velocity = Vector2.up * BaseSpeed;
        }

        void Start()
        {
            PlanetManager.GetInstance().ShowPlanet(GetPosition() + new Vector2(100, 300));

            Reset();
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                OnApplySheil();
            }

            for (int n = 0; n < 3; n++)
            {
                if (ForceState[n].bAdd)
                {
                    CurEnergy = Mathf.Max(0, CurEnergy - DeltaEnergy);
                    ForceState[n].FireObj.localScale = Vector3.Lerp(ForceState[n].FireObj.localScale,
                        new Vector3(FireScale, FireScale, FireScale), Time.deltaTime);

                    if (CurEnergy <= 0)
                    {
                        ForceState[n].TargetForce = 0;
                        ForceState[n].FireObj.localScale = Vector3.Lerp(ForceState[n].FireObj.localScale,
                            Vector3.one, Time.deltaTime);
                    }
                }
                ForceState[n].FireObj.localScale = Vector3.Lerp(ForceState[n].FireObj.localScale,
                  ForceState[n].TargetForce > 0 ?
                    new Vector3(FireScale, FireScale, FireScale) :
                    Vector3.one, Time.deltaTime);
                ForceState[n].CurForce = Mathf.Lerp(ForceState[n].CurForce, ForceState[n].TargetForce, Time.deltaTime);
                if (ForceState[n].TargetForce <= 0)
                {
                    Vector2 v = thisRb2D.velocity;
                    v.Normalize();
                    thisRb2D.velocity = Vector2.Lerp(thisRb2D.velocity, v * BaseSpeed, Time.deltaTime);
                }
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
                CurEnergy = Mathf.Lerp(CurEnergy, MaxEnergy, Time.deltaTime * DeltaEnergy);
            }

            //获取引力
            if (bWithSheil)
            {
                CurSheilTime -= Time.deltaTime;
                if (CurSheilTime <= 0)
                {
                    CurSheilTime = 0;
                    bWithSheil = false;
                }
            }
            else
            {
                Vector2 force = PlanetManager.GetInstance().GetPlanetsForce(transform.position, thisRb2D.mass);
                thisRb2D.AddForce(force);
                //force.Normalize();
                //Debug.DrawLine(transform.position, transform.position + new Vector3(force.x, force.y));
            }


            Vector2 vec = thisRb2D.velocity;
            vec.Normalize();
            vec *= MaxSpeed;
            if (thisRb2D.velocity.magnitude > MaxSpeed)
                thisRb2D.velocity = vec;
        }

        public Vector2 GetPosition()
        {
            return new Vector2(transform.position.x, transform.position.y);
        }

        public void OnAddLeftForce()
        {
            if (!SceneManager.GetInstance().GetGameFlag())
            {
                return;
            }

            ForceState[1].bAdd = true;
            ForceState[1].TargetForce = AccForce;
            AudioManager.GetInstance().Play("fire");
        }
        public void OnCancelLeftForce()
        {
            ForceState[1].bAdd = false;
            ForceState[1].TargetForce = 0;
            AudioManager.GetInstance().Stop();
        }

        public void OnAddRightForce()
        {
            if (!SceneManager.GetInstance().GetGameFlag())
            {
                return;
            }

            ForceState[2].bAdd = true;
            ForceState[2].TargetForce = AccForce;
            AudioManager.GetInstance().Play("fire");
        }
        public void OnCancelRightForce()
        {
            ForceState[2].bAdd = false;
            ForceState[2].TargetForce = 0;
            AudioManager.GetInstance().Stop();
        }

        public void OnAddForwardForce()
        {
            if (!SceneManager.GetInstance().GetGameFlag())
            {
                return;
            }
             
            ForceState[0].bAdd = true;
            ForceState[0].TargetForce = AccForce;
            AudioManager.GetInstance().Play("fire");
        }
        public void OnCancelForwardForce()
        {
            ForceState[0].bAdd = false;
            ForceState[0].TargetForce = 0;
            AudioManager.GetInstance().Stop();
        }

        public float GetTravelDistance()
        {
            Vector2 curPos = transform.position;
            Distance = (curPos - OrgPos).magnitude;
            return Distance;
        }

        public void OnApplySheil()
        {
            if (SheilCount > 0)
            {
                bWithSheil = true;
                CurSheilTime = SheilTime;
                SheilCount--;
            }
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

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "planet")
            {
                gameObject.SetActive(false);
                SceneManager.GetInstance().EndGame();
            }
            else if (other.tag == "sheil")
            {
                SheilCount++;
            }
        }

        public void Reset()
        {
            thisRb2D.velocity = Vector2.zero;
            OrgPos = transform.position;
            CurEnergy = MaxEnergy;
            bWithSheil = false;
            CurSheilTime = 0;
            for (int n = 0; n < 3; n++)
            {
                ForceState[n].bAdd = false;
                ForceState[n].CurForce = 0;
                ForceState[n].TargetForce = 0;
                ForceState[n].FireObj = transform.Find("Fire").Find("F" + n);
                ForceState[n].FireObj.localScale = Vector3.one;
            }
        }
        public Vector2 GetVelocityDir()
        {
            return thisRb2D.velocity;
        }
        public float GetCurEnergyRate()
        {
            return CurEnergy / MaxEnergy;
        }
    }
}
