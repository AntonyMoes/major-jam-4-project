using System;
using System.Collections;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Rigidbody2D), typeof(Health), typeof(Collider2D))]
public class Slime : EnemyController {
    [SerializeField] float speed;
    [SerializeField] float jumpRange;
    [SerializeField] float jumpTime;
    [SerializeField] float jumpCooldown;

    bool _isJumping;
    float _currentCooldown;
    Coroutine _jumpCoro;
    Rigidbody2D _rb;

    protected override void Start() {
        base.Start();
        
        _rb = GetComponent<Rigidbody2D>();
        GetComponent<Health>().OnDeath += () => {
            if (_isJumping) {
                StopCoroutine(_jumpCoro);
                _isJumping = false;
            }

            _rb.velocity = Vector2.zero;
            gameObject.SetActive(false);
        };
    }

    void FixedUpdate() {
        if (_isJumping) {
            return;
        }

        _rb.velocity = PlayerDirection * speed;
    }

    void Update() {
        _currentCooldown = Mathf.Max(0, _currentCooldown - Time.deltaTime);
        
        if (PlayerDistance <= jumpRange && _currentCooldown == 0) {
            _isJumping = true;
            _currentCooldown = jumpCooldown;
            _jumpCoro = StartCoroutine(Jump(PlayerDirection));
        }
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
