using Indicators;

namespace Python.Runtime.Optimization.Wrappers
{
    using static Runtime;
    internal class AverageWrapper2 : ObjectWrapper<AverageWrapper2>
    {
        private readonly Average _average = new();
        public AverageWrapper2()
        {
            InitializeAttributes(_dictionary,
                new Method[]
                {
                    new(nameof(Add), Add)
                });
        }

        protected override IEnumerable<Property> CreateProperties()
        {
            return new Property[]
            {
                new(nameof(Value), Value)
            };
        }

        private IntPtr Add(IntPtr args)
        {
            if (!PyTuple_Check(args)) throw new ArgumentException($"Method {nameof(Add)} got invalid type argument ptr: {args}");
            var argsCount = PyTuple_Size(args);
            if(argsCount != 1) throw new ArgumentException($"Method {nameof(Add)} got invalid type argument count {argsCount}, ptr: {args}");
            var valuePtr = PyTuple_GetItem(args, 0);
            if (!PyFloat_Check(valuePtr)) throw new ArgumentException($"Method {nameof(Add)} got invalid type (float) argument 0 ptr: {valuePtr}");
            var value = PyFloat_AsDouble(valuePtr);
            _average.Add(value);
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
