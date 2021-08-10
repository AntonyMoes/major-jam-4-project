using System;
using UnityEngine;

public class CheatDetector : MonoBehaviour {
    static readonly CheatSymbol[] Symbols = (CheatSymbol[]) Enum.GetValues(typeof(CheatSymbol));
    (Cheat, Action)[] _cheats;

    void Start() {
        _cheats = new (Cheat, Action)[] {
            (new Cheat(new[] {CheatSymbol.A, CheatSymbol.B, CheatSymbol.C, CheatSymbol.B, CheatSymbol.A}), () => { Debug.Log("HEALTH"); }),
            (new Cheat(new[] {CheatSymbol.B, CheatSymbol.B, CheatSymbol.B, CheatSymbol.D, CheatSymbol.C}), () => { Debug.Log("AMMO"); }),
        };
    }

    void Update() {
        foreach (var symbol in Symbols) {
            if (!symbol.GetSymbolDown()) {
                continue;
            }
            
            var activated = false;
            foreach (var (cheat, action) in _cheats) {
                activated = cheat.Add(symbol);
                if (!activated) {
                    continue;
                }

                action();
                break;
            }

            if (activated) {
                foreach (var (cheat, _) in _cheats) {
                    cheat.Reset();
                }
            }
        }
    }
}
