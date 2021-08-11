using System.Collections;
using UnityEngine;

public class CorruptAmmoDispenser : CorruptDispenserController {
    [SerializeField] float attackDistance;
    [SerializeField] float attackCooldown;
    [SerializeField] float attackDuration;
    [SerializeField] float minDistance;
    [SerializeField] float attackDegreeDelta;
    [SerializeField] int projectileCount;
    [SerializeField] GameObject projectilePrefab;

    float _currentAttackCooldown;
    float _oneDirectionTime;
    Vector2 _wanderDirection;

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

        var chargeTime = 0.2f * attackDuration;
        var attackTime = 0.5f * attackDuration;
        var staggerTime = 0.3f * attackDuration;

        yield return new WaitForSeconds(chargeTime);

        var angle = Vector2.SignedAngle(PlayerDirection, Vector2.up);
        
        for (var i = 0; i < projectileCount; i++) {
            yield return new WaitForSeconds(attackTime / projectileCount);
            
            var direction = (angle + Random.Range(-attackDegreeDelta, attackDegreeDelta)).DegreesToVector();
            
            var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
            projectile.GetComponent<ProjectileController>().Init(tag, direction);
        }

        yield return new WaitForSeconds(staggerTime);
    }
}
