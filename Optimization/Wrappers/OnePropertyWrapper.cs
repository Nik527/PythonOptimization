using Indicators;

namespace Python.Runtime.Optimization.Wrappers
{
    using static Runtime;
    internal class OnePropertyWrapper : ObjectWrapper<OnePropertyWrapper>
    {
        private readonly Average _average = new();
        public OnePropertyWrapper()
        {
        }

        protected override IEnumerable<Property> CreateProperties()
        {
            return new Property[]
            {
                new(nameof(Value), Value, SetValue)
            };
        }

        private static IntPtr Value(IntPtr objectPtr)
        {
            var obj = GetObject(objectPtr);
            return PyFloat_FromDouble(obj._average.Value);
        }
        private static void SetValue(IntPtr objectPtr, IntPtr valuePtr)
        {
            if (!PyFloat_Check(valuePtr)) throw new ArgumentException($"Property {nameof(Value)} got invalid type (float) value ptr: {valuePtr}");
            var value = PyFloat_AsDouble(valuePtr);
            var obj = GetObject(objectPtr);
            obj._average.Add(value);
        }
    }
}
