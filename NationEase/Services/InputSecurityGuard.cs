using System.Text.RegularExpressions;

namespace NationEase.Services;

public static class InputSecurityGuard
{
    private static readonly Regex XssPattern = new(
        @"</?\s*(?:script|iframe|object|embed|svg|math)\b|javascript\s*:|data\s*:\s*text/html|<[^>]*\bon[a-z]+\s*=",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.NonBacktracking);

    private static readonly Regex SqlInjectionPattern = new(
        @"--(?:\s|$)|/\*|\*/|\bunion\s+(?:all\s+)?select\b|\b(?:select\s+.+\s+from|insert\s+into|update\s+\w+\s+set|delete\s+from|drop\s+(?:table|database)|alter\s+table|attach\s+database|pragma\s+\w+)\b|['""]\s*(?:or|and)\s+(?:['""]?\w+['""]?\s*=\s*['""]?\w+|[0-9]+\s*=\s*[0-9]+)",
        RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.NonBacktracking);

    public static string? GetWarning(params string?[] values)
    {
        foreach (var value in values)
        {
            if (string.IsNullOrEmpty(value))
            {
                continue;
            }

            if (XssPattern.IsMatch(value))
            {
                return "Pan! No XSS allowed!";
            }

            if (SqlInjectionPattern.IsMatch(value))
            {
                return "Pan! No SQL Injection allowed!";
            }
        }

        return null;
    }
}
