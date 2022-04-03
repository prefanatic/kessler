using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameUIResult : MonoBehaviour
{
    public enum AnimationType
    {
        fade, shake,
    }

    public TextMeshProUGUI value;
    public AnimationType animationType = AnimationType.fade;
    public float shakeDuration;
    public float shakeDampening;

    private CanvasGroup group;
    private float shakeStart;
    private Vector3 startPosition;
    private float dampening = 1.0f;

    void OnEnable()
    {
        group = GetComponent<CanvasGroup>();
        startPosition = gameObject.transform.position;

        switch (animationType)
        {
            case AnimationType.fade:
                group.alpha = 0f;
                group.LeanAlpha(1f, 0.3f);
                break;
            case AnimationType.shake:
                shakeStart = Time.time;
                group.alpha = 0f;
                group.LeanAlpha(1f, 0.1f);
                break;
        }

    }

    public void SetValue(int val)
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        if (value != null)
            value.text = val.ToString();
    }

    void Update()
    {
        var delta = Time.time - shakeStart;
        if (delta >= shakeDuration) return;

        var val = 30 * dampening * Mathf.Cos(Time.time * 80);
        gameObject.transform.position = new Vector3(startPosition.x + val, startPosition.y, startPosition.z);

        dampening = Mathf.Lerp(dampening, 0.001f, Time.deltaTime * shakeDampening);
    }
}
