namespace UnderpinSlots
{
    public interface IReel
    {
        bool IsStopped { get; }
        // Raised when this reel has finished stopping.
        event System.Action Stopped;
        SymbolType StoppedSymbol { get; }

        // target = symbol that must end on the pay line; runTime = seconds at full speed.
        void Spin(SymbolType target, float runTime);
    }
}