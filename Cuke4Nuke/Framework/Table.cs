using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuke4Nuke.Framework
{
    public class Table
    {
        public List<Dictionary<string, string>> Hashes()
        {
            var hashes = new List<Dictionary<string, string>>();
            if (_data.Count == 0)
            {
                return hashes;
            }

            var keys = new List<string>();
            _data[0].ForEach(val => keys.Add(val));

            for (int i = 1; i < _data.Count; i++)
            {
                var hash = new Dictionary<string, string>();
                for (int j = 0; j < _data[i].Count; j++)
                {
                    hash.Add(keys[j], _data[i][j]);
                }
                hashes.Add(hash);
            }

            return hashes;
        }

        List<List<string>> _data = new List<List<string>>();
        public List<List<string>> Data
        {
            get
            {
                return _data;
            }
        }

        public void AssertSameAs(Table expectedTable)
        {
            throw new TableAssertionException(this, expectedTable);
        }

        public bool Includes(Table expectedTable)
        {
            foreach (var exRow in expectedTable.Hashes())
            {
                bool hasMatchingRow = false;
                foreach (var acRow in this.Hashes())
                {
	                foreach (var key in exRow.Keys)
	                {
		                if (exRow[key] == acRow[key])
		                {
			                hasMatchingRow = true;
		                }
		                else
		                {
			                hasMatchingRow = false;
			                break;
		                }
	                }
	                if(hasMatchingRow)
	                    break;
                }
                if (!hasMatchingRow)
		            return false;
		            
            }
            return true;
        }
    }
}
