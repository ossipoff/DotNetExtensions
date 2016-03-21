using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DotNetExtensions
{
    public static class Object
    {
        public static TValue GetMemberValue<T, TValue>(this T o, Expression<Func<T, TValue>> member)
        {
            return (TValue)GetMemberValueFromExpression(o, member.Body);
        }

        public static string GetMemberName<T>(this T o, Expression<Action<T>> member)
        {
            return GetMemberNameFromExpression(member.Body);
        }

        public static string GetMemberName<T>(this T o, Expression<Func<T, object>> member)
        {
            return GetMemberNameFromExpression(member.Body);
        }

        public static bool AllEqual<T>(this T o, params T[] other)
        {
            return AllEqual<T>(o, EqualityComparer<T>.Default, other);
        }

        public static bool AllEqual<T>(this T x, IEqualityComparer<T> comparer, params T[] ys)
        {
            if (comparer == null)
            {
                throw new ArgumentNullException("comparer");
            }

            return ys.All(y => comparer.Equals(x, y));
        }

        private static Dictionary<System.Type, Func<object, Expression, object>> getMemberValueActionMap = new Dictionary<System.Type, Func<object, Expression, object>>()
        {
            { typeof(MemberExpression), GetMemberValueFromMemberExpression },
            { typeof(MethodCallExpression), GetMemberValueFromUnaryExpression }
        };

        private static object GetMemberValueFromMemberExpression(object o, Expression expression)
        {
            return GetMemberValueFromMemberInfo(o, (expression as MemberExpression).Member);
        }

        private static object GetMemberValueFromUnaryExpression(object o, Expression expression)
        {
            return GetMemberValueFromMemberInfo(o, ((expression as UnaryExpression).Operand as MemberExpression).Member);
        }

        private static object GetMemberValueFromMemberInfo(object o, MemberInfo memberInfo)
        {
            var propertyInfo = memberInfo as PropertyInfo;
            if (propertyInfo != null)
            {
                return propertyInfo.GetValue(o);
            }

            var fieldInfo = memberInfo as FieldInfo;
            if (fieldInfo != null)
            {
                return fieldInfo.GetValue(o);
            }

            throw new ArgumentException("memberInfo must be either PropertyInfo or FieldInfo");
        }

        private static object GetMemberValueFromExpression(object o, Expression expression)
        {
            var type = expression.GetType();

            if (!type.IsPublic)
            {
                type = type.BaseType;
            }

            if (getMemberValueActionMap.ContainsKey(type))
            {
                return getMemberValueActionMap[type](o, expression);
            }

            return null;
        }

        private static Dictionary<System.Type, Func<Expression, string>> getMemberNameActionMap = new Dictionary<System.Type, Func<Expression, string>>()
        {
            { typeof(MemberExpression), GetMemberNameFromMemberExpression },
            { typeof(MethodCallExpression), GetMemberNameFromMethodCallExpression },
            { typeof(UnaryExpression), GetMemberNameFromUnaryExpression }
        };

        private static string GetMemberNameFromUnaryExpression(Expression expression)
        {
            return ((expression as UnaryExpression).Operand as MemberExpression).Member.Name;
        }

        private static string GetMemberNameFromMemberExpression(Expression expression)
        {
            return (expression as MemberExpression).Member.Name;
        }

        private static string GetMemberNameFromMethodCallExpression(Expression expression)
        {
            return (expression as MethodCallExpression).Method.Name;
        }

        private static string GetMemberNameFromExpression(Expression expression)
        {
            var type = expression.GetType();

            if (!type.IsPublic)
            {
                type = type.BaseType;
            }

            if (getMemberNameActionMap.ContainsKey(type))
            {
                return getMemberNameActionMap[type](expression);
            }

            return null;
        }
    }
}
