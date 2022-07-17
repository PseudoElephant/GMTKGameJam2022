using System;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Enemies
{
    [RequireComponent(typeof(Animator), typeof(Rigidbody2D))]
    public class Enemy : MonoBehaviour
    {
        public int health;
        public int damage;
        public float moveSpeed;
        public Animator _animator;
        private bool _isDying = false;

        private void Start()
        {
            _animator = GetComponent<Animator>();
            SubscribeEvents();
            LevelManager.Instance.AddEnemy();
            ApplyBuffs();
        }

        private void ApplyBuffs() 
        {
            LevelManager.EntityBuff buffs = LevelManager.GetEnemyBuffs();
            damage = Math.Clamp(damage + buffs.damage, 0, 1000);
            health = Math.Clamp(health + buffs.health, 1, 1000);
            moveSpeed = Math.Clamp(moveSpeed + buffs.speed, 0, 1000);
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
            if (_isDying) return;

            _isDying = true;
            _animator.SetTrigger("Death");
            LeanTween.scale(gameObject, Vector3.zero, 1f).setEaseInOutSine().setOnComplete(() => Destroy(gameObject));
            LevelManager.BroadcastEvent(LevelManager.Event.EnemyKilled);

            if (Random.value > 0.5)
            {
                LevelManager.BroadcastEvent(LevelManager.Event.DiceRoll);
                LevelManager.BroadcastEvent(LevelManager.Event.BadRoll);
            }
        }

        private void OnHit()
        {
            AudioManager.Play("sfx_onPlayerHit");
            Vector3 originalScale = transform.localScale;
            LeanTween.scale(gameObject, originalScale + new Vector3(0.05f, 0.05f, 0.05f), 0.1f).setEaseInOutSine()
                .setOnComplete(
                    () =>
                    {
                        LeanTween.scale(gameObject, originalScale, 0.1f).setEaseInOutSine();
                    });
            LeanTween.rotateLocal(gameObject, new Vector3(0f, 0f, 0.4f), 0.1f).setEaseInOutSine().setOnComplete(() =>
            {
                LeanTween.rotateLocal(gameObject, Vector3.zero, 0.1f).setEaseInOutSine();
            });
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            if (col.gameObject.tag != "HarmEnemy") return;
            
            health -= col.gameObject.GetComponent<Bullet>().damage;
            if (health <= 0)
            {
                OnDeath();
            }
            
            OnHit();
        }

        private void OnDestroy()
        {
            LevelManager.OnDuplicate -= OnDuplicate;
            LevelManager.OnIncreaseEnemyDamage -= OnIncreaseEnemyDamage;
            LevelManager.OnIncreaseEnemySpeed -= OnIncreaseEnemySpeed;
            LevelManager.OnIncreaseEnemyHealth -= OnIncreaseEnemyHealth;
        }
    }
}