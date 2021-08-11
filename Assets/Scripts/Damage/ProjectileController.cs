using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D), typeof(Rigidbody2D))]
public class ProjectileController : MonoBehaviour {
    [SerializeField] float damage;
    [SerializeField] float speed;
    
    DamageDealer _damageDealer;

    public void Init(string spawnerTag, Vector2 direction) {
        _damageDealer = new DamageDealer(spawnerTag, damage, () => {
            Destroy(gameObject);
        });
        GetComponent<Rigidbody2D>().velocity = direction * speed;
    }

    void OnTriggerEnter2D(Collider2D other) {
        _damageDealer.OnCollision(other);
    }
}
