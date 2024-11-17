using System;
using System.Collections.Generic;

namespace ComponentSystem
{
    public static class AlternateInitializator
    {
        public static void InitializeAlternately(IAlternateInitializableCollection initializableCollection)
        {
            Stack<int> initializationStack = new();

            while (initializableCollection.IsCollectionInitialized() == false)
            {
                int currentItemIndex = initializationStack.Count > 0 ? initializationStack.Pop() : initializableCollection.GetNextUninitializedIndex();

                bool isItemInit = TryInitialize(initializableCollection, currentItemIndex, out int uninitializedDependentIndex);

                if (isItemInit)
                    continue;

                if (initializationStack.Contains(uninitializedDependentIndex))
                    throw new Exception($"Item by index {uninitializedDependentIndex} produces an infinite loop in the initialization queue");

                initializationStack.Push(currentItemIndex);
                initializationStack.Push(uninitializedDependentIndex);
            }
        }

        private static bool TryInitialize(IAlternateInitializableCollection initializableCollection, int currentIndex,
            out int uninitializedDependentIndex)
        {
            uninitializedDependentIndex = -1;

            if (initializableCollection.IsInitialized(currentIndex))
                return true;

            int[] dependentIndexes = initializableCollection.MustInitializeBedfore(currentIndex);

            if (dependentIndexes.Length == 0)
            {
                initializableCollection.Initialize(currentIndex);
                return true;
            }

            foreach (int dependentIndex in dependentIndexes)
                if (initializableCollection.IsInitialized(dependentIndex) == false)
                {
                    uninitializedDependentIndex = dependentIndex;
                    return false;
                }

            initializableCollection.Initialize(currentIndex);
            return true;
        }
    }
}
