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

        [Test]
        public void AssertSameAsShouldThrow()
        {
            Table actualTable = new Table();
            Table expectedTable = new Table();
            Assert.Throws<TableAssertionException>(delegate()
            {
                actualTable.AssertSameAs(expectedTable);
            });
        }

        [Test]
        public void AssertSameAsShouldProvideTablesInException()
        {
            Table actualTable = new Table();
            Table expectedTable = new Table();
            try
            {
                actualTable.AssertSameAs(expectedTable);
            }
            catch (TableAssertionException ex)
            {
                Assert.AreSame(actualTable, ex.Actual);
                Assert.AreSame(expectedTable, ex.Expected);
            }
        }

        [Test]
        public void IncludesShouldReturnTrueWithOneColumMatching()
        {
            Table expTable = new Table();
            Table actTable = new Table();

            expTable.Data.Add(new List<string>(new []{"Provider"}));
            expTable.Data.Add(new List<string>(new[]{"Nurse (Ann)"}));

            actTable.Data.Add(new List<string>(new[] { "Provider" }));
            actTable.Data.Add(new List<string>(new[] { "Nurse (Ann)" }));
            actTable.Data.Add(new List<string>(new[] { "Doctor (Zeke)" }));

            Assert.That(actTable.Includes(expTable));
        }

        [Test]
        public void IncludesShouldReturnFalseWithOneColumnNotMatching()
        {
            Table expTable = new Table();
            Table actTable = new Table();

            expTable.Data.Add(new List<string>(new[] { "Provider" }));
            expTable.Data.Add(new List<string>(new[] { "Nurse (Sue)" }));

            actTable.Data.Add(new List<string>(new[] { "Provider" }));
            actTable.Data.Add(new List<string>(new[] { "Nurse (Ann)" }));
            actTable.Data.Add(new List<string>(new[] { "Doctor (Zeke)" }));

            Assert.That(actTable.Includes(expTable), Is.False);
        }

        [Test]
        public void IncludesShouldReturnTrueWithTwoColumnMatching()
        {
            Table expTable = new Table();
            Table actTable = new Table();

            expTable.Data.Add(new List<string>(new[] { "Provider", "Date" }));
            expTable.Data.Add(new List<string>(new[] { "Nurse (Ann)", "01/15/2010" }));

            actTable.Data.Add(new List<string>(new[] { "Provider", "Date" }));
            actTable.Data.Add(new List<string>(new[] { "Nurse (Ann)", "01/15/2010" }));
            actTable.Data.Add(new List<string>(new[] { "Doctor (Zeke)", "01/15/2010" }));

            Assert.That(actTable.Includes(expTable));
        }

        [Test]
        public void IncludesShouldReturnFalseWithTwoColumnsWithMismatchInFirstColumn()
        {
            Table expTable = new Table();
            Table actTable = new Table();

            expTable.Data.Add(new List<string>(new[] { "Provider", "Date" }));
            expTable.Data.Add(new List<string>(new[] { "Nurse (Sue)", "01/15/2010" }));

            actTable.Data.Add(new List<string>(new[] { "Provider", "Date" }));
            actTable.Data.Add(new List<string>(new[] { "Nurse (Ann)", "01/15/2010" }));
            actTable.Data.Add(new List<string>(new[] { "Doctor (Zeke)", "01/15/2010" }));

            Assert.That(actTable.Includes(expTable), Is.False);
        }

        [Test]
        public void IncludesShouldReturnFalseWithTwoColumnsWithMismatchInSecondColumn()
        {
            Table expTable = new Table();
            Table actTable = new Table();

            expTable.Data.Add(new List<string>(new[] { "Provider", "Date" }));
            expTable.Data.Add(new List<string>(new[] { "Nurse (Ann)", "01/15/2015" }));

            actTable.Data.Add(new List<string>(new[] { "Provider", "Date" }));
            actTable.Data.Add(new List<string>(new[] { "Nurse (Ann)", "01/15/2010" }));
            actTable.Data.Add(new List<string>(new[] { "Doctor (Zeke)", "01/15/2010" }));

            Assert.That(actTable.Includes(expTable), Is.False);
        }

        [Test]
        public void IncludesShouldReturnTrueWithTwoColumnsWithSameData()
        {
            Table expTable = new Table();
            Table actTable = new Table();

            expTable.Data.Add(new List<string>(new[] { "Provider", "Date" }));
            expTable.Data.Add(new List<string>(new[] { "Nurse (Ann)", "01/15/2010" }));
            expTable.Data.Add(new List<string>(new[] { "Doctor (Zeke)", "01/15/2010" }));

            actTable.Data.Add(new List<string>(new[] { "Provider", "Date" }));
            actTable.Data.Add(new List<string>(new[] { "Nurse (Ann)", "01/15/2010" }));
            actTable.Data.Add(new List<string>(new[] { "Doctor (Zeke)", "01/15/2010" }));

            Assert.That(actTable.Includes(expTable), Is.True);
        }
    }
}
