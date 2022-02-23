using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization
{
    internal class ObjectMethodAttribute : NamedWrapper<ObjectMethodAttribute>
    {
        private readonly Func<IntPtr, IntPtr, IntPtr> _body;

        public ObjectMethodAttribute(string name, Func<IntPtr, IntPtr, IntPtr> body) : base(name)
        {
            _body = body;
        }

        #region Python object attributes
        public static IntPtr tp_descr_get(IntPtr methodPtr, IntPtr objectPtr, IntPtr tp)
        {

            Logger.Instance.WriteLine($"tp_descr_get obj ptr: {objectPtr}, ds: {methodPtr}, tp: {tp}");
            Logger.Instance.WriteLine($"\tAverageWrapper: {{ {string.Join(", ", Wrappers.AverageWrapper._mapping.Keys)} }}");
            Logger.Instance.WriteLine($"\tTypeMethod: {{ {string.Join(", ", _mapping.Keys)} }}");
            Logger.Instance.WriteLine($"\tMethod: {{ {string.Join(", ", ObjectMethod._mapping.Keys)} }}");
            if (!_mapping.TryGetValue(methodPtr, out var typeMethod)) throw new Exception($"Not found method for ptr {methodPtr}");
            var method = new ObjectMethod(typeMethod.Name, objectPtr, typeMethod._body);
            var dictionary = GetObjectDict(objectPtr);

            var dictionarySize = (int)Runtime.PyDict_Size(dictionary);
            Logger.Instance.WriteLine($"Dictionary {dictionary}, size: {dictionarySize}");
            IntPtr keylist = Runtime.PyDict_Keys(dictionary);
            IntPtr valueList = Runtime.PyDict_Values(dictionary);
            for (int i = 0; i < dictionarySize; ++i)
            {
                var key = Runtime.GetManagedString(Runtime.PyList_GetItem(new BorrowedReference(keylist), i));
                var ptr = Runtime.PyList_GetItem(new BorrowedReference(valueList), i).DangerousGetAddress();
                Logger.Instance.WriteLine($"\tkey {key} => {ptr}");
            }
            Runtime.XDecref(keylist);
            Runtime.XDecref(valueList);


            Logger.Instance.WriteLine($"\tTry set objectPtr: {objectPtr}, dictionary: {dictionary}, item name: {method.Name}, ptr: {method.pyHandle}");
            Logger.Instance.Flush();
            if (Runtime.PyDict_SetItemString(dictionary, method.Name, method.pyHandle) != 0) throw new PythonException();
            return method.pyHandle;
        }

        //public static IntPtr tp_call(IntPtr objectPtr, IntPtr args, IntPtr kw)
        //{
        //    Console.WriteLine($"tp_call obj ptr: {objectPtr}, args: {args}, kw: {kw}");
        //    if (!_mapping.TryGetValue(objectPtr, out var method)) throw new Exception($"Not found method for object {objectPtr}");

        //    var result = method._body(objectPtr, args);
        //    if(result == IntPtr.Zero) throw new Exception($"Method {method.Name} return zero, obj ptr: {objectPtr}, args: {args}, kw: {kw}");
        //    return result;
        //}
        #endregion
    }
}
