from clr_loader import get_coreclr
from pythonnet import set_runtime

runtime = get_coreclr("runtimeconfig.json")
set_runtime(runtime)

import clr
clr.AddReference("Fasterflect")
print("initilize clr")



#from Python.Runtime import Logger
#Logger.Instance.IsEnabled = True



#test
import optimization

#-----------------------------------------------------------------------------------
testObject = optimization.OneProperty()
testObject.Value = 100.0
testObject.Value = 50.1
print(f"testObject.Value: {testObject.Value}")
#-----------------------------------------------------------------------------------
#Logger.Instance.Flush()