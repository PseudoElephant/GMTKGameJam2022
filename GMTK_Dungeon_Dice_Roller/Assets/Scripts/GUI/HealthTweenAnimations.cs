using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthTweenAnimations : MonoBehaviour
{
    public float timeShowAnimation;
    public float timeHideAnimation;
    private Vector3 _originalScale;
    private void Awake()
    {
        _originalScale = GetComponent<RectTransform>().localScale;
        LeanTween.scale(gameObject, Vector3.zero, 0f);
    }

    public void Show()
    {
        LeanTween.scale(gameObject, _originalScale, timeShowAnimation).setEaseSpring();
    }

    public void Hide()
    {
        LeanTween.scale(gameObject, Vector3.zero, timeHideAnimation).setEaseInSine()
            .setOnComplete(() => Destroy(gameObject));
    }
}
