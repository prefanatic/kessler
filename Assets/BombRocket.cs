using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombRocket : MonoBehaviour
{
    public float force;
    public float factor;
    public float trashEmissionRate;

    public GameObject explosionPrefab;
    public GameObject trashPrefab;

    private Rigidbody2D rigidbody;
    private float additionalForce;
    private float lastTrashEmission;


    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();

        // Give the trash emission a little breathing room
        lastTrashEmission = Time.time + trashEmissionRate;
    }

    void Update()
    {
        if (Input.GetButtonUp("Fire1"))
        {
            Explode();
        }
    }

    void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity);

        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        additionalForce += factor * Time.fixedDeltaTime;
        rigidbody.AddRelativeForce(Vector2.up * (force + additionalForce));

        // Give some flexibility in where this rocket can go
        var mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        var worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        var delta = (transform.position - worldPos);
        var angle = Mathf.Atan2(delta.y, delta.x);

        rigidbody.MoveRotation((Mathf.Rad2Deg * angle) + 90f);
        // rigidbody.AddForce(delta);

        var timeDelta = Time.time - lastTrashEmission;
        if (timeDelta >= trashEmissionRate)
        {
            lastTrashEmission = Time.time;
            var trash = Instantiate(trashPrefab, transform.position, Quaternion.identity);
        }
    }
}
