using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Bullet : MonoBehaviour
{

    [Header("Bullet")] 
    [Tooltip("Configure the bullet props")]
    public int damage = 5;
    public float shootSpeed = 1f;
    private Vector2 _shootDir;

    public float maxLife = 5f;

    private Rigidbody2D _rb;

    // Start is called before the first frame update
    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        StartCoroutine(KillUntil());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "HarmEnemy" || other.gameObject.tag == "HarmPlayer") return;
        
        if (other.gameObject.tag != "Player" && other.gameObject.tag != "Enemy")
            Destroy(gameObject);
        
        if (gameObject.tag == "HarmEnemy" && other.gameObject.tag == "Enemy")
            Destroy(gameObject);

        if (gameObject.tag == "HarmPlayer" && other.gameObject.tag == "Player")
             Destroy(gameObject);
    }
 

    IEnumerator KillUntil() {
        yield return new WaitForSecondsRealtime(maxLife);
        Destroy(this);
    }

   public float GetDamage() {
        return damage;
   }

   public void SetSpeed(float speed) {
        this.shootSpeed =  speed;
   }

    public void SetDamage(int dmg) {
        this.damage =  dmg;
   }

    public void SetDirection(Vector2 dir) {
        this._shootDir =  dir;
   }

    void FixedUpdate() {
        _rb.velocity = _shootDir * shootSpeed;
    }
}
