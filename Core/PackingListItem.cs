using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WhatToPack.Core
{
    public class PackingListItem
    {
        private string _item;
        public string Item
        {
            get { return _item; }
            set
            {
                _item = value;
            }
        }

        private string _condition;
        public string Condition
        {
            get { return _condition; }
            set
            {
                _condition = value;
            }
        }

        /// <summary>
        /// Initializes a new instance of the PackingListItem class.
        /// </summary>
        /// <param name="item"></param>
        /// <param name="condition"></param>
        public PackingListItem(string item, string condition)
        {
            _item = item;
            _condition = condition;
        }

        /// <summary>
        /// Initializes a new instance of the PackingListItem class.
        /// </summary>
        public PackingListItem()
        {
        }

    }
}
