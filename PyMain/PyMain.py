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

#-----------------------------------------------------------------------------------
optIndicator2 = optimization.Average2()

start_time_optimization2 = time.time()
for i in range(test_count):
    optIndicator2.Add(random.uniform(0, 100))
end_time_optimization2 = time.time()
print(f"optimization indicator2 time: {end_time_optimization2-start_time_optimization2}s,  value: {optIndicator2.Value}")

#-----------------------------------------------------------------------------------
start_time_create = time.time()
for i in range(test_count):
    indicator = optimization.Empty()
end_time_create = time.time()
print(f"create Empty object time: {end_time_create-start_time_create}s")

#-----------------------------------------------------------------------------------
start_time_create = time.time()
for i in range(test_count):
    indicator = optimization.OneProperty()
end_time_create = time.time()
print(f"create OneProperty object time: {end_time_create-start_time_create}s")

#-----------------------------------------------------------------------------------
start_time_create = time.time()
for i in range(test_count):
    indicator = optimization.OneMethod()
end_time_create = time.time()
print(f"create OneMethod object time: {end_time_create-start_time_create}s")

#-----------------------------------------------------------------------------------
start_time_create = time.time()
for i in range(test_count):
    indicator = optimization.Average()
end_time_create = time.time()
print(f"create Average object time: {end_time_create-start_time_create}s")

#-----------------------------------------------------------------------------------
start_time_create = time.time()
for i in range(test_count):
    indicator = optimization.Average2()
end_time_create = time.time()
print(f"create Average2 object time: {end_time_create-start_time_create}s")



#Logger.Instance.Flush()