using UnityEngine;

public class DamageDealerController : MonoBehaviour {
    [SerializeField] float collisionDamage;
    DamageDealer _damageDealer;

    protected virtual void Start() {
        _damageDealer = new DamageDealer(tag, collisionDamage);
    }

    void OnEnable() {
        Debug.Log("ENABLE");
    }
    
    void OnDisable() {
        Debug.Log("DISABLE");
    }

    void OnCollisionEnter2D(Collision2D other) {
        _damageDealer.OnCollision(other.collider);
    }

    void OnTriggerEnter2D(Collider2D other) {
        Debug.Log("A");
        _damageDealer.OnCollision(other);
    }
}
