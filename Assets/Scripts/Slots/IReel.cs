namespace UnderpinSlots
{
    public interface IReel
    {
        bool IsStopped { get; }
        SymbolType StoppedSymbol { get; }

        // target = symbol that must end on the pay line; runTime = seconds at full speed.
        void Spin(SymbolType target, float runTime);
    }
}
