using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public HealthUi healthUi;
    public SpriteRenderer damageOverlay;

    public float maxHealth;
    [SerializeField] private float health;

    public float damageFlashIntensity;
    public float damageFlashRate;

    public AudioSource audioSource;

    void Awake()
    {
        health = maxHealth;
        healthUi.SetMaxHealth(maxHealth);
    }

    public void AddHealth(float val)
    {
        health += val;
        UpdateUi();
    }

    public void RemoveHealth(float val)
    {
        if (!GameManager.Instance.running) return;

        health -= val;
        UpdateUi();
        FlashDamage();

        audioSource.Play();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Trash"))
        {
            RemoveHealth(1f);
        }
    }

    void Update()
    {
        if (health <= 0)
        {
            GameManager.Instance.EndGame();
        }
    }

    private void UpdateUi()
    {
        healthUi.SetHealth(health);
    }

    private void FlashDamage()
    {
        damageOverlay.gameObject.LeanAlpha(damageFlashIntensity, damageFlashRate).setOnComplete(() =>
        { damageOverlay.gameObject.LeanAlpha(0f, damageFlashRate); });
    }
}
