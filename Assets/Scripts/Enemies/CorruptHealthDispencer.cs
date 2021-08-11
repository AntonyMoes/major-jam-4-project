using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using UnityEngine;

public class CorruptHealthDispencer : CorruptDispenserController {
    [SerializeField] float attackDistance;
    [SerializeField] float attackCooldown;
    [SerializeField] float attackDuration;
    [SerializeField] float attackDamage;
    [SerializeField] float minDistance;
    [SerializeField] SpriteRenderer attackRenderer;

    DamageDealer _attack;

    float _currentAttackCooldown;
    float _oneDirectionTime;
    Vector2 _wanderDirection;

    protected override void Start() {
        base.Start();

        _attack = new DamageDealer(tag, attackDamage);
        GetComponent<Health>().OnDeath += () => {
            attackRenderer.color = attackRenderer.color.WithAlpha(0);
        };
    }

    protected override Vector2 GetMoveDirection(float deltaTime) {
        _oneDirectionTime = Mathf.Max(0, _oneDirectionTime - deltaTime);
        
        if (_oneDirectionTime != 0) {
            return _wanderDirection;
        }
        
        _oneDirectionTime = Random.Range(0.3f, 0.5f);
        
        if (PlayerDistance > attackDistance) {
            _wanderDirection = PlayerDirection.normalized;
        } else if (PlayerDistance < minDistance) {
            return -PlayerDirection.normalized;
        } else {
            _wanderDirection = Random.insideUnitCircle.normalized;
        }

        return _wanderDirection;
    }

    protected override void Update() {
        base.Update();

        _currentAttackCooldown = Mathf.Max(0, _currentAttackCooldown - Time.deltaTime);
    }

    protected override bool ShouldAttack() {
        return _currentAttackCooldown == 0 && PlayerDistance <= attackDistance;
    }

    protected override IEnumerator AttackLogic() {
        _currentAttackCooldown = attackCooldown;

        var chargeTime = 0.3f * attackDuration;
        var attackTime = 0.4f * attackDuration;
        var staggerTime = 0.3f * attackDuration;

        yield return new WaitForSeconds(chargeTime);

        DOGetter<float> alphaGetter = () => attackRenderer.color.a;
        DOSetter<float> alphaSetter = value => attackRenderer.color = attackRenderer.color.WithAlpha(value);

        var first = DOTween.To(alphaGetter, alphaSetter, 1, attackTime * 0.5f).SetEase(Ease.InExpo);
        var second = DOTween.To(alphaGetter, alphaSetter, 0, attackTime * 0.5f).SetEase(Ease.OutExpo);
        var attack = DOTween.Sequence()
            .Append(first)
            .AppendCallback(() => {
                var hits = Physics2D.CircleCastAll(transform.position, attackDistance, Vector2.zero, 0);
                foreach (var hit in hits) {
                    _attack.OnCollision(hit.collider);
                }
            })
            .Append(second);
        yield return new DOTweenCYInstruction.WaitForCompletion(attack);

        yield return new WaitForSeconds(staggerTime);
    }
}
