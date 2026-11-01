namespace UnderpinSlots
{
    // Abstraction over RNG so a fixed/fake generator can be used in tests.
    public interface IOutcomeGenerator
    {
        SymbolType Next();
    }
}
