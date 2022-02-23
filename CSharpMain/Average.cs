using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Indicators
{
    public class Average
    {
        private double _sum = 0;
        private long _count = 0;
        public double Value => _count == 0 ? 0 : _sum / _count;

        public void Add(double value)
        {
            _sum += value;
            _count++;
        }
    }
}
