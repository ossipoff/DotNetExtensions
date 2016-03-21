using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DotNetExtensions
{
    public static class Type
    {
        public enum AttributeSearchMode
        {
            /// <summary>
            /// Searches the specified type only, not checking inheritance chain
            /// </summary>
            TypeOnly,

            /// <summary>
            /// Searches the specified type and its descendants, but respects the AttributeUsage Inherited property
            /// </summary>
            InheritanceChain,
            /// <summary>
            /// Searches the specified type and its descendants and disregards the AttributeUsage Inherited property
            /// </summary>
            InheritanceChainForced
        }

        public static IEnumerable<T> GetCustomAttributes<T>(this System.Type t, AttributeSearchMode attributeSearchMode = AttributeSearchMode.TypeOnly) where T : Attribute
        {
            var customAttributes = t.GetCustomAttributes(typeof(T), attributeSearchMode != AttributeSearchMode.TypeOnly);

            if (customAttributes?.Length == 0 && attributeSearchMode == AttributeSearchMode.InheritanceChainForced && t.BaseType != typeof(object))
            {
                customAttributes = (t.BaseType.GetCustomAttributes<T>(attributeSearchMode)).ToArray();
            }

            return customAttributes.OfType<T>();
        }

        public static T GetCustomAttribute<T>(this System.Type t, AttributeSearchMode attributeSearchMode = AttributeSearchMode.TypeOnly) where T : Attribute
        {
            return t.GetCustomAttributes<T>(attributeSearchMode).SingleOrDefault();
        }
    }
}
