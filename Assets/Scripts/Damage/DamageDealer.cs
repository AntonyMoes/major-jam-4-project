using System;
using UnityEngine;

public class DamageDealer {
    readonly string _ignoredTag;
    readonly float _damage;
    readonly Action _onCollision;

    public DamageDealer(string ignoredTag, float damage, Action onCollision = null) {
        _ignoredTag = ignoredTag;
        _damage = damage;
        _onCollision = onCollision;
    }
    
    public void OnCollision(Collider2D other) {
        if (other.gameObject.CompareTag(_ignoredTag) || other.gameObject.CompareTag("IgnoreCollisions")) {
            return;
        }

        var hasHealth = other.gameObject.TryGetComponent(out Health health);
        if (hasHealth) {
            health.AddHealth(-_damage);
        }
        
        _onCollision?.Invoke();
    }
}
