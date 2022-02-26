using System.Runtime.InteropServices;

namespace Python.Runtime.Optimization
{
    using static Runtime;
    internal abstract class ObjectWrapper<T> : Wrapper<T> where T : ObjectWrapper<T>
    {
        private static bool _isInitialized = false;
        private static readonly HashSet<string> _names = new();
        protected ObjectWrapper()
        {
            if (!_isInitialized)
            {
                _isInitialized = true;
                var dictionary = GetTypeDictionary();
                InitializeAttributes(dictionary, CreateProperties(), true);
                InitializeAttributes(dictionary, CreateMethods(), true);
            }
        }

        protected virtual IEnumerable<Property> CreateProperties() => Array.Empty<Property>();

        protected virtual IEnumerable<ClassMethod> CreateMethods() => Array.Empty<ClassMethod>();

        protected static void InitializeAttributes<TAttribute>(IntPtr dictionary, IEnumerable<NamedWrapper<TAttribute>> attributes, bool addNameToGlobalSet = false) where TAttribute : NamedWrapper<TAttribute>
        {
            foreach (var attribute in attributes)
            {
                var refCount = attribute.RefCount;
                if (_names.Contains(attribute.Name)) throw new Exception($"Dublicate attribute name {attribute.Name}");
                if (PyDict_SetItemString(dictionary, attribute.Name, attribute.pyHandle) != 0) throw new PythonException();
                attribute.DecrRefCount();
                if(addNameToGlobalSet) _names.Add(attribute.Name);
            }
        }

        private IntPtr GetTypeDictionary()
        {
            var type = GetType();
            var @class = ClassManager.GetClass(type);
            var typeHandle = TypeManager.GetTypeHandle(@class, type);
            return Marshal.ReadIntPtr(typeHandle, TypeOffset.tp_dict);
        }

        protected override void Dealloc()
        {
            ClassMethod.DeallocMethods(pyHandle);
            base.Dealloc();
        }
    }
}