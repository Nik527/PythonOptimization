namespace Python.Runtime.Optimization
{
    internal class Method : NamedWrapper<Method>
    {
        private readonly Func<IntPtr, IntPtr> _body;

        public Method(string name, Func<IntPtr, IntPtr> body) : base(name)
        {
            _body = body;
        }

        #region Python object attributes
        public static IntPtr tp_call(IntPtr methodPtr, IntPtr args, IntPtr kw)
        {
            if (!_mapping.TryGetValue(methodPtr, out var method)) throw new Exception($"Not found method for object {methodPtr}");

            var result = method._body(args);
            if(result == IntPtr.Zero) throw new Exception($"Method {method.Name} return zero, obj ptr: {methodPtr}, args: {args}, kw: {kw}");
            return result;
        }
        #endregion
    }
}
