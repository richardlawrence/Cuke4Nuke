using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Cuke4Nuke.Core
{
    public class StepDefinition
    {
        /// <summary>
        /// Initializes a new instance of the StepDefinition class.
        /// </summary>
        /// <param name="pattern"></param>
        /// <param name="method"></param>
        public StepDefinition(string pattern, MethodInfo method)
        {
            _pattern = pattern;
            _method = method;
            _id = GetHashCode().ToString();
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

        private MethodInfo _method;
        public MethodInfo Method
        {
            get { return _method; }
            set
            {
                _method = value;
            }
        }

        public override int GetHashCode()
        {
            return _pattern.GetHashCode() ^ _method.GetHashCode();
        }
    }
}
