using System;
using System.Collections;
using System.Collections.Generic;
using Enemies;
using Unity.VisualScripting;
using UnityEngine;


public class EnemySquid : Enemy
{
    
    public GameObject bulletPrefab;
    public float shootSpeed;
    public int numOfBulletsToShoot;
    public float shootCooldown;
    public float shootCooldownBetweenBullets;
    
    private GameObject _playerTarget;
    private bool _canShoot = true;
    private Rigidbody2D _rigidBody;
    
    
    // Start is called before the first frame update
    private void Awake()
    {
        _playerTarget = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
    }
    
    // Update is called once per frame
    void Update()
    {
        Move();
        if (_canShoot)
        {
            StartCoroutine(Shoot());
        }
    }
    
    private IEnumerator Shoot()
    {
        _canShoot = false;
        
        yield return new WaitForSeconds(shootCooldown);
        
        int currentFiredBullets = 0;
        while (currentFiredBullets != numOfBulletsToShoot)
        {
            Vector2 shootDir = _playerTarget.transform.position - transform.position;
            _animator.SetTrigger("Shoot");
            
            AudioManager.Play("sfx_EnemyShoot");
            GameObject newBullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            
            Bullet bullet = newBullet.GetComponent<Bullet>();
            bullet.SetDirection(shootDir.normalized);
            bullet.SetDamage(damage);
            bullet.SetSpeed(shootSpeed);

            currentFiredBullets++;
            yield return new WaitForSeconds(shootCooldownBetweenBullets);
        }
        
        _canShoot = true;
    }
    
    private void Move()
    {
        Vector2 moveDir = (_playerTarget.transform.position - transform.position).normalized;
        _rigidBody.velocity = moveDir * moveSpeed;
    }
    
}
