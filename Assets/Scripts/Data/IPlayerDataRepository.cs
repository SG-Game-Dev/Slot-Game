namespace UnderpinSlots
{
    public interface IPlayerDataRepository
    {
        bool TryLoad(out PlayerData data);
        void Save(PlayerData data);
        void Clear();
    }
}
