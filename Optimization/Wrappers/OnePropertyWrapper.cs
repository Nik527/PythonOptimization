namespace Python.Runtime.Optimization.Wrappers
{
    using static Runtime;
    internal class OnePropertyWrapper : ObjectWrapper<OnePropertyWrapper>
    {
        public OnePropertyWrapper()
        {
        }

        protected override IEnumerable<Property> CreateProperties()
        {
            return new Property[]
            {
                new(nameof(Value), Value)
            };
        }

        private static IntPtr Value(IntPtr objectPtr)
        {
            XIncref(objectPtr);
            return objectPtr;
        }
    }
}
