using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmBehaviour : MonoBehaviour
{
    public int damage = 5;
    
    public float GetDamage() {
        return damage;
    }
    
    public void SetDamage(int dmg) {
        this.damage =  dmg;
    }
}
