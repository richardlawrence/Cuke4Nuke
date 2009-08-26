using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace WhatToPack.Core.Test
{
    [TestFixture]
    public class Forecast_Specification
    {
        [Test]
        public void ShouldHavePrecipitationProbabilityProperty()
        {
            Forecast f = new Forecast();
            f.PrecipitationProbability = 40;
            Assert.AreEqual(40, f.PrecipitationProbability);
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ShouldRequirePrecipitationProbabilityGreaterThanOrEqualToZero()
        {
            Forecast f = new Forecast();
            f.PrecipitationProbability = -1;
        }

        [Test]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ShouldRequirePrecipitationProbabilityLessThanOrEqualTo100()
        {
            Forecast f = new Forecast();
            f.PrecipitationProbability = 101;
        }
    }
}
