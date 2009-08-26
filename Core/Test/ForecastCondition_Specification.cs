using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace WhatToPack.Core.Test
{
    [TestFixture]
    public class ForecastCondition_Specification
    {
        [Test]
        public void ShouldHavePropertyProperty()
        {
            ForecastCondition fc = new ForecastCondition();
            fc.Property = "PrecipitationProbability";
            Assert.AreEqual("PrecipitationProbability", fc.Property);
        }

        [Test]
        public void ShouldHaveAnOperatorProperty()
        {
            ForecastCondition fc = new ForecastCondition();
            fc.Operator = "<";
            Assert.AreEqual("<", fc.Operator);
        }

        [Test]
        public void ShouldHaveAValueProperty()
        {
            ForecastCondition fc = new ForecastCondition();
            fc.Value = 50;
            Assert.AreEqual(50, fc.Value);
        }

        [Test]
        public void CanParseConditionStrings()
        {
            ForecastCondition fc = ForecastCondition.Parse("PrecipitationProbability > 50");
            Assert.AreEqual("PrecipitationProbability", fc.Property);
            Assert.AreEqual(">", fc.Operator);
            Assert.AreEqual(50, fc.Value);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void CanParseConditionStrings_BadInput()
        {
            ForecastCondition fc = ForecastCondition.Parse("Likely to rain");
        }

        [Test]
        public void CanTestConditionWithSuppliedPropertyValue_ExpectingTrue()
        {
            ForecastCondition fc = ForecastCondition.Parse("PrecipitationProbability > 50");
            Assert.IsTrue(fc.Test(51));
        }

        [Test]
        public void CanTestConditionWithSuppliedPropertyValue_ExpectingFalse()
        {
            ForecastCondition fc = ForecastCondition.Parse("PrecipitationProbability > 50");
            Assert.IsFalse(fc.Test(40));
        }

        [Test]
        public void CanTestConditionWithSuppliedForecast()
        {
            Forecast f = new Forecast();
            f.PrecipitationProbability = 51;
            ForecastCondition fc = ForecastCondition.Parse("PrecipitationProbability > 50");
            Assert.IsTrue(fc.Test(f));
        }
    }
}
