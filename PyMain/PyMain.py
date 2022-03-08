import init_net_6 
import random, time
import optimization

#-----------------------------------------------------------------------------------
#value = random.uniform(0, 100)
#start_time_random = time.time()
#for i in range(test_count):
#    value = random.uniform(0, 100)
#end_time_random = time.time()
#print(f"random time: {end_time_random-start_time_random}s, value: {value}")

#-----------------------------------------------------------------------------------
start_time_py_indicator = time.time()
from test_py_average import py_indicator
end_time_py_indicator = time.time()
print(f"py indicator time: {end_time_py_indicator-start_time_py_indicator}s")

#-----------------------------------------------------------------------------------
start_time_net_indicator = time.time()
from test_net_average import net_indicator
end_time_net_indicator = time.time()
print(f"net indicator time: {end_time_net_indicator-start_time_net_indicator}s")

#-----------------------------------------------------------------------------------
start_time_optimization_indicator = time.time()
from test_optimization_average import opt_indicator
end_time_optimization_indicator = time.time()
print(f"optimization indicator time: {end_time_optimization_indicator-start_time_optimization_indicator}s")

#-----------------------------------------------------------------------------------
start_time_optimization_indicator2 = time.time()
from test_optimization_average2 import opt_indicator2
end_time_optimization_indicator2 = time.time()
print(f"optimization indicator 2 time: {end_time_optimization_indicator2-start_time_optimization_indicator2}s")

#-----------------------------------------------------------------------------------
start_time_get_propety = time.time()
from test_optimization_get_property import get_property_indicator
end_time_get_propety = time.time()
print(f"get propety time: {end_time_get_propety-start_time_get_propety}s")

#-----------------------------------------------------------------------------------
start_time_set_propety = time.time()
from test_optimization_set_property import set_property_indicator
end_time_set_propety = time.time()
print(f"set propety time: {end_time_set_propety-start_time_set_propety}s")

#-----------------------------------------------------------------------------------
start_time_create = time.time()
import test_create_empty
end_time_create = time.time()
print(f"create Empty object time: {end_time_create-start_time_create}s")

#-----------------------------------------------------------------------------------
start_time_create = time.time()
import test_create_one_property
end_time_create = time.time()
print(f"create OneProperty object time: {end_time_create-start_time_create}s")

#-----------------------------------------------------------------------------------
start_time_create = time.time()
import test_create_one_method
end_time_create = time.time()
print(f"create OneMethod object time: {end_time_create-start_time_create}s")

#-----------------------------------------------------------------------------------
start_time_create = time.time()
import test_create_average
end_time_create = time.time()
print(f"create Average object time: {end_time_create-start_time_create}s")

#-----------------------------------------------------------------------------------
start_time_create = time.time()
import test_create_average2
end_time_create = time.time()
print(f"create Average2 object time: {end_time_create-start_time_create}s")
