using System.Collections.Generic;

namespace Our.Extensions
{
    public static class StringExtensions
    {
        public static string ReplaceVariables(this string source, Dictionary<string, string> variables)
        {
            variables.ForEach(variablePair => source = source
                .Replace($"{{{variablePair.Key}}}", variablePair.Value));
            return source;
        }
    }
}