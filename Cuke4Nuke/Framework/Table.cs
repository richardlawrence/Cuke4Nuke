using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuke4Nuke.Framework
{
    public class Table
    {
        public IList<Dictionary<string, string>> Hashes
        {
            get
            {
                return new List<Dictionary<string, string>>();
            }
        }
    }
}
