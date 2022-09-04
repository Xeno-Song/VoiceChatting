using System;
using System.Collections.Generic;
using System.Reflection;
using VoiceChattingManagementServer.Core.Database.Attributes;

namespace VoiceChattingManagementServer.Core.Database.Builder
{
    public enum Comparator
    {
        NotEqual = 0,

        Less = 1,
        Equal = 2,
        Greater = 4,

        LessOrEqual = 3,
        GreaterOrEqual = 6,
    }

    public class QueryBuilder<Model> where Model : Entity, new()
    {
        private List<string> conditions;
        private readonly List<PropertyInfo> properties;

        public QueryBuilder()
        {
            conditions = new List<string>();
            properties = new List<PropertyInfo>();

            conditions.Clear();
            properties.AddRange(typeof(Model).GetProperties());
        }

        public void AddCondition(string propertyName, Comparator comparator, int value)
        {
            int propertyIndex = properties.FindIndex((propertyInfo) => propertyInfo.Name == propertyName);
            if (propertyIndex == -1) // Could not found property name in target model
            {
                throw new ArgumentException($"Could not found property name [{propertyName}]");
            }

            var propertyInfo = properties[propertyIndex];
            var columnDefinition = propertyInfo.GetCustomAttribute<ColumnAttribute>();
            if (columnDefinition == null)
            {
                throw new ArgumentException($"This property doesn't defined column name [{propertyName}]");
            }

            string conditionQuery = propertyName;
            conditionQuery += ConvertComparatorToString(comparator);
            conditionQuery += value;

            conditions.Add(conditionQuery);
        }

        private static string ConvertComparatorToString(Comparator comparator)
        {
            switch (comparator)
            {
                case Comparator.NotEqual:       return "!=";
                case Comparator.Less:           return "<";
                case Comparator.Equal:          return "=";
                case Comparator.LessOrEqual:    return "<=";
                case Comparator.Greater:        return ">";
                case Comparator.GreaterOrEqual: return ">=";
            }

            throw new ArgumentException("Could not add query with this comparator [" +
                                       $"NotEuql : {(((int)comparator == 0) ? "True" : "False")}]" +
                                       $"Less : {((((int)comparator & (int)Comparator.Less) == 0) ? "True" : "False")}]" +
                                       $"Equal : {((((int)comparator & (int)Comparator.Equal) == 0) ? "True" : "False")}]" +
                                       $"Greater : {((((int)comparator & (int)Comparator.Greater) == 0) ? "True" : "False")}]");
        }
    }
}
