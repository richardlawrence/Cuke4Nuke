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

        [Test]
        public void HeadersShouldReturnListOfHeaders()
        {
            Table tbl = new Table();

            tbl.Data.Add(new List<string>(new[] { "Provider", "Date" }));
            tbl.Data.Add(new List<string>(new[] { "Nurse (Ann)", "01/15/2010" }));
            tbl.Data.Add(new List<string>(new[] { "Doctor (Zeke)", "01/15/2010" }));

            Assert.That(tbl.Headers(), Is.EqualTo(new List<string>(new[] { "Provider", "Date" })));
        }

        [Test]
        public void HeadersShouldReturnEmptyListForEmptyTable()
        {
            Table tbl = new Table();
            Assert.That(tbl.Headers(), Is.EqualTo(new List<string>()));
        }

        [Test]
        public void RowHashesShouldReturnDictionaryForTwoColumnTable()
        {
            Table tbl = new Table();
            tbl.Data.Add(new List<string>(new[] { "Key1", "Value1" }));
            tbl.Data.Add(new List<string>(new[] { "Key2", "Value2" }));
            tbl.Data.Add(new List<string>(new[] { "Key3", "Value3" }));

            Assert.That(tbl.RowHashes(), Is.EqualTo(new Dictionary<string, string>()
                {
                    { "Key1", "Value1" },
                    { "Key2", "Value2" },
                    { "Key3", "Value3" }
                }
            ));
        }

        [Test]
        public void RowHashesShouldThrowForOneColumnTable()
        {
            Table tbl = new Table();
            tbl.Data.Add(new List<string>(new[] { "Value1" }));
            tbl.Data.Add(new List<string>(new[] { "Value2" }));
            tbl.Data.Add(new List<string>(new[] { "Value3" }));

            Assert.Throws<InvalidOperationException>(delegate
            {
                tbl.RowHashes();
            });
        }

        [Test]
        public void RowHashesShouldThrowForThreeColumnTable()
        {
            Table tbl = new Table();
            tbl.Data.Add(new List<string>(new[] { "Value1", "Value1", "Value1" }));
            tbl.Data.Add(new List<string>(new[] { "Value2", "Value2", "Value2" }));
            tbl.Data.Add(new List<string>(new[] { "Value3", "Value3", "Value3" }));

            Assert.Throws<InvalidOperationException>(delegate
            {
                tbl.RowHashes();
            });
        }

        [Test]
        public void RowHashesShouldThrowForEmptyTable()
        {
            Table tbl = new Table();

            Assert.Throws<InvalidOperationException>(delegate
            {
                tbl.RowHashes();
            });
        }
    }
}
