using System;
using System.Collections;
using Enemies;
using UnityEngine;
using Random = UnityEngine.Random;


public class EnemySaw : Enemy
{
    public GameObject bulletPrefab;
    public float shootSpeed;
    public int numOfBulletsToShoot;
    public float attackCooldown;
    public float DashTime;
    public float DashSpeed;
    public CircleCollider2D triggerDamage;

    private bool _isSpinAttacking;
    private Vector2 _spinAttackDirection;
    private Vector2 _moveDir;
    private GameObject _playerTarget;
    private bool _canAttack = true;
    private Rigidbody2D _rigidBody;
    // Start is called before the first frame update
    private void Awake()
    {
        triggerDamage.enabled = false;
        _playerTarget = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    
    void Update()
    {
        Move();
        if (_canAttack)
        {
            if (Random.value > 0.5)
            {
                StartCoroutine(Shoot());
            }
            else
            {
                StartCoroutine(DashAttack());    
            }
        }
    }

    private void AnimateShoot()
    {
        Vector3 originalScale = transform.localScale;
        LeanTween.scale(gameObject, originalScale + new Vector3(0.05f, 0.05f, 0.1f), 0.1f).setEaseInOutSine()
            .setOnComplete(
                () =>
                {
                    LeanTween.scale(gameObject, originalScale, 0.1f).setEaseInOutSine();
                });
        LeanTween.rotateLocal(gameObject, new Vector3(0f, 0f, -3f), 0.1f).setEaseInOutSine().setOnComplete(() =>
        {
            LeanTween.rotateLocal(gameObject, new Vector3(0f, 0f, 3f), 0.1f).setEaseInOutSine().setOnComplete(() =>
            {
                LeanTween.rotateLocal(gameObject, Vector3.zero, 0.1f).setEaseInOutSine();
            });
        });
    }
    
    private IEnumerator Shoot()
    {
        _canAttack = false;
        
        yield return new WaitForSeconds(attackCooldown);
        
        _animator.SetTrigger("Shoot");
        AudioManager.Play("sfx_EnemyShoot");
        AnimateShoot();

        for (int i = 0; i < numOfBulletsToShoot; i++)
        {
            Vector2 shootDir = new Vector2((float) Math.Cos((2*Math.PI/numOfBulletsToShoot)*i), (float) Math.Sin((2*Math.PI/numOfBulletsToShoot)*i));
            GameObject newBullet = Instantiate(bulletPrefab, transform.position + new Vector3(shootDir.x,shootDir.y,0).normalized, Quaternion.identity);
            
            Bullet bullet = newBullet.GetComponent<Bullet>();
            bullet.SetDirection(shootDir.normalized);
            bullet.SetDamage(damage);
            bullet.SetSpeed(shootSpeed);
        }
        
        _canAttack = true;
    }

    private IEnumerator DashAttack()
    {
        _canAttack = false;
        
        yield return new WaitForSeconds(attackCooldown);

        _isSpinAttacking = true;
        triggerDamage.enabled = true;
        
        _animator.SetTrigger("Spin_Attack");
        AudioManager.Play("sfx_EnemyShootHard");

        _spinAttackDirection = _moveDir;

        yield return new WaitForSeconds(DashTime);

        triggerDamage.enabled = false;
        _canAttack = true;
        _isSpinAttacking = false;
    }
    
    private void Move()
    {
        _moveDir = (_playerTarget.transform.position - transform.position).normalized;

        if (_isSpinAttacking)
        {
            _rigidBody.velocity = _spinAttackDirection * DashSpeed;
            return;
        }
        
        _rigidBody.velocity = _moveDir * moveSpeed;
    }
}
