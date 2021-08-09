using System;
using UnityEngine;

public class Health : MonoBehaviour {
    [SerializeField] float maxHealth;
    public Action<float, float> OnChangeHealth;
    public Action OnDeath;
    
    public float MaxHealth { get; private set; }
    public float CurrentHealth { get; private set; }

    public bool invulnerable;

    void OnEnable() {
        MaxHealth = maxHealth;
        CurrentHealth = MaxHealth;
        OnChangeHealth?.Invoke(CurrentHealth, CurrentHealth);
    }

    public void AddHealth(float addedHealth) {
        if (invulnerable) {
            return;
        }
        
        var oldHealth = CurrentHealth;
        CurrentHealth = Mathf.Min(MaxHealth, Mathf.Max(0, CurrentHealth + addedHealth));
        OnChangeHealth?.Invoke(oldHealth, CurrentHealth);

        if (CurrentHealth == 0) {
            OnDeath?.Invoke();
        }
    }
}
