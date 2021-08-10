using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Weapon : MonoBehaviour {
    [SerializeField] Transform crosshair;
    [SerializeField] GameObject projectilePrefab;
    [SerializeField] InputDetector inputDetector;
    [SerializeField] float cooldown;
    [SerializeField] int maxAmmo;

    Rigidbody2D _rb;
    
    Vector2 _lastAimDirection;
    float _currentCooldown;
    int _ammo;

    public int MaxAmmo { get; private set; }
    public int Ammo {
        get => _ammo;
        set {
            var oldAmmo = _ammo;
            _ammo = Mathf.Min(MaxAmmo, value);
            OnAmmoChange?.Invoke(oldAmmo, _ammo);
        }
    }
    public Action<int, int> OnAmmoChange;

    void Start() {
        _rb = GetComponent<Rigidbody2D>();
        Init();
    }

    public void Init() {
        _lastAimDirection = transform.forward;
        _currentCooldown = 0;
        MaxAmmo = maxAmmo;
        Ammo = MaxAmmo;
    }

    void Update() {
        _currentCooldown = Mathf.Max(0, _currentCooldown - Time.deltaTime);
    }

    public void Aim(Vector2 direction) {
        if (inputDetector.State == InputDetector.EInputState.Controller && direction == Vector2.zero) {
            var velocity = _rb.velocity;
            direction = velocity == Vector2.zero ? _lastAimDirection : velocity.normalized;
        }
        _lastAimDirection = direction;

        crosshair.position = transform.position + (Vector3) direction;
    }
    
    public void Shoot(Vector2 direction) {
        if (_currentCooldown != 0 || Ammo <= 0) {
            return;
        }

        _currentCooldown = cooldown;
        Ammo--;
        
        var projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        projectile.GetComponent<ProjectileController>().Init(tag, direction.normalized);
    }
}
