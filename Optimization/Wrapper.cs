using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Optimization
{
    internal abstract class Wrapper<T> : BaseWrapper<T> where T : Wrapper<T>
    {
        private static bool _isInitialized = false;
        protected Wrapper()
        {
            if(!_isInitialized)
            {
                _isInitialized = true;
                InitializeAttributes<T, Property>(CreateProperties());
                InitializeAttributes<T, TypeMethod>(CreateMethods());
            }
        }

        protected abstract IEnumerable<Property> CreateProperties();

        protected abstract IEnumerable<TypeMethod> CreateMethods();
    }
}
