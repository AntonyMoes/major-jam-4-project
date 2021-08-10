public class Cheat {
    public readonly CheatSymbol[] Sequence;
    public int Pointer { get; private set;  }

    public Cheat(CheatSymbol[] sequence) {
        Sequence = sequence;
    }

    public bool Add(CheatSymbol symbol) {
        if (Sequence[Pointer] != symbol) {
            Reset();
        }

        if (Sequence[Pointer] == symbol) {
            Pointer++;
            if (Pointer == Sequence.Length) {
                return true;
            }
        }

        return false;
    }

    public void Reset() {
        Pointer = 0;
    }
}