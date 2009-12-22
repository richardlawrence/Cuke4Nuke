using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuke4Nuke.Framework
{
    public class TableAssertionException : Exception
    {
        public Table Actual { get; private set; }
        public Table Expected { get; private set;  }

        public TableAssertionException(Table actualTable, Table expectedTable)
        {
            this.Actual = actualTable;
            this.Expected = expectedTable;
        }
    }
}
