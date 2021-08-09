using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D), typeof(Health), typeof(Collider2D))]
public class Slime : EnemyController {
    [SerializeField] float speed;
    [SerializeField] float jumpRange;
    [SerializeField] float jumpTime;
    
    bool _isJumping;
    Coroutine _jumpCoro;
    Rigidbody2D _rb;

    protected override void Start() {
        base.Start();
        
        _rb = GetComponent<Rigidbody2D>();
        GetComponent<Health>().OnDeath += () => {
            StopCoroutine(_jumpCoro);
            _isJumping = false;

            gameObject.SetActive(false);
        };
    }

    void FixedUpdate() {
        if (_isJumping) {
            return;
        }
        
        var direction = PlayerDirection;
        
        if (PlayerDistance <= jumpRange) {
            _isJumping = true;
            _jumpCoro = StartCoroutine(Jump(direction));
            return;
        }

        _rb.velocity = PlayerDirection * speed;
    }

    IEnumerator Jump(Vector2 direction) {
        var setupTime = jumpTime * 0.2f;
        var moveTime = jumpTime * 0.65f;
        var staggerTime = jumpTime * 0.15f;
        
        
        _rb.velocity = Vector2.zero;
        
        yield return new WaitForSeconds(setupTime);

        var jump = DOTween.To(() => _rb.position, (value) => _rb.position = value, _rb.position + direction * jumpRange, moveTime)
            .SetEase(Ease.OutSine);

        yield return new DOTweenCYInstruction.WaitForCompletion(jump);

        yield return new WaitForSeconds(staggerTime);

        _isJumping = false;
    }
}
