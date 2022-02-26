namespace Python.Runtime.Optimization.Wrappers
{
    using static Runtime;
    internal class OneMethodWrapper : ObjectWrapper<OneMethodWrapper>
    {
        public OneMethodWrapper()
        {
            InitializeAttributes(_dictionary,
                new Method[]
                {
                    new(nameof(Add), Add)
                });
        }

        private IntPtr Add(IntPtr args)
        {
            XIncref(PyNone);
            return PyNone;
        }
    }
}
