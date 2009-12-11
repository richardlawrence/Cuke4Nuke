using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuke4Nuke.Framework
{
    public class Table
    {
        List<Dictionary<string, string>> _rows = new List<Dictionary<string, string>>();
        
        public IList<Dictionary<string, string>> Hashes
        {
            get
            {
                return _rows;
            }
        }
    }
}
