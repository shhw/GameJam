using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float LerpSpeed = 0.5f;//几秒

    Vector3 DeltaPos;

    // Use this for initialization
    void Start()
    {
        Vector2 OrgPos = WanderingEarth.SceneManager.GetInstance().GetEarthEntity().GetPosition();
        DeltaPos = new Vector3(OrgPos.x, OrgPos.y, 0) - transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector2 OrgPos = WanderingEarth.SceneManager.GetInstance().GetEarthEntity().GetPosition();
        Vector3 LerpPos = new Vector3(OrgPos.x, OrgPos.y, 0) - DeltaPos;
        transform.position = Vector3.Lerp(transform.position, LerpPos, Time.deltaTime / LerpSpeed);
    }
}
