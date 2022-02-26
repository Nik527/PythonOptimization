using System.Collections.Concurrent;

namespace Python.Runtime.Optimization
{
    internal abstract class Wrapper<T> : BaseObject where T : Wrapper<T>
    {
        internal static readonly ConcurrentDictionary<IntPtr, T> _mapping = new();

        protected Wrapper()
        {
            if (this is not T t) throw new Exception($"Error generic type {typeof(T).FullName}, must be {GetType().FullName}");
            if (!_mapping.TryAdd(pyHandle, t)) throw new Exception($"Error create (add to dictionary) object in type {GetType().FullName}");
        }

        protected static T GetObject(IntPtr objectPtr)
        {
            if (_mapping.TryGetValue(objectPtr, out var @object)) return @object;
            throw new Exception($"Object {typeof(T).Name} with ptr {objectPtr} not found");
        }

        #region Python object attributes
        public static void tp_dealloc(IntPtr objectPtr)
        {
            if (!_mapping.TryRemove(objectPtr, out var self)) throw new Exception($"Object {typeof(T).Name} with ptr {objectPtr} not found");
            self.Dealloc();
        }
        #endregion
    }
}
