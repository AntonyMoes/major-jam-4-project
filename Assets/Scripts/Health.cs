using System;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] float maxHealth;
    public Action<float, float> OnChangeHealth;
    public Action OnDeath;
    
    public float MaxHealth { get; private set; }

    float _currentHealth;
    public float CurrentHealth {
        get => _currentHealth;
        private set {
            var oldHealth = CurrentHealth;
            _currentHealth = Mathf.Min(MaxHealth, Mathf.Max(0, value));
            OnChangeHealth?.Invoke(oldHealth, CurrentHealth);
        }
    }

    public bool invulnerable;

    void Start() {
        Init();
    }

    public void Init() {
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
    }

    public void AddHealth(float addedHealth) {
        if (invulnerable) {
            return;
        }

        CurrentHealth += addedHealth;

        if (CurrentHealth == 0) {
            OnDeath?.Invoke();
        }
    }
}
