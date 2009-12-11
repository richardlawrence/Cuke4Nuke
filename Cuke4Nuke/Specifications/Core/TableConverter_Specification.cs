using System.ComponentModel;
using Cuke4Nuke.Framework;
using NUnit.Framework;
using Cuke4Nuke.Core;
using System;

namespace Cuke4Nuke.Specifications.Core
{
    public class TableConverter_Specification
    {
        [SetUp]
        public void SetUp()
        {
            TypeConverterAttribute attr = new TypeConverterAttribute(typeof(TableConverter));
            TypeDescriptor.AddAttributes(typeof(Table), new Attribute[] { attr });
        }

        [Test]
        public void ShouldConvertFromString()
        {
            string serializedTable = "[[\"foo\",\"1\"],[\"bar\",\"2\"]]";
            Assert.DoesNotThrow(delegate {                
                TypeConverter converter = TypeDescriptor.GetConverter(typeof(Table));
                Table table = (Table) converter.ConvertFromString(serializedTable);
            });
        }
    }
}
