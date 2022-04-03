using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthUi : MonoBehaviour
{
    public Slider slider;
    public Image fill;
    public CanvasGroup group;
    public Color standingColor;
    public Color tingeColor;

    void Awake()
    {
        group.alpha = 0f;
    }

    public void SetMaxHealth(float val)
    {
        slider.maxValue = val;
        slider.value = val;
    }

    public void SetHealth(float val)
    {
        if (val > 0 && val != slider.maxValue && !gameObject.activeSelf)
            ShowHealthBar();
        if (val <= 0)
            HideHealthBar();
        LeanTween.value(gameObject, standingColor, tingeColor, 0.1f).setOnUpdate(UpdateFillColor).setOnComplete(() =>
        {
            LeanTween.value(gameObject, tingeColor, standingColor, 0.1f).setOnUpdate(UpdateFillColor);
        });
        LeanTween.value(gameObject, slider.value, val, 0.2f).setOnUpdate((float val) => slider.value = val);

    }

    private void UpdateFillColor(Color color)
    {
        fill.color = color;
    }

    private void ShowHealthBar()
    {
        gameObject.SetActive(true);
        group.LeanAlpha(1f, 0.4f);
    }

    private void HideHealthBar()
    {
        gameObject.SetActive(false);
    }
}
