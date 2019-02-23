using UnityEngine;

namespace WanderingEarth
{
    /// <summary>
    /// 工具类。
    /// </summary>
    public class Utility
    {
        public static float K = 25.0f;
        public static Vector2 GetAttractiveForce(Vector2 distanceVector, float earthMass, float planetMass)
        {
            float m_forceCoefficient = 0;
            Vector2 m_attractiveForce;
            Vector2 forceDirection = distanceVector.normalized;    //引力方向
            float sqrDistance = distanceVector.sqrMagnitude;

            m_forceCoefficient = K * earthMass * planetMass;
            m_attractiveForce = m_forceCoefficient / sqrDistance * forceDirection;
            return m_attractiveForce;
        }
    }
}

