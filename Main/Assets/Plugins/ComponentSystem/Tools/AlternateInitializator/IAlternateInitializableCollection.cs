namespace ComponentSystem
{
    public interface IAlternateInitializableCollection
    {
        int GetNextUninitializedIndex();

        void Initialize(int index);

        int[] MustInitializeBedfore(int index);

        bool IsInitialized(int index);

        bool IsCollectionInitialized();
    }
}
