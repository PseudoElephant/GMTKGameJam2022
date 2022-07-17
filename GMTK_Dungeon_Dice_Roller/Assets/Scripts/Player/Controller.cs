using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

[RequireComponent(typeof(Rigidbody2D))]
public class Controller : MonoBehaviour
{

    [Header("Controls")] 
    [Tooltip("Configure your inputs")]
    public InputAction playerControls;
    public InputAction dashAction;

    [Header("Movement")] 
    [Tooltip("Configure the player movement")]
    public float moveSpeed = 5f;
    public float dashSpeed = 10f;
    public float dashTime = 1f;

    public float dashHardCooldown;

    private float _currentDashCooldown;

    private bool _dashing;

    private Vector2 _dashDirection;

    Vector2 _moveDirection = Vector2.zero;

    Vector2 _dashTarget;

    float _startDistance;

    int _currentDashCount;

    private bool _canDash = true;

    Rigidbody2D _rb;

    public void Awake(){
        dashAction.performed += _ => OnDash();
        SubscribeEvents();
    }

    private void SubscribeEvents() {
        LevelManager.OnIncreaseSpeed += extra => moveSpeed += extra;
    }

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
    }
    void OnEnable() {
        playerControls.Enable();
        dashAction.Enable();
    }

    void OnDisable() {
        playerControls.Disable();
        dashAction.Disable();
    }

    public Vector2 GetMoveDirection() {
        return _moveDirection;
    }
    void OnDash()
    {
        if (!_canDash) return;

        if (_currentDashCooldown <= 0 && !_dashing && _moveDirection.magnitude != 0)
        {
            DashAnimation();
            
            _canDash = false;
            _dashing = true;
            _dashDirection = _moveDirection;
            
            StartCoroutine(DashDuration());
            
            if (_currentDashCount <= 0) {
                _currentDashCooldown = dashHardCooldown;
                 StartCoroutine(WaitThen(dashHardCooldown, () => 
                 {
                    _currentDashCooldown = 0;
                 }));
            }
        }
    }

    private void DashAnimation()
    {
        Vector2 _originalScale = Vector2.one;
        LeanTween.scale(gameObject, transform.localScale + (Vector3.one * 0.15f), 0.1f).setEaseOutSine().setOnComplete(
            () =>
            {
                LeanTween.scale(gameObject, _originalScale, 0.3f).setEaseOutSine();
            });
    }

    private void CanDashAgainAnimation()
    {
        LeanTween.rotateLocal(gameObject, new Vector3(0f, 0f, -6f), 0.1f).setEaseInOutSine().setOnComplete(() =>
        {
            LeanTween.rotateLocal(gameObject, new Vector3(0f, 0f, 6f), 0.1f).setEaseInOutSine().setOnComplete(() =>
            {
                LeanTween.rotateLocal(gameObject, Vector3.zero, 0.1f).setEaseInOutSine();
            });
        });
    }

    IEnumerator DashDuration()
    {
        yield return new WaitForSeconds(dashTime);
        _dashing = false;
        
        yield return new WaitForSeconds(dashHardCooldown);
        CanDashAgainAnimation();
        _canDash = true;
    }

    IEnumerator WaitThen(float cooldown, Action callback) {
        yield return new WaitForSecondsRealtime(cooldown);
        callback();
    }


    // Start is called before the first frame update
    void Update()
    {
        _moveDirection = playerControls.ReadValue<Vector2>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (_dashing) {
            _rb.velocity = _dashDirection * dashSpeed;
            return;
        }
        _rb.velocity = _moveDirection * moveSpeed;
    }
}
