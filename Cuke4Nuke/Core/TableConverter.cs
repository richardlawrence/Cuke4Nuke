using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Cuke4Nuke.Framework;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Cuke4Nuke.Core
{
    public class TableConverter : TypeConverter
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            if (sourceType == typeof(string))
            {
                return true;
            }
            return base.CanConvertFrom(context, sourceType);
        }
        
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string)
            {
                return JsonToTable(value.ToString());
            }
            return base.ConvertFrom(context, culture, value);
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string))
            {
                throw new NotImplementedException();
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }

        public Table JsonToTable(string jsonTable)
        {
            log.DebugFormat("Trying to convert JSON to table: {0}", jsonTable);
            Table table = new Table();
            
            if (jsonTable == null || jsonTable == String.Empty)
            {
                return table;
            }

            ArgumentException invalidDataEx = new ArgumentException("Input not in expected format. Expecting a JSON array of string arrays.");

            JArray rows;
            try
            {
                rows = JArray.Parse(jsonTable);
            }
            catch (Exception ex)
            {
                throw invalidDataEx;
            }

            if (rows.Count == 0)
            {
                return table;
            }

            foreach (JToken row in rows.Children())
            {
                if (!(row.Type == JTokenType.Array))
                {
                    throw invalidDataEx;
                }

                var values = new List<string>();

                foreach (JToken cell in row.Children())
                {
                    if (!(cell.Type == JTokenType.String))
                    {
                        throw invalidDataEx;
                    }
                    values.Add(cell.Value<string>() as string);
                }

                table.Data.Add(values);
            }
            log.DebugFormat("Converted JSON to table with {0} rows (including headers).", table.Data.Count);
            return table;
        }
    }
}
