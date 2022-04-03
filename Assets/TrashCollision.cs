using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashCollision : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager.Instance.planetHits += 1;
            Destroy(gameObject);
            return;
        }

        if (collision.gameObject.CompareTag("Explosion"))
        {
            GameManager.Instance.trashDestroyed += 1;
            Destroy(gameObject);
            return;
        }

    }
}
