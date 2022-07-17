using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySaw : MonoBehaviour
{
    public int health;
    public int damage;
    public GameObject bulletPrefab;
    public float moveSpeed;
    public float shootSpeed;
    
    private GameObject _playerTarget;
    // Start is called before the first frame update
    private void Awake()
    {
        _playerTarget = GameObject.FindWithTag("Player");
    }
}
