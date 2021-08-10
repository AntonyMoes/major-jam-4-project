using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class HUD : MonoBehaviour {
    [SerializeField] Slider healthBar;
    [SerializeField] Text ammoCounter;
    
    [SerializeField] Health playerHealth;
    [SerializeField] Weapon playerWeapon;

    void Start() {
        // healthBar.value = playerHealth.CurrentHealth / playerHealth.MaxHealth;
        playerHealth.OnChangeHealth += (old, current) => {
            DOTween.To(() => healthBar.value, val => healthBar.value = val, current / playerHealth.MaxHealth, 0.3f)
                .SetEase(Ease.OutSine);
        };

        ammoCounter.text = $"{playerWeapon.Ammo} / {playerWeapon.MaxAmmo}";
        playerWeapon.OnAmmoChange += (old, current) => {
            ammoCounter.text = $"{current} / {playerWeapon.MaxAmmo}";
        };
    }
}