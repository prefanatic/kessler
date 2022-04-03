using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public GameObject launchPad;
    public Image cooldownIndicator;
    public float launchPadRadialDistance;
    public float launchCooldown;

    public GameObject rocketPrefab;

    [SerializeField] private GameObject rocket;
    [SerializeField] private bool shouldFire;
    [SerializeField] private float angle;

    [SerializeField] private float lastLaunchTime;

    public AudioSource audioSource;

    void Update()
    {
        if (!GameManager.Instance.running) return;

        var mousePos = Input.mousePosition;
        mousePos.z = Camera.main.nearClipPlane;
        var worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        var dist = (worldPos - transform.position);
        angle = Mathf.Atan2(dist.y, dist.x);

        launchPad.transform.position = transform.position + new Vector3(
                  launchPadRadialDistance * Mathf.Cos(angle),
                  launchPadRadialDistance * Mathf.Sin(angle),
                  0f
              );

        cooldownIndicator.transform.position = Input.mousePosition;

        shouldFire = Input.GetButton("Fire1");
        if (shouldFire)
        {
            if (rocket) return;
            var delta = Time.time - lastLaunchTime;
            if (delta < launchCooldown) return;
            LaunchRocket();
        }
    }

    void UpdateCooldownIndicator(float value)
    {
        cooldownIndicator.fillAmount = value;
    }

    void HideCooldownIndicator()
    {
        LeanTween.value(gameObject, 1f, 0f, 0.3f).setOnUpdate((float val) => cooldownIndicator.color = new Color(1f, 1f, 1f, val));
    }

    void LaunchRocket()
    {
        audioSource.Play();
        StartCoroutine(AudioFade.FadeAudio(audioSource, 1f, 0f));

        GameManager.Instance.bombRocketsLaunched += 1;
        lastLaunchTime = Time.time;

        rocket = Instantiate(rocketPrefab, launchPad.transform.position, Quaternion.identity);
        var rb = rocket.GetComponent<Rigidbody2D>();
        rb.SetRotation((Mathf.Rad2Deg * angle) - 90f);

        cooldownIndicator.color = new Color(1f, 1f, 1f, 1f);
    }
}
