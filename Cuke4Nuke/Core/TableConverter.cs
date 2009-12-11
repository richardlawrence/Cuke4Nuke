using System;
using System.ComponentModel;
using System.Globalization;
using Cuke4Nuke.Framework;

namespace Cuke4Nuke.Core
{
    public class TableConverter : TypeConverter
    {
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
                return new Table();
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
    }
}
