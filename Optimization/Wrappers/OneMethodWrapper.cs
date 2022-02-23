using Indicators;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization.Wrappers
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
