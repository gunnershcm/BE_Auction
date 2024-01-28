using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Helpers;

public static class DataResponse
{

    public static T CleanNullableDateTime<T>(T input)
    {
        if (input == null)
            return input;

        Type type = input.GetType();

        if (type.IsValueType)
        {
            if (type == typeof(DateTime) && (DateTime)Convert.ChangeType(input, typeof(DateTime)) == DateTime.MinValue)
            {
                return (T)(object)null;
            }
        }
        else if (type.IsClass)
        {
            PropertyInfo[] properties = type.GetProperties();

            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(DateTime))
                {
                    DateTime propertyValue = (DateTime)property.GetValue(input, null);
                    if (propertyValue == DateTime.MinValue)
                    {
                        property.SetValue(input, null);
                    }
                }
                else if (property.PropertyType.IsClass)
                {
                    // Recursively clean nested objects
                    var propertyValue = property.GetValue(input);
                    propertyValue = CleanNullableDateTime(propertyValue);
                    property.SetValue(input, propertyValue);
                }
            }
        }

        return input;
    }


    public static DateTime? CleanNullableDateTime(DateTime? dateTime)
    {
        return dateTime == DateTime.MinValue ? null : dateTime;
    }

}
