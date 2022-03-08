from test_definition import *
import init_net_6 
import optimization

get_property_indicator = optimization.OneProperty()
get_property_indicator.Value = 55.5

for i in range(test_count):
    value = get_property_indicator.Value