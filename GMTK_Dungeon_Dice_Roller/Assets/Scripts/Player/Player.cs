using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;
using System;

[RequireComponent(typeof(Controller))]
public class Player : MonoBehaviour
{

    public int health = 10;
    void Start() {
        Debug.Log("Player Start");
        LevelManager.SetHealth(health);
    }

}
