namespace Medici.Collections
{
    public delegate void ItemAddedDelegate(object item);
    public delegate void ItemAddedDelegate<in T>(T item);

    public delegate void ItemRemovedDelegate(object item);
    public delegate void ItemRemovedDelegate<in T>(T item);

    public delegate void CollectionClearedDelegate();

    public interface ICollectionEvents
    {
        event ItemAddedDelegate OnItemAdded;

        event ItemRemovedDelegate OnItemRemoved;

        event CollectionClearedDelegate OnCollectionCleared;
    }

    public interface ICollectionEvents<out T> : ICollectionEvents
    {
        new event ItemAddedDelegate<T> OnItemAdded;

        new event ItemRemovedDelegate<T> OnItemRemoved;
    }

    public abstract class NotifyingCollectionBase : ICollectionEvents
    {
        protected NotifyingCollectionBase() { }

        public event ItemAddedDelegate OnItemAdded;

        public event ItemRemovedDelegate OnItemRemoved;

        public event CollectionClearedDelegate OnCollectionCleared;

        protected void RaiseOnItemAdded(object item)
        {
            if (OnItemAdded != null)
            {
                OnItemAdded(item);
            }
        }

        protected void RaiseOnItemRemoved(object item)
        {
            if (OnItemRemoved != null)
            {
                OnItemRemoved(item);
            }
        }

        protected void RaiseCollectionCleared()
        {
            if (OnCollectionCleared != null)
            {
                OnCollectionCleared();
            }
        }

    }

    public abstract class NotifyingCollectionBase<T> : ICollectionEvents<T>
    {
        protected NotifyingCollectionBase() { }

        public event ItemAddedDelegate<T> OnItemAdded;

        public event ItemRemovedDelegate<T> OnItemRemoved;

        public event CollectionClearedDelegate OnCollectionCleared;

        protected void RaiseOnItemAdded(T item)
        {
            if (OnItemAdded != null)
            {
                OnItemAdded(item);
            }
        }

        protected void RaiseOnItemRemoved(T item)
        {
            if (OnItemRemoved != null)
            {
                OnItemRemoved(item);
            }
        }

        protected void RaiseCollectionCleared()
        {
            if (OnCollectionCleared != null)
            {
                OnCollectionCleared();
            }
        }

        event ItemAddedDelegate ICollectionEvents.OnItemAdded
        {
            add { throw new System.NotImplementedException(); }
            remove { throw new System.NotImplementedException(); }
        }

        event ItemRemovedDelegate ICollectionEvents.OnItemRemoved
        {
            add { throw new System.NotImplementedException(); }
            remove { throw new System.NotImplementedException(); }
        }
    }

}
