using System;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public abstract class Dispenser : MonoBehaviour {
    [SerializeField] float cooldown;
    [SerializeField] GameObject corruptedVersion;
    [SerializeField] Transform respawnTransform;
    float _currentCooldown;
    Collider2D _collider;

    protected virtual void Start() {
        _collider = GetComponent<Collider2D>();
    }

    protected virtual void Update() {
        var oldCooldown = _currentCooldown;
        _currentCooldown = Mathf.Max(0, _currentCooldown - Time.deltaTime);
        if (_currentCooldown == 0 && oldCooldown != 0) {
            OnCooldown(false);
            _collider.enabled = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (!other.gameObject.CompareTag("Player") || _currentCooldown != 0) {
            return;
        }

        _currentCooldown = cooldown;
        OnCooldown(true);
        _collider.enabled = false;

        DispenseLogic(other.gameObject);
    }

    public void Corrupt(bool corrupt) {
        if (corrupt) {
            Instantiate(corruptedVersion, transform.position, Quaternion.identity, respawnTransform);
        }

        gameObject.SetActive(!corrupt);
    }

    protected abstract void DispenseLogic(GameObject player);
    protected virtual void OnCooldown(bool isOnCooldown) { }
}