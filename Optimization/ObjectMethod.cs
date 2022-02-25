﻿using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization
{
    internal class ObjectMethod : NamedWrapper<ObjectMethod>
    {
        private readonly IntPtr _objectPtr;
        private readonly Func<IntPtr, IntPtr, IntPtr> _body;

        public ObjectMethod(string name, IntPtr objectPtr, Func<IntPtr, IntPtr, IntPtr> body) : base(name)
        {
            _objectPtr = objectPtr;
            _body = body;
        }

        #region Python object attributes
        public static IntPtr tp_call(IntPtr methodPtr, IntPtr args, IntPtr kw)
        {
            Logger.Instance.WriteLine($"tp_call obj ptr: {methodPtr}, args: {args}, kw: {kw}");
            Logger.Instance.Flush();
            if (!_mapping.TryGetValue(methodPtr, out var method)) throw new Exception($"Not found method for object {methodPtr}");

            var result = method._body(method._objectPtr, args);
            if(result == IntPtr.Zero) throw new Exception($"Method {method.Name} return zero, obj ptr: {methodPtr}, args: {args}, kw: {kw}");
            return result;
        }
        #endregion
    }
}
