using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using Python.Runtime;
using Optimization.Wrappers;

namespace Optimization
{
    /// <summary>
    /// Implements a Python type that provides access to CLR namespaces. The
    /// type behaves like a Python module, and can contain other sub-modules.
    /// </summary>
    [Serializable]
    internal class Module : ExtensionType
    {
        internal string _name;
        internal IntPtr _dict;
        internal BorrowedReference DictRef => new BorrowedReference(_dict);

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

            _dict = Runtime.PyDict_New();
            using var pyname = NewReference.DangerousFromPointer(Runtime.PyString_FromString(_name));
            using var pyfilename = NewReference.DangerousFromPointer(Runtime.PyString_FromString(filename));
            using var pydocstring = NewReference.DangerousFromPointer(Runtime.PyString_FromString(docstring));
            BorrowedReference pycls = TypeManager.GetTypeReference(GetType());
            Runtime.PyDict_SetItem(DictRef, PyIdentifier.__name__, pyname);
            Runtime.PyDict_SetItem(DictRef, PyIdentifier.__file__, pyfilename);
            Runtime.PyDict_SetItem(DictRef, PyIdentifier.__doc__, pydocstring);
            Runtime.PyDict_SetItem(DictRef, PyIdentifier.__class__, pycls);

            Runtime.XIncref(_dict);
            SetObjectDict(pyHandle, _dict);

            InitializeModuleAttributes();
        }

        ///// <summary>
        ///// Stores an attribute in the instance dict for future lookups.
        ///// </summary>
        //private void StoreAttribute(string name, ManagedType ob)
        //{
        //    if (Runtime.PyDict_SetItemString(_dict, name, ob.pyHandle) != 0)
        //    {
        //        throw new PythonException();
        //    }
        //    ob.IncrRefCount();
        //}

        /// <summary>
        /// Initialize module level functions and attributes
        /// </summary>
        internal void InitializeModuleAttributes()
        {
            var names = new HashSet<string>();
            foreach (var method in new Method[]
            {
                new("Average", pyHandle, CreateAverageWrapper)
            })
            {
                var refCount = method.RefCount;
                if (names.Contains(method.Name)) throw new Exception($"Dublicate attribute name {method.Name}");
                if (Runtime.PyDict_SetItemString(_dict, method.Name, method.pyHandle) != 0) throw new PythonException();
                method.DecrRefCount();
                names.Add(method.Name);
            }
        }

        private IntPtr CreateAverageWrapper(IntPtr objectPtr, IntPtr args)
        {
            var handle = new AverageWrapper().pyHandle;
            Logger.Instance.WriteLine($"CreateAverageWrapper objectPtr: {objectPtr}, args: {args}");
            return handle;
        }

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
            Runtime.Py_CLEAR(ref self._dict);
            ClearObjectDict(ob);
            return 0;
        }
    }
}