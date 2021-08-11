using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CheatDetector : MonoBehaviour {
    enum CheatType {
        Health,
        Ammo,
        // Shooting,
        // Moving
    }

    [SerializeField] PlayerController playerController;
    [SerializeField] Health playerHealth;
    [SerializeField] Weapon playerWeapon;

    static readonly CheatSymbol[] Symbols = (CheatSymbol[]) Enum.GetValues(typeof(CheatSymbol));
    Dictionary<CheatType, (Cheat, Action)> _cheats;
    Dictionary<CheatType, Action<bool>> _onShuffle; 
    Dictionary<CheatType, (Cheat, Action)> _currentCheats = new Dictionary<CheatType, (Cheat, Action)>();

    const int ShuffledCheats = 1;
    const int CheatsTillShuffle = 3;
    int _currentCheatsTillShuffle;

    void Start() {
        _cheats = new Dictionary<CheatType, (Cheat, Action)> {
             {
                CheatType.Health,
                (new Cheat(new[] {CheatSymbol.A, CheatSymbol.B, CheatSymbol.C, CheatSymbol.B, CheatSymbol.A}),
                    WithTryShuffle(HealthCheat))
            },
            {
                CheatType.Ammo, (
                    new Cheat(new[] {CheatSymbol.B, CheatSymbol.B, CheatSymbol.B, CheatSymbol.D, CheatSymbol.C}),
                    WithTryShuffle(AmmoCheat))
            },
        };

        _onShuffle = new Dictionary<CheatType, Action<bool>> {
            {CheatType.Health, OnHealthShuffle},
            {CheatType.Ammo, OnAmmoShuffle},
        };

        playerHealth.OnDeath += Shuffle;
        playerController.OnNewRespawn += respawn => {
            Shuffle();
        };
    }

    void Shuffle() {
        foreach (var cheatType in _currentCheats.Keys) {
            _onShuffle[cheatType](false);
        }
        
        _currentCheats = ((CheatType[]) Enum.GetValues(typeof(CheatType)))
            .OrderBy(t => Guid.NewGuid())
            .Take(ShuffledCheats)
            .ToDictionary(t => t, t => _cheats[t]);
        
        Debug.Log("Cheat rotation: " + string.Join(", ", _currentCheats.Keys));

        _currentCheatsTillShuffle = CheatsTillShuffle;
        
        foreach (var cheatType in _currentCheats.Keys) {
            _onShuffle[cheatType](true);
        }
    }

    void Update() {
        foreach (var symbol in Symbols) {
            if (!symbol.GetSymbolDown()) {
                continue;
            }

            var activated = false;
            foreach (var (cheat, action) in _currentCheats.Values) {
                activated = cheat.Add(symbol);
                if (!activated) {
                    continue;
                }

                action();
                break;
            }

            if (activated) {
                foreach (var (cheat, _) in _currentCheats.Values) {
                    cheat.Reset();
                }
            }
        }
    }

    void HealthCheat() {
        playerHealth.AddHealth(50);
    }

    void OnHealthShuffle(bool shuffledIn) {
        foreach (var dispenser in playerController.Respawn.healthDispensers) {
            dispenser.Corrupt(shuffledIn);
        }
    }

    void AmmoCheat() {
        playerWeapon.Ammo += 10;
    }
    
    void OnAmmoShuffle(bool shuffledIn) {
        foreach (var dispenser in playerController.Respawn.ammoDispensers) {
            dispenser.Corrupt(shuffledIn);
        }
    }

    void ShootingCheat() { }

    void MovingCheat() { }

    Action WithTryShuffle(Action action) {
        return () => {
            action();

            _currentCheatsTillShuffle--;
            if (_currentCheatsTillShuffle == 0) {
                Shuffle();
            }
        };
    }
}
