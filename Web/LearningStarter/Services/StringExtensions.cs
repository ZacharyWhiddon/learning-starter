namespace LearningStarter.Services;

public static class StringExtensions
{
    public static int? SafeParseInt(this string s)
    {
        if (int.TryParse(s, out var result)) return result;
        return null;
    }
}