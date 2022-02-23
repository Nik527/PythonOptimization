using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization
{
    internal class Property : NamedWrapper<Property>
    {
        private readonly Func<IntPtr, IntPtr> _getter;
        private readonly Action<IntPtr, IntPtr> _setter;

        public Property(string name, Func<IntPtr, IntPtr> getter, Action<IntPtr, IntPtr> setter = null) : base(name)
        {
            _getter = getter;
            _setter = setter;
        }

        #region Python object attributes
        public static IntPtr tp_descr_get(IntPtr ds, IntPtr objectPtr, IntPtr tp) 
        {
            if (!_mapping.TryGetValue(ds, out var property)) throw new Exception($"Not found property for ds {ds}");
            if(property._getter == null) throw new Exception($"Property {property.Name} can not be reading, ds {ds}");

            var result = property._getter(objectPtr);
            if (result == IntPtr.Zero) throw new Exception($"Property {property.Name} return zero, ds: {ds}, obj ptr: {objectPtr}, tp: {tp}");
            return result;
        }

        public new static int tp_descr_set(IntPtr ds, IntPtr objectPtr, IntPtr val)
        {
            if (!_mapping.TryGetValue(ds, out var property)) throw new Exception($"Not found property for ds {ds}");

            if (property._setter == null) throw new Exception($"Property {property.Name} can not be setting, ds {ds}");

            if (val == IntPtr.Zero)
            {
                Exceptions.SetError(Exceptions.TypeError, "cannot delete field");
                return -1;
            }

            property._setter(objectPtr, val);
            return 0;
        }
        #endregion

    }
}
