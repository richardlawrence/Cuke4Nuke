using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using WhatToPack.Core;

namespace WhatToPack.Core.Test
{
    [TestFixture]
    public class PackingList_Specification
    {
        [Test]
        public void ShouldBeAListOfPackingListItems()
        {
            PackingList pl = new PackingList();
            pl.Add(new PackingListItem("Dress Pants", ""));
            pl.Add(new PackingListItem("Dress Shirts", ""));
            pl.Add(new PackingListItem("Umbrella", "PrecipitationProbability > 50"));
            Assert.AreEqual(3, pl.Count);
        }

        [Test]
        public void CanFilterByAForecast()
        {
            PackingList pl = new PackingList();
            pl.Add(new PackingListItem("Dress Pants", ""));
            pl.Add(new PackingListItem("Dress Shirts", ""));
            pl.Add(new PackingListItem("Umbrella", "PrecipitationProbability > 50"));
            Forecast f = new Forecast();
            f.PrecipitationProbability = 10;
            PackingList filteredList = pl.Filter(f);
            Assert.AreEqual(2, filteredList.Count);
        }
    }
}
