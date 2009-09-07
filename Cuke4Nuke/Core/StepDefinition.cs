using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cuke4Nuke.Core
{
    public class StepDefinition
    {
        /// <summary>
        /// Initializes a new instance of the StepDefinition class.
        /// </summary>
        public StepDefinition()
        {
        }

        /// <summary>
        /// Initializes a new instance of the StepDefinition class.
        /// </summary>
        /// <param name="pattern"></param>
        public StepDefinition(string pattern)
        {
            _pattern = pattern;
        }

        /// <summary>
        /// Initializes a new instance of the StepDefinition class.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="pattern"></param>
        public StepDefinition(string id, string pattern)
        {
            _id = id;
            _pattern = pattern;
        }

        private string _id;
        public string Id
        {
            get { return _id; }
            set
            {
                _id = value;
            }
        }

        private string _pattern;
        public string Pattern
        {
            get { return _pattern; }
            set
            {
                _pattern = value;
            }
        }
    }
}
