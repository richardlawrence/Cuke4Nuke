using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WhatToPack.Core;

namespace WhatToPack.Core.Test
{
    [TestFixture]
    public class PackingListItem_Specification
    {
        [Test]
        public void ShouldHaveItemProperty()
        {
            PackingListItem pli = new PackingListItem();
            pli.Item = "Dress Pants";
            Assert.AreEqual("Dress Pants", pli.Item);
        }

        [Test]
        public void ShouldHaveConditionProperty()
        {
            PackingListItem pli = new PackingListItem();
            pli.Condition = "PrecipitationProbability > .5";
            Assert.AreEqual("PrecipitationProbability > .5", pli.Condition);
        }

        [Test]
        public void ShouldTakeItemAndConditionInConstructor()
        {
            PackingListItem pli = new PackingListItem("Umbrella", "PrecipitationProbability > .5");
            Assert.AreEqual("Umbrella", pli.Item);
            Assert.AreEqual("PrecipitationProbability > .5", pli.Condition);
        }
    }
}
