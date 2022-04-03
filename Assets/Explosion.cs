using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Explosion : MonoBehaviour
{
    public float scaleUpTime;
    public float holdDelay;
    public float scaleDownTime;
    public Vector3 finalScale;
    public float forceRadiusModifier;
    public float forceMagnitude;

    private Light2D light;

    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = Vector3.zero;
        LeanTween.scale(gameObject, finalScale, scaleUpTime).setEaseInOutCubic().setOnComplete(OnPeakExplosion);
        LeanTween.scale(gameObject, Vector3.zero, scaleDownTime).setDelay(scaleUpTime + holdDelay).setOnComplete(OnAnimComplete);

        light = GetComponent<Light2D>();
        LeanTween.value(gameObject, 0f, 1f, scaleUpTime).setOnUpdate(UpdateLightIntensity).setEaseInOutCubic();
        LeanTween.value(gameObject, 1f, 0f, scaleDownTime).setOnUpdate(UpdateLightIntensity).setDelay(scaleUpTime + holdDelay);

        StartCoroutine(AudioFade.FadeAudio(GetComponent<AudioSource>(), scaleUpTime + scaleDownTime, 0f));
    }

    void FixedUpdate()
    {
        var colliders = Physics2D.OverlapCircleAll(transform.position, transform.localScale.x * forceRadiusModifier);
        foreach (var collider in colliders)
        {
            if (!collider.gameObject.CompareTag("Trash")) continue;
            var rb = collider.GetComponent<Rigidbody2D>();
            var normal = (transform.position - collider.transform.position).normalized;
            rb.AddForce(-normal * forceMagnitude);
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.DrawSphere(transform.position, transform.localScale.x * forceRadiusModifier);
    }

    void OnPeakExplosion()
    {

    }

    void OnAnimComplete()
    {
        Destroy(gameObject);
    }

    void UpdateLightIntensity(float val)
    {
        light.intensity = val;
    }
}
