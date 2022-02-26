using Python.Runtime.Optimization.Wrappers;

namespace Python.Runtime.Optimization
{
    /// <summary>
    /// Implements a Python type that provides access to CLR namespaces. The
    /// type behaves like a Python module, and can contain other sub-modules.
    /// </summary>
    [Serializable]
    internal class Module : ExtensionType
    {
        internal string _name;
        internal IntPtr _dictionary;
        internal BorrowedReference DictRef => new BorrowedReference(_dictionary);

        public Module(string name)
        {
            if (name == string.Empty)
            {
                throw new ArgumentException("Name must not be empty!");
            }
            _name = name;

            // Use the filename from any of the assemblies just so there's something for
            // anything that expects __file__ to be set.
            ;
            var docstring = "Optimization module";
            var assembly = typeof(Module).Assembly;
            var filename = (!assembly.IsDynamic && assembly.Location != null) ? assembly.Location : "unknown";

            _dictionary = Runtime.PyDict_New();
            using var pyname = NewReference.DangerousFromPointer(Runtime.PyString_FromString(_name));
            using var pyfilename = NewReference.DangerousFromPointer(Runtime.PyString_FromString(filename));
            using var pydocstring = NewReference.DangerousFromPointer(Runtime.PyString_FromString(docstring));
            BorrowedReference pycls = TypeManager.GetTypeReference(GetType());
            Runtime.PyDict_SetItem(DictRef, PyIdentifier.__name__, pyname);
            Runtime.PyDict_SetItem(DictRef, PyIdentifier.__file__, pyfilename);
            Runtime.PyDict_SetItem(DictRef, PyIdentifier.__doc__, pydocstring);
            Runtime.PyDict_SetItem(DictRef, PyIdentifier.__class__, pycls);

            Runtime.XIncref(_dictionary);
            SetObjectDict(pyHandle, _dictionary);

            InitializeModuleAttributes();
        }

        /// <summary>
        /// Initialize module level functions and attributes
        /// </summary>
        internal void InitializeModuleAttributes()
        {
            var names = new HashSet<string>();
            foreach (var attribute in new Method[]
            {
                new("Average", CreateAverageWrapper),
                new("Average2", CreateAverageWrapper2),
                new("Empty", CreateEmptyWrapper),
                new("OneMethod", CreateOneMethodWrapper),
                new("OneProperty", CreateOnePropertyWrapper)
            })
            {
                var refCount = attribute.RefCount;
                if (names.Contains(attribute.Name)) throw new Exception($"Dublicate attribute name {attribute.Name}");
                if (Runtime.PyDict_SetItemString(_dictionary, attribute.Name, attribute.pyHandle) != 0) throw new PythonException();
                attribute.DecrRefCount();
                names.Add(attribute.Name);
            }
        }

        #region Methods
        private IntPtr CreateAverageWrapper(IntPtr args) => new AverageWrapper().pyHandle;

        private IntPtr CreateAverageWrapper2(IntPtr args) => new AverageWrapper2().pyHandle;

        private IntPtr CreateEmptyWrapper(IntPtr args) => new EmptyWrapper().pyHandle;

        private IntPtr CreateOneMethodWrapper(IntPtr args) => new OneMethodWrapper().pyHandle;

        private IntPtr CreateOnePropertyWrapper(IntPtr args) => new OnePropertyWrapper().pyHandle;
        #endregion

        #region Python object attributes
        /// <summary>
        /// ModuleObject __repr__ implementation.
        /// </summary>
        public static IntPtr tp_repr(IntPtr ob)
        {
            var self = (Module)GetManagedObject(ob);
            return Runtime.PyString_FromString($"<module '{self._name}'>");
        }

        public new static void tp_dealloc(IntPtr ob)
        {
            var self = (Module)GetManagedObject(ob);
            tp_clear(ob);
            self.Dealloc();
        }

        public static int tp_clear(IntPtr ob)
        {
            var self = (Module)GetManagedObject(ob);
            Runtime.Py_CLEAR(ref self._dictionary);
            ClearObjectDict(ob);
            return 0;
        }
        #endregion
    }
}
