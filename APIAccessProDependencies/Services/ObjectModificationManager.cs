using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APIAccessProDependencies.Services
{
    public static class ObjectModificationManager
    {
        public static bool IsNullOrEmpty(object value)
        {
            if (value == null)
                return true;

            if (value is string str)
                return string.IsNullOrEmpty(str);

            return false;
        }
        public static bool IsArray(Type type)
        {
            return type.IsArray || (type.IsGenericType && type.GetInterfaces().Any(i => i == typeof(IEnumerable)));
        }

        public static void UpdateProperties(object existingObj, object newObj)
        {
            // Get properties of the existing object's type
            var properties = existingObj.GetType().GetProperties();

            foreach (var property in properties)
            {                
                var propertyName = property.Name;

                // Skip properties that should not be updated
                if (propertyName.ToUpper() == "ID" || propertyName.ToUpper() == "PROGRAMID") continue;

                var existingValue = property.GetValue(existingObj);
                var newValue = property.GetValue(newObj);

                //Special Update for Arrays
                if (IsArray(property.PropertyType))
                {
                    UpdateArrayElements(existingValue, newValue);
                    continue;
                }

                // If the new value is not null or empty (or a string), update the property
                if (!IsNullOrEmpty(newValue) && newValue.ToString().ToLower() != "string")
                {
                    if (property.PropertyType.IsClass && !property.PropertyType.IsPrimitive && property.PropertyType != typeof(string))
                    {
                        // If the property is a class (e.g., sub-object), recursively update its properties
                        UpdateProperties(existingValue, newValue);
                    }
                    else
                    {
                        // Update the property with the new value
                        property.SetValue(existingObj, newValue);
                    }
                }
            }
        }
        public static void UpdateArrayElements(object existingArray, object newArray)
        {
            if (existingArray is IEnumerable existingEnumerable && newArray is IEnumerable newEnumerable)
            {
                // Get the type of elements within the array
                var elementType = existingArray.GetType().GetElementType() ?? newArray.GetType().GetElementType();

                // Check if the element type is a reference type (class)
                if (elementType != null && elementType.IsClass && !elementType.IsPrimitive && elementType != typeof(string))
                {
                    // If the elements are classes (e.g., sub-objects), update their properties
                    var existingEnumerator = existingEnumerable.GetEnumerator();
                    var newEnumerator = newEnumerable.GetEnumerator();

                    while (existingEnumerator.MoveNext() && newEnumerator.MoveNext())
                    {
                        UpdateProperties(existingEnumerator.Current, newEnumerator.Current);
                    }
                }
            }
        }
    }
}
