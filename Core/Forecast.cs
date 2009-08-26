using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatToPack.Core
{
    public class Forecast
    {
        private int _precipitationProbability;
        public int PrecipitationProbability
        {
            get { return _precipitationProbability; }
            set
            {
                if (value < 0 || value > 100)
                {
                    throw new ArgumentOutOfRangeException(
                        "PrecipitationProbability must be between 0 and 100."
                        );
                }
                _precipitationProbability = value;
            }
        }
        
    }
}
