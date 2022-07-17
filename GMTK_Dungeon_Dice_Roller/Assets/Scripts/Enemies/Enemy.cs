using System;
using UnityEngine;

namespace Enemies
{
    [RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour
    {
        public int health;
        public int damage;
        public float moveSpeed;
        public Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void OnDuplicate()
        {
            Instantiate(gameObject, transform.position, Quaternion.identity);
        }
        private void OnIncreaseEnemyHealth(float extraHealth)
        {
            if (extraHealth < 0) return;
            health += (int) Math.Round(extraHealth);
        }

        private void OnIncreaseEnemyDamage(float damageIncrease)
        {
            if (damageIncrease < 0) return;
            damage += (int) Math.Round(damageIncrease);
        }

        private void OnIncreaseEnemySpeed(float speedIncrease)
        {
            if (speedIncrease < 0) return;
            moveSpeed += (int) Math.Round(speedIncrease);
        }

        private void SubscribeEvents()
        {
            LevelManager.OnDuplicate += OnDuplicate;
            LevelManager.OnIncreaseEnemyDamage += OnIncreaseEnemyDamage;
            LevelManager.OnIncreaseEnemySpeed += OnIncreaseEnemySpeed;
            LevelManager.OnIncreaseEnemyHealth += OnIncreaseEnemyHealth;
        }

        private void OnDeath()
        {
            LevelManager.BroadcastEvent(LevelManager.Event.EnemyKilled);
            _animator.SetTrigger("Death");
        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.tag != "Harm") return;
            health -= collision.gameObject.HarmBehaviour.GetDamage();
            if (health <= 0)
            {
                OnDeath();
            }
        }
    }
}