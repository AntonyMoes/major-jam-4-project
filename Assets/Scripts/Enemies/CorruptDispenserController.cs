using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public abstract class CorruptDispenserController : EnemyController {
    [SerializeField] float speed;

    Rigidbody2D _rb;

    bool _isAttacking;
    Coroutine _attackCoro;

    protected override void Start() {
        base.Start();

        _rb = GetComponent<Rigidbody2D>();
        GetComponent<Health>().OnDeath += () => {
            Destroy(gameObject);
        };
    }

    void FixedUpdate() {
        var direction = GetMoveDirection(Time.fixedDeltaTime);
        
        if (_isAttacking) {
            _rb.velocity = Vector2.zero;
            return;
        }

        _rb.velocity = direction * speed;
    }

    protected virtual void Update() {
        if (_isAttacking || !ShouldAttack()) {
            return;
        }

        _isAttacking = true;
        _attackCoro = StartCoroutine(Attack());
    }


    IEnumerator Attack() {
        yield return AttackLogic();
        _isAttacking = false;
    }

    protected abstract Vector2 GetMoveDirection(float deltaTime);
    protected abstract bool ShouldAttack();
    protected abstract IEnumerator AttackLogic();
}
