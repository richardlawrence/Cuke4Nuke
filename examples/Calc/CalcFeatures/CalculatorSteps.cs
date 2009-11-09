using System;
using Calc;
using Cuke4Nuke.Framework;
using NUnit.Framework;

namespace CalcFeatures
{
    class CalculatorSteps
    {
        private Calculator _calculator;
        private double _result;

        [Before]
        public void CreateCalculator()
        {
            _calculator = new Calculator();
        }

        [Given(@"^I have entered (\d+) into the calculator$")]
        public void EnterNumber(double n)
        {
            _calculator.Push(n);
        }

        [When(@"^I press divide$")]
        public void Divide()
        {
            _result = _calculator.Divide();
        }

        [When(@"^I press add$")]
        public void Add()
        {
            _result = _calculator.Add();
        }

        [Then(@"^the result should be (.*) on the screen$")]
        public void CheckResult(double expected)
        {
            Assert.That(_result, Is.EqualTo(expected));
        }
    }
}
