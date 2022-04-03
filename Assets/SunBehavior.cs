using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunBehavior : MonoBehaviour
{
    public GameObject trashPrefab;
    public int trashToGenerate;
    public float explosionMagnitude;

    [SerializeField] private bool sunDying;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (!collider.gameObject.CompareTag("Explosion")) return;
        if (sunDying) return;
        sunDying = true;

        gameObject.LeanScale(new Vector3(2f, 2f, 2f), 2f).setEaseOutQuart().setOnComplete(() =>
        {
            gameObject.LeanScale(new Vector3(0f, 0f, 0f), 0.6f).setEaseInExpo().setOnComplete(() =>
            {
                var planet = FindObjectOfType<PlayerController>();

                for (int i = 0; i < trashToGenerate; i++)
                {
                    var trash = Instantiate(trashPrefab, transform.position, Quaternion.identity);
                    trash.GetComponent<SpriteRenderer>().color = spriteRenderer.color;
                    var trailRenderer = trash.GetComponent<TrailRenderer>();
                    trailRenderer.startColor = spriteRenderer.color;
                    trailRenderer.endColor = spriteRenderer.color;

                    // Give them all a push towards the planet, with some fudge.
                    var normal = (trash.transform.position - planet.transform.position).normalized;
                    normal += new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(-0.5f, 0.5f), 0f);

                    trash.GetComponent<Rigidbody2D>().AddForce(-normal * explosionMagnitude);
                }

                GameManager.Instance.sunKilled = true;
                Destroy(gameObject);
            });
        });
    }
}
