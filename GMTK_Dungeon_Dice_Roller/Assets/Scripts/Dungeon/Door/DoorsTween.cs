using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorsTween : MonoBehaviour
{
    public float timeShowAnimation;
    public float timeHideAnimation;
    private Vector3 _originalScale;
    private void Awake()
    {
        _originalScale = transform.localScale;
        LeanTween.scale(gameObject, Vector3.zero, 0f);
    }

    public void Lock()
    {
        LeanTween.scale(gameObject, _originalScale, timeShowAnimation).setEaseSpring();
    }

    public void Open()
    {
        LeanTween.scale(gameObject, Vector3.zero, timeHideAnimation).setEaseInSine();
    }
}
