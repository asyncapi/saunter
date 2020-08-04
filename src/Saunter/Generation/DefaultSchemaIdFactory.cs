using System;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Saunter.AsyncApiSchema.v2;

namespace Saunter.Generation
{
    public static class DefaultSchemaIdFactory
    {
        private static readonly NamingStrategy CamelCase = new CamelCaseNamingStrategy(); 
        
        public static string Generate(Type type)
        {
            var name = NameForType(type);
            
            if (type.IsGenericType)
            {
                // generic type names look like this
                // List`1
                // we need to strip off the arity "`n" and replace it with the names of the type arguments
                // e.g. listOfFoo
                name = WithoutArity(name);
                var genericTypeArgNames = type.GetGenericArguments().Select(NameForType);

                name = name + "Of" + string.Join("And", genericTypeArgNames);
            }

            // strip any invalid characters from generated names
            name = Regex.Replace(name, ComponentFieldName.InvalidRegex, string.Empty);
            
            // and lastly, camelCase the name
            name = CamelCase.GetPropertyName(name, false);
            
            return name;
        }

        private static string NameForType(Type type)
        {
            // We can create classes like
            // public class Foo {
            //     public class Bar {}
            // }
            // which should be named as "fooBar", rather than just "bar"
            var name = type.IsNested
                ? NameForType(type.DeclaringType) + type.Name
                : type.Name;
            

            // If a title is specified, use that
            var jsonObject = type.GetCustomAttribute<JsonObjectAttribute>();
            if (jsonObject != null && !string.IsNullOrWhiteSpace(jsonObject.Title))
            {
                name = jsonObject.Title;
            }

            return name;
        }

        /// <summary>
        /// Removes the arity from a type name.
        /// e.g. List`1 -> List, Dictionary`2 -> Dictionary 
        /// </summary>
        private static string WithoutArity(string value)
        {
            var index = value.IndexOf('`');
            return index == -1 ? value : value.Substring(0, index);
        }
    }
}