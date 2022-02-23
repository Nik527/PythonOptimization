using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Optimization
{
    internal abstract class BaseObject : ManagedType
    {
        protected IntPtr _dictionary;

        public BaseObject()
        {
            tpHandle = TypeManager.GetTypeHandle(GetType());
            pyHandle = Runtime.PyType_GenericAlloc(tpHandle, 0);
            SetupGc();
            _dictionary = Runtime.PyDict_New();
            SetObjectDict(pyHandle, _dictionary);
            Logger.Instance.WriteLine($"Create {GetType().Name} pyHandle: {pyHandle}, dictionary: {_dictionary}, ref count: {RefCount}");
        }

        private void SetupGc()
        {
            GCHandle gc = AllocGCHandle(TrackTypes.Untrack);
            Marshal.WriteIntPtr(pyHandle, ObjectOffset.magic(tpHandle), (IntPtr)gc);
            Runtime.PyObject_GC_UnTrack(pyHandle);
        }

        protected virtual void Dealloc()
        {
            Logger.Instance.WriteLine($"Dealloc object ptr: {pyHandle}, ref count: {RefCount}, type: {GetType().Name}");
            FinalizeObject(this);
        }

        protected void InitializeAttributes<T, TAttribute>(IEnumerable<NamedWrapper<TAttribute>> attributes) where TAttribute : NamedWrapper<TAttribute>
        {
            var names = new HashSet<string>();
            var dictionary = GetTypeDictionary<T>();
            foreach (var attribute in attributes)
            {
                var refCount = attribute.RefCount;
                if (names.Contains(attribute.Name)) throw new Exception($"Dublicate attribute name {attribute.Name}");
                if(Runtime.PyDict_SetItemString(dictionary, attribute.Name, attribute.pyHandle) != 0) throw new PythonException();
                attribute.DecrRefCount();
                names.Add(attribute.Name);
                Logger.Instance.WriteLine($"Add attribute {attribute.Name} to type {typeof(T).Name}, ptr: {attribute.pyHandle}, ref count: {attribute.RefCount} (before {refCount})");
            }
        }

        //internal static void InitializeProperties<T>(IEnumerable<Property> properties)
        //{
        //    var names = new HashSet<string>();
        //    var dictionary = GetTypeDictionary<T>();
        //    foreach (var method in methods)
        //    {
        //        if (names.Contains(method.Name)) throw new Exception($"Dublicate property name {method.Name}");
        //    }

        //}

        private static IntPtr GetTypeDictionary<T>()
        {
            var type = typeof(T);
            var @class = ClassManager.GetClass(type);
            var typeHandle = TypeManager.GetTypeHandle(@class, type);
            return Marshal.ReadIntPtr(typeHandle, TypeOffset.tp_dict);
        }

        /// <summary>
        /// Common finalization code to support custom tp_deallocs.
        /// </summary>
        public static void FinalizeObject(ManagedType self)
        {
            ClearObjectDict(self.pyHandle);
            Runtime.PyObject_GC_Del(self.pyHandle);
            // Not necessary for decref of `tpHandle`.
            self.FreeGCHandle();
        }

        /// <summary>
        /// Type __setattr__ implementation.
        /// </summary>
        public static int tp_setattro(IntPtr ob, IntPtr key, IntPtr val)
        {
            var message = "type does not support setting attributes";
            if (val == IntPtr.Zero)
            {
                message = "readonly attribute";
            }
            Exceptions.SetError(Exceptions.TypeError, message);
            return -1;
        }

        /// <summary>
        /// Default __set__ implementation - this prevents descriptor instances
        /// being silently replaced in a type __dict__ by default __setattr__.
        /// </summary>
        public static int tp_descr_set(IntPtr ds, IntPtr ob, IntPtr val)
        {
            Exceptions.SetError(Exceptions.AttributeError, "attribute is read-only");
            return -1;
        }

        ///// <summary>
        ///// Default dealloc implementation.
        ///// </summary>
        //public static void tp_dealloc(IntPtr ob)
        //{
        //    // Clean up a Python instance of this extension type. This
        //    // frees the allocated Python object and decrefs the type.
        //    var self = (ExtensionType)GetManagedObject(ob);
        //    self.Dealloc();
        //}
    }
}
