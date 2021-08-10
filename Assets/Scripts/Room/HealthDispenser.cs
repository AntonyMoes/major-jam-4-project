
using UnityEngine;

public class HealthDispenser : Dispenser {
    [SerializeField] float dispensedHealth;

    protected override void DispenseLogic(GameObject player) {
        player.GetComponent<Health>().AddHealth(dispensedHealth);
    }

    protected override void OnCooldown(bool isOnCooldown) {
        Debug.Log($"Health dispenser cooldown: {isOnCooldown}");
    }
}
