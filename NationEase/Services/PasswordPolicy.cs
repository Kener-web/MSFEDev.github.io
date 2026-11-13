namespace NationEase.Services;

public static class PasswordPolicy
{
    public static void EnsureValid(string password)
    {
        if (password.Length < 12 ||
            !password.Any(char.IsUpper) ||
            !password.Any(char.IsLower) ||
            !password.Any(char.IsDigit) ||
            !password.Any(character => !char.IsLetterOrDigit(character)))
        {
            throw new InvalidOperationException(
                "Passwords must be at least 12 characters and include uppercase, lowercase, digit, and symbol.");
        }
    }
}
