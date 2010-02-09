using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kalkulator
{
    public class Kalkulator
    {
        private int _sum;

        public void LeggSammen(int tall, int annetTall)
        {
            _sum = tall + annetTall;
        }

        public int Sum()
        {
            return _sum;
        }
    }
}
