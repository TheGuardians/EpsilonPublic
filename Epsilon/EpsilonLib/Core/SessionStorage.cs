using EpsilonLib.Editors;
using System.Collections.Generic;
using System.ComponentModel.Composition;

namespace EpsilonLib.Core
{
    public interface ISessionStore
    {
        bool TryGetItem<T>(object key, out T value, T defaultValue = default);
        void StoreItem<T>(object key, T value);
    }

    [Export(typeof(ISessionStore))]
    [PartCreationPolicy(CreationPolicy.Shared)]
    class SessionStorage : ISessionStore    
    {
        private readonly Dictionary<object, object> _store = new Dictionary<object, object>();

        public bool TryGetItem<T>(object key, out T value, T defaultValue = default)
        {
            if (_store.TryGetValue(key, out var o) && o is T castedValue)
            {
                value = castedValue;
                return true;
            }

            value = defaultValue;
            return false;
        }

        public void StoreItem<T>(object key, T value)
        {
            _store[key] = value;
        }
    }
}
