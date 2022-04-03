using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrbitBehavior : MonoBehaviour
{
    public GameObject target;
    public float radius;
    public float rate;

    private float count;

    void FixedUpdate()
    {
        count += rate * Time.fixedDeltaTime;
        transform.position = target.transform.position + new Vector3(
            radius * Mathf.Cos(count),
            radius * Mathf.Sin(count),
            0f
        );
    }
}
