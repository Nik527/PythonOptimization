from test_definition import *
import init_net_6 
import optimization

opt_indicator = optimization.Average()

for i in range(test_count):
    opt_indicator.Add(33.3)