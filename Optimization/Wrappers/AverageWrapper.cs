using Indicators;

namespace Python.Runtime.Optimization.Wrappers
{
    using static Runtime;
    internal class AverageWrapper : ObjectWrapper<AverageWrapper>
    {
        private readonly Average _average = new();
        public AverageWrapper()
        {
        }

        protected override IEnumerable<Property> CreateProperties()
        {
            return new Property[]
            {
                new(nameof(Value), Value)
            };
        }

        protected override IEnumerable<ClassMethod> CreateMethods()
        {
            return new ClassMethod[]
            {
                new(nameof(Add), Add)
            };
        }

        private static IntPtr Add(IntPtr objectPtr, IntPtr args)
        {
            if (!PyTuple_Check(args)) throw new ArgumentException($"Method {nameof(Add)} got invalid type argument ptr: {args}");
            var argsCount = PyTuple_Size(args);
            if(argsCount != 1) throw new ArgumentException($"Method {nameof(Add)} got invalid type argument count {argsCount}, ptr: {args}");
            var valuePtr = PyTuple_GetItem(args, 0);
            if (!PyFloat_Check(valuePtr)) throw new ArgumentException($"Method {nameof(Add)} got invalid type (float) argument 0 ptr: {valuePtr}");
            var value = PyFloat_AsDouble(valuePtr);
            var obj = GetObject(objectPtr);
            obj._average.Add(value);
            XIncref(PyNone);
            return PyNone;
        }

        private static IntPtr Value(IntPtr objectPtr)
        {
            var obj = GetObject(objectPtr);
            return PyFloat_FromDouble(obj._average.Value);
        }
    }
}
