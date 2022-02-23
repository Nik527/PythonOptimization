using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization
{
    internal abstract class NamedWrapper<T> : Wrapper<T> where T : NamedWrapper<T>
    {
        public string Name { get; }

        public NamedWrapper(string name)
        {
            Name = name;
        }

        #region Python object attributes
        public static IntPtr tp_repr(IntPtr objectPtr)
        {
            if (_mapping.TryGetValue(objectPtr, out var self))
            {
                return Runtime.PyString_FromString($"<{self.GetType().Name} name: {self.Name}, ptr: {objectPtr}>");
            }
            return Runtime.PyString_FromString($"<{typeof(T).Name} '{objectPtr}'>");
        }
        #endregion

    }
}
