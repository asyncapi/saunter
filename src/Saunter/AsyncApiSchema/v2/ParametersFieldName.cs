using System;
using System.Text.RegularExpressions;

namespace Saunter.AsyncApiSchema.v2
{
    public class ParametersFieldName
    {
        private const string ValidRegex = @"^[A-Za-z0-9_\-]+$";

        private readonly string value;

        public ParametersFieldName(string fieldName)
        {
            if (fieldName == null) throw new ArgumentNullException(nameof(fieldName));
            if (!Regex.IsMatch(fieldName, ValidRegex)) throw new Exception($"parameter field name must match pattern {ValidRegex}");

            value = fieldName;
        }

        public override string ToString() => value;

        public override int GetHashCode() => value.GetHashCode();

        public override bool Equals(object obj) => obj is ParametersFieldName parametersFieldName && value.Equals(parametersFieldName.value);

        public static implicit operator ParametersFieldName(string s) => new ParametersFieldName(s);
    }
}