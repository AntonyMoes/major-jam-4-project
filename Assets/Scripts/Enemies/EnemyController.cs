using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class EnemyController : MonoBehaviour {
    [SerializeField] float collisionDamage;
    
    Transform _playerTransform;
    DamageDealer _damageDealer;

    protected virtual void Start() {
        _playerTransform = GameObject.FindWithTag("Player").transform;
        _damageDealer = new DamageDealer(tag, collisionDamage);
    }

    protected Vector2 PlayerDirection => (_playerTransform.position - transform.position).normalized;
    protected float PlayerDistance => (_playerTransform.position - transform.position).magnitude;

    void OnCollisionEnter2D(Collision2D other) {
        _damageDealer.OnCollision(other.collider);
    }
}
