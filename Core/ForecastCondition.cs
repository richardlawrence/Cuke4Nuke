using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq.Expressions;
using System.Reflection;

namespace WhatToPack.Core
{
    public class ForecastCondition
    {
        private string _property;
        public string Property
        {
            get { return _property; }
            set
            {
                _property = value;
            }
        }

        private string _operator;
        public string Operator
        {
            get { return _operator; }
            set
            {
                _operator = value;
            }
        }

        private int _value;
        public int Value
        {
            get { return _value; }
            set
            {
                _value = value;
            }
        }

        public static ForecastCondition Parse(string condition)
        {
            string pattern = @"(\w+) (\<|\>|\<=|\>=|=|==|!=) (\d+)";
            Match match = Regex.Match(condition, pattern);
            if (match.Success)
            {
                ForecastCondition fc = new ForecastCondition();
                fc.Property = match.Groups[1].Value;
                fc.Operator = match.Groups[2].Value;
                fc.Value = Int32.Parse(match.Groups[3].Value);
                return fc;
            }
            else
            {
                throw new ArgumentException("Unable to parse.", "condition");
            }
        }

        private ExpressionType GetExpressionType()
        {
            ExpressionType expressionType;
            switch (this.Operator)
            {
                case "<":
                    expressionType = ExpressionType.LessThan;
                    break;
                case "<=":
                    expressionType = ExpressionType.LessThanOrEqual;
                    break;
                case ">":
                    expressionType = ExpressionType.GreaterThan;
                    break;
                case ">=":
                    expressionType = ExpressionType.GreaterThanOrEqual;
                    break;
                case "=":
                    expressionType = ExpressionType.Equal;
                    break;
                case "==":
                    expressionType = ExpressionType.Equal;
                    break;
                case "!=":
                    expressionType = ExpressionType.NotEqual;
                    break;
                default:
                    throw new InvalidOperationException("Operator is invalid.");
            }
            return expressionType;
        }

        internal bool Test(int value)
        {
            ExpressionType expressionType = GetExpressionType();
            ParameterExpression param = Expression.Parameter(typeof(int), "p");
            ConstantExpression constant = Expression.Constant(this.Value);
            BinaryExpression filter = Expression.MakeBinary(expressionType, param, constant);
            Expression<Predicate<int>> predicate = 
                Expression.Lambda<Predicate<int>>(filter, new ParameterExpression[] { param });
            return predicate.Compile().Invoke(value);
        }

        public bool Test(Forecast forecast)
        {
            int value = (int) forecast.GetType().GetProperty(this.Property).GetValue(forecast, null);
            return Test(value);
        }
    }
}
