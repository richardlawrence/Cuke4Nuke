using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Cuke4Nuke.Framework;

namespace Cuke4Nuke.Specifications.Core
{
    public class Table_Specification
    {
        [Test]
        public void HashesShouldTreatFirstRowAsHeaders()
        {
            Table table = new Table();
            table.Data.Add(new List<string> { "item", "count" });
            table.Data.Add(new List<string> { "cucumbers", "3" });
            table.Data.Add(new List<string> { "bananas", "5" });
            table.Data.Add(new List<string> { "tomatoes", "2" });

            var hashes = table.Hashes();
            Assert.That(hashes.Count, Is.EqualTo(3));
            Assert.That(hashes[0].Keys, Has.Member("item"));
            Assert.That(hashes[0].Keys, Has.Member("count"));
            Assert.That(hashes[1]["item"], Is.EqualTo("bananas"));
        }
    }
}
