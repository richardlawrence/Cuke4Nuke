using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using fitlibrary;
using WhatToPack.Core;
using System.Reflection;
using System.Text.RegularExpressions;
using System.IO;

namespace WhatToPack.Test
{
    public class EvaluatePackingRules : DoFixture
    {
        PackingList _packingListWithConditions = new PackingList();
        PackingList _generatedPackingList = new PackingList();
        Forecast _forecast = new Forecast();

        public PackingListBuilder PackingListWithConditions()
        {
            return new PackingListBuilder(_packingListWithConditions);
        }

        public void PrecipitationProbabilityIs(int probability)
        {
            _forecast.PrecipitationProbability = probability;
        }

        public void GeneratePackingList()
        {
            _generatedPackingList = _packingListWithConditions.Filter(_forecast);
        }

        private string GetXmlFromResource(string xmlFileName)
        {
            Assembly asm = Assembly.GetExecutingAssembly();
            string[] resourceNames = asm.GetManifestResourceNames();
            string pattern = xmlFileName.Replace(" ", ""); +@"\.xml$";
            Predicate<string> predicate = delegate(string resourceName)
            {
                return Regex.IsMatch(resourceName, pattern);
            };
            string selectedResourceName = resourceNames.First(predicate);
            Stream stream = asm.GetManifestResourceStream(selectedResourceName);
            byte[] buffer = new byte[stream.Length];
            stream.Read(buffer, 0, buffer.Length);
            return buffer.ToString();
        }

        public void GeneratePackingListFor(string weatherXmlFileName) 
        {
            Forecast forecast = new Forecast();
            switch (weatherXmlFileName) {
                case "high probability of precipitation":
                    forecast.PrecipitationProbability = 90;
                    break;
                case "low probability of precipitation":
                    forecast.PrecipitationProbability = 10;
                    break;
            }
            _generatedPackingList = _packingListWithConditions.Filter(forecast);
        }

        public PackingList PackingListContains()
        {
            return _generatedPackingList;
        }
    }

    public class PackingListBuilder : SetUpFixture
    {
        PackingList _packingListWithConditions;

        public PackingListBuilder(PackingList packingListWithConditions)
        {
            _packingListWithConditions = packingListWithConditions;
        }

        public void PackIf(string item, string condition)
        {
            PackingListItem pli = new PackingListItem();
            pli.Item = item;
            pli.Condition = condition;
            _packingListWithConditions.Add(pli);
        }
    }
}
