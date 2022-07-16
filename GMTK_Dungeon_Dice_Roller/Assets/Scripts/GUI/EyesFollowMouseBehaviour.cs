using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyesFollowMouseBehaviour : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Vector2 _originalPos;
    public float scalarMultiple = 3;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _originalPos = transform.position;
    }
    
    void Update()
    {
        UpdateEyesPosition();
    }
    
    private void UpdateEyesPosition()
    {
        Vector2 mousePosition = Input.mousePosition;

        Vector2 mouseDir = _originalPos - mousePosition;
        
        
        transform.position = _originalPos + Vector2.ClampMagnitude(-mouseDir, scalarMultiple);
    }
}
