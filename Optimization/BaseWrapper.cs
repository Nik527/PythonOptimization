using Python.Runtime;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization
{
    internal abstract class BaseWrapper<T> : BaseObject where T : BaseWrapper<T>
    {
        internal static readonly ConcurrentDictionary<IntPtr, T> _mapping = new();

        protected BaseWrapper()
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
            Logger.Instance.WriteLine($"Try dealloc object ptr: {objectPtr}");
            if (!_mapping.TryRemove(objectPtr, out var self)) throw new Exception($"Object {typeof(T).Name} with ptr {objectPtr} not found");
            self.Dealloc();
        }
        #endregion
    }
}
