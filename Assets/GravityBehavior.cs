using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GravityBehavior : MonoBehaviour
{
    public float attractorMass;
    public float attractorAtmosSize;
    public float dragFactor;

    private GameObject target;
    private Rigidbody2D rigidbody;

    void Start()
    {
        target = FindObjectOfType<PlayerController>().gameObject;
        rigidbody = GetComponent<Rigidbody2D>();
    }

    public void OnDrawGizmos()
    {
        var normal = (target.transform.position - transform.position).normalized;
        var tangent = new Vector3(-normal.y, normal.x, 0f);
        Gizmos.DrawLine(transform.position, transform.position + tangent);
    }

    void FixedUpdate()
    {
        Vector2 dist = target.transform.position - transform.position;
        if (dist == Vector2.zero) return;
        float sqrDst = Mathf.Max(dist.sqrMagnitude, 0f);
        Vector2 forceDir = dist.normalized;
        Vector2 force = forceDir * attractorMass / sqrDst;

        rigidbody.velocity += force * Time.fixedDeltaTime;

        // Some form of atmospheric drag.
        if (sqrDst < attractorAtmosSize)
        {
            var normal = (target.transform.position - transform.position).normalized;
            var tangent = new Vector3(-normal.y, normal.x, 0f);
            rigidbody.AddForce(tangent * dragFactor);
        }
    }
}
