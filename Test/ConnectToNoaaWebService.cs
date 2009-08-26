using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fit;
using WhatToPack.Core;
using System.Xml;

namespace WhatToPack.Test
{
    public class ConnectToNoaaWebService : ColumnFixture
    {
        public string ZipCode;

        public bool CanGetForecast()
        {
            Weather weather = new Weather();
            string xmlData = weather.GetForecast(ZipCode);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlData);
            XmlNode errorMessageNode = xmlDoc.SelectSingleNode("/errorMessage");
            bool hasError = (errorMessageNode != null);
            return !hasError;
        }
    }
}
