using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public float LerpSpeed = 0.5f;//几秒

    Transform earthT;
    Vector3 DeltaPos;

    // Use this for initialization
    void Start()
    {
        earthT = WanderingEarth.SceneManager.GetInstance().GetEarthEntity().transform;
        DeltaPos = earthT.position - transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 LerpPos = earthT.position - DeltaPos;
        transform.position = Vector3.Lerp(transform.position, LerpPos, Time.deltaTime / LerpSpeed);
    }
}
