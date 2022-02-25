from clr_loader import get_coreclr
from pythonnet import set_runtime

runtime = get_coreclr("runtimeconfig.json")
set_runtime(runtime)

import clr
clr.AddReference("Fasterflect")
print("initilize clr")



from Python.Runtime import Logger
Logger.Instance.IsEnabled = True



#test
import optimization

#-----------------------------------------------------------------------------------
optIndicator = optimization.Average()
optIndicator.Add(100.0)
optIndicator = None
#-----------------------------------------------------------------------------------
optIndicator2 = optimization.Average2()
optIndicator2 = None
#-----------------------------------------------------------------------------------
Logger.Instance.Flush()