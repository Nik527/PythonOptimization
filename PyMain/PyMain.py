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
test_count = 1000000
import random, time
import optimization

clr.AddReference("CSharpMain")
from Indicators import Average

#-----------------------------------------------------------------------------------
value = random.uniform(0, 100)
start_time_random = time.time()
for i in range(test_count):
    value = random.uniform(0, 100)
end_time_random = time.time()
print(f"random time: {end_time_random-start_time_random}s, value: {value}")

#-----------------------------------------------------------------------------------
netIndicator = Average()
netIndicator.Add(random.uniform(0, 100))

start_time_indicator = time.time()
for i in range(test_count):
    netIndicator.Add(random.uniform(0, 100))
end_time_indicator = time.time()
print(f"net indicator time: {end_time_indicator-start_time_indicator}s, value: {netIndicator.Value}")

#-----------------------------------------------------------------------------------
optIndicator = optimization.Average()

start_time_optimization = time.time()
for i in range(test_count):
    optIndicator.Add(random.uniform(0, 100))
end_time_optimization = time.time()
print(f"optimization indicator time: {end_time_optimization-start_time_optimization}s,  value: {optIndicator.Value}")


#print(f"optIndicator __dir__(): {optIndicator.__dir__()}\n")

#print()
#print(f"optIndicator id: {id(optIndicator)}, dir: {dir(optIndicator)}\n")
#print(f"optIndicator.Add id: {id(optIndicator.Add)}, dir: {dir(optIndicator.Add)}\n")
#print(f"optIndicator.Value id: {id(optIndicator.Value)}, dir: {dir(optIndicator.Value)}\n")
#Logger.Instance.Flush()
#optIndicator.Add(random.uniform(0, 100))
#print(f"optIndicator.Add id: {id(optIndicator.Add)}, dir: {dir(optIndicator.Add)}\n")
#optIndicator.Add(random.uniform(0, 100))

#-----------------------------------------------------------------------------------
#import sys
#print(sys.path)
##m = ModuleLoader()
##print(m)
##m.test()
##print("----------------------------------")
##print(sys.modules.keys())
#print("----------------------------------")
#import optimization

#print(f"test: {optimization.test()}")



#Logger.Instance.Flush()