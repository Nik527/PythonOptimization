using Optimization;
using Python.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Python.Runtime.Optimization
{
    public class ModuleLoader
    {

        public static unsafe void Load()
        {
            using (var gil = Py.GIL())
            {
                var module = new Module("Optimization");
                Logger.Instance.WriteLine($"module {module._name} ptr: {module.pyHandle}, ref count: {module.RefCount}");
                // create a python module with the same methods as the clr module-like object
                var moduleDef = ModuleDefOffset.AllocModuleDef(module._name);
                var pyModuleHandler = Runtime.PyModule_Create2(moduleDef, 3);
                var pyModuleReference = new BorrowedReference(pyModuleHandler);
                // both dicts are borrowed references
                BorrowedReference mod_dict = Runtime.PyModule_GetDict(pyModuleReference);
                BorrowedReference clr_dict = *Runtime._PyObject_GetDictPtr(module.ObjectReference);

                Runtime.PyDict_Update(mod_dict, clr_dict);
                BorrowedReference dict = Runtime.PyImport_GetModuleDict();
                Runtime.PyDict_SetItemString(dict, module._name, pyModuleReference);
                Runtime.PyDict_SetItemString(dict, module._name.ToLower(), pyModuleReference);

                Logger.Instance.WriteLine($"module {module._name} ptr: {module.pyHandle}, ref count: {module.RefCount}");
            }
        }
    }
}
