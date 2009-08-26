using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace WhatToPack.Core
{
    public class Weather
    {
        public string GetForecast(string zipCode)
        {
            string serviceUrl = 
                @"http://www.weather.gov/forecasts/xml/sample_products/browser_interface/ndfdXMLclient.php?zipCodeList=" + 
                zipCode + "&product=time-series&begin=2004-01-01T00:00:00&end=2013-04-21T00:00:00&pop12=pop12";
            WebClient client = new WebClient();
            return client.DownloadString(serviceUrl);
        }
    }
}
