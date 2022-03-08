from test_definition import *
import init_net_6 
import clr
clr.AddReference("CSharpMain")
from Indicators import Average

net_indicator = Average()

for i in range(test_count):
    net_indicator.Add(22.2)