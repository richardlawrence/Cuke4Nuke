using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cuke4Nuke.Framework.Languages.Norwegian;
using NUnit.Framework;

namespace KalkulatorFeatures
{
    class KalkulatorSteps
    {
        private Kalkulator.Kalkulator kalkulator;


        [Gitt(@"^at jeg trykker ""(.+)"" pluss ""(.+)""$")]
        public void AtJegTrykker4Pluss4(string tall, string annetTall)
        {
            kalkulator = new Kalkulator.Kalkulator();
            kalkulator.LeggSammen(int.Parse(tall), int.Parse(annetTall));
        }

        [Så(@"^skal jeg få ""(\d+)""$")]
        public void SkalJegF8(string result)
        {
            Assert.That(int.Parse(result), Is.EqualTo(kalkulator.Sum()));
        }
    }
}
