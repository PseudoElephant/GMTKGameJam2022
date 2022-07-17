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
    public float dashDistance = 5f;
    public int dashCount;

    public float dashHardCooldown;

    private float _currentDashCooldown;

    private bool _dashing;

    Vector2 _moveDirection = Vector2.zero;

    Vector2 _dashTarget;

    float _startDistance;

    int _currentDashCount;

    Rigidbody2D _rb;

    public void Awake(){
        dashAction.performed += _ => OnDash();
        _currentDashCount = dashCount;
    }

    public void SubscribeEvents() {
        LevelManager.OnIncreaseSpeed += extra => moveSpeed += extra;
        LevelManager.OnDashCountChange += count => {
             dashCount += (int)count;
             dashCount = Math.Min(dashCount, _currentDashCount);
         };
         
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

    void OnDash() {
        Debug.Log("dashing");
        if (_currentDashCooldown <= 0 && !_dashing && _moveDirection.magnitude != 0) {
            _dashing = true;
            _currentDashCount--;
            if (_currentDashCount <= 0) {
                _currentDashCooldown = dashHardCooldown;
                 StartCoroutine(WaitThen(dashHardCooldown, () => {
                _currentDashCooldown = 0;
                _currentDashCount = dashCount;
            }));
            }
           
           Vector3 currPos = transform.position;
            _dashTarget =  currPos + new Vector3(_moveDirection.x, _moveDirection.y, 0) * dashDistance;
             
             RaycastHit2D hit = Physics2D.Raycast(new Vector3(currPos.x - 2f, currPos.y - 2f, 0), _moveDirection, dashDistance);
             // Clamping
             if (hit.collider != null) {
                _dashTarget = hit.point;
             }

            _startDistance = Vector3.Distance(transform.position, _dashTarget);
        }
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

            float percent = 1 -  Vector3.Distance(transform.position, _dashTarget) / _startDistance;

            transform.position = Vector3.Lerp(transform.position, _dashTarget, percent);

            if (percent > 0.8) {
                _dashing = false;
            }

            return;
        }
        _rb.velocity = _moveDirection * moveSpeed;
    }
}
