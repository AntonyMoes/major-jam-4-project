using UnityEngine;

public class AmmoDispenser : Dispenser {
    [SerializeField] int dispensedAmmo;
    protected override void DispenseLogic(GameObject player) {
        player.GetComponent<Weapon>().Ammo += dispensedAmmo;
    }

    protected override void OnCooldown(bool isOnCooldown) {
        Debug.Log($"Ammo dispenser cooldown: {isOnCooldown}");
    }
}
