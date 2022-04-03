using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashRocket : MonoBehaviour
{
    public GameObject trashPrefab;
    public float minTrashCreate;
    public float maxTrashCreate;

    public float minTrashForce;
    public float maxTrashForce;

    public float force;
    public float minAliveTime;
    public float maxAliveTime;

    private Rigidbody2D rigidbody;
    private float destroyTime;
    private float startTime;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        destroyTime = Random.Range(minAliveTime, maxAliveTime);
        startTime = Time.time;
    }

    void Update()
    {
        var delta = Time.time - startTime;
        if (delta > destroyTime) Explode();
    }

    void Explode()
    {
        var count = Random.Range(minTrashCreate, maxTrashCreate);
        for (int i = 0; i < count; i++)
        {
            var trash = Instantiate(trashPrefab, transform.position, Quaternion.identity);
            var angle = Random.value * 360f;

            var trashForce = Random.Range(minTrashForce, maxTrashForce);
            var rb = trash.GetComponent<Rigidbody2D>();
            rb.SetRotation(angle);
            rb.AddRelativeForce(Vector2.up * trashForce);


            // Fudge factor to coax trash into orbit
            var distance = rb.position.sqrMagnitude;
            var radial = Mathf.Atan2(rb.position.y, rb.position.x);
            float magnitudeOfVelocity = Mathf.Sqrt(3 / distance);
            float vx = -1 * magnitudeOfVelocity * Mathf.Sin(radial);
            float vy = magnitudeOfVelocity * Mathf.Cos(radial);
            rb.velocity = new Vector2(vx, vy);

            // Add an additional fudge.
            var fudge = Random.Range(0f, 0.5f);
            var velFudge = rb.velocity * fudge;
            rb.velocity -= velFudge;
        }

        Destroy(gameObject);
    }

    void FixedUpdate()
    {
        rigidbody.AddRelativeForce(Vector2.up * force);
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Trash"))
        {
            Explode();
        }
    }
}
