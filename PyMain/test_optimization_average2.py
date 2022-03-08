from test_definition import *
import init_net_6 
import optimization

opt_indicator2 = optimization.Average2()

for i in range(test_count):
    opt_indicator2.Add(44.4)