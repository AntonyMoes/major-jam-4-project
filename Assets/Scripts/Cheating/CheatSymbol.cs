using System.Collections.Generic;
using UnityEngine;

public enum CheatSymbol {
    A,
    B,
    C,
    D
}

public static class CheatSymbolExtensions {
    static readonly Dictionary<CheatSymbol, string> CheatButtons = new Dictionary<CheatSymbol, string> {
        {CheatSymbol.A, "CheatA"},
        {CheatSymbol.B, "CheatB"},
        {CheatSymbol.C, "CheatC"},
        {CheatSymbol.D, "CheatD"},
    };
    
    static readonly Dictionary<CheatSymbol, string> CheatControllerAxis = new Dictionary<CheatSymbol, string> {
        {CheatSymbol.C, "CheatCController"},
        {CheatSymbol.D, "CheatDController"},
    };

    static Dictionary<string, bool> _wasAsixPressed = new Dictionary<string, bool>();

    static bool GetAxisDown(CheatSymbol symbol) {
        if (!CheatControllerAxis.ContainsKey(symbol)) {
            return false;
        }

        var axis = CheatControllerAxis[symbol];
        
        var current = Input.GetAxisRaw(axis) > 0;
        var previous = _wasAsixPressed.GetOrDefault(axis);

        _wasAsixPressed[axis] = current;

        return current && !previous;
    }

    public static bool GetSymbolDown(this CheatSymbol symbol) {
        return Input.GetButtonDown(CheatButtons[symbol]) || GetAxisDown(symbol);
    }
}