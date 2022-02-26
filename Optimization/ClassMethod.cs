using System.Collections.Concurrent;

namespace Python.Runtime.Optimization
{
    internal class ClassMethod : NamedWrapper<ClassMethod>
    {
        private static readonly ConcurrentDictionary<IntPtr, ConcurrentDictionary<IntPtr, ObjectMethod>> _cache = new();
        private readonly Func<IntPtr, IntPtr, IntPtr> _body;

        public ClassMethod(string name, Func<IntPtr, IntPtr, IntPtr> body) : base(name)
        {
            _body = body;
        }

        internal static void DeallocMethods(IntPtr objectPtr)
        {
            if (!_cache.TryRemove(objectPtr, out var objectMethods)) return;
            foreach (var method in objectMethods.Values)
            {
                method.DecrRefCount();
            }
        }

        #region Python object attributes
        public static IntPtr tp_descr_get(IntPtr methodPtr, IntPtr objectPtr, IntPtr tp)
        {

            if(!_cache.TryGetValue(objectPtr, out var objectMethods))
            {
                objectMethods = new ConcurrentDictionary<IntPtr, ObjectMethod>();
                if (!_cache.TryAdd(objectPtr, objectMethods))
                {
                    if (!_cache.TryGetValue(objectPtr, out objectMethods)) throw new Exception($"Error get object ptr: {objectPtr} from cache");
                }
            }
            if (!objectMethods.TryGetValue(methodPtr, out var method))
            {
                if (!_mapping.TryGetValue(methodPtr, out var objectMethodAttribute)) throw new Exception($"Not found method for ptr {methodPtr}");
                method = new ObjectMethod(objectMethodAttribute.Name, objectPtr, objectMethodAttribute._body);
                if(!objectMethods.TryAdd(methodPtr, method))
                {
                    method.DecrRefCount();
                    if (!objectMethods.TryGetValue(methodPtr, out method)) throw new Exception($"Error get method ptr: {methodPtr} for object ptr: {objectPtr} from cache");
                }
            }

            method.IncrRefCount();
            return method.pyHandle;
        }
        #endregion
    }
}
