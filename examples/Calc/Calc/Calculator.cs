using System;
using System.Collections.Generic;

namespace Calc
{
    public class Calculator
    {
        private List<double> args = new List<double>();

        public void Push(double n)
        {
            args.Add(n);
        }

        public double Add()
        {
            double result = 0;
            foreach (double n in args)
            {
                result += n;
            }
            return result;
        }

        public double Divide()
        {
            double result = args[0];
            foreach (int n in args.GetRange(1, args.Count-1))
            {
                result /= n;
            }
            return result;
        }
    }
}
