using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatToPack.Core
{
    public class PackingList : List<PackingListItem>
    {
        public PackingList Filter(Forecast forecast)
        {
            PackingList filteredList = new PackingList();
            foreach (PackingListItem item in this)
            {
                if (item.Condition == null || ForecastCondition.Parse(item.Condition).Test(forecast))
                {
                    filteredList.Add(item);
                }
            }
            return filteredList;
        }
    }
}
