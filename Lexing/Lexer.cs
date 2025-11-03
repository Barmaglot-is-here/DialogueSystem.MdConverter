using System.Text.RegularExpressions;

namespace DialogueConverter;
internal static class Lexer
{
    private readonly static Dictionary<LexemType, Regex> _patterns = new()
    {
        { LexemType.Text,           new(@"^([^\[//])+", RegexOptions.Compiled) },
        { LexemType.Choice,         new(@"^\[\[.*]]",    RegexOptions.Compiled) },
        { LexemType.Comment,        new(@"//.*",    RegexOptions.Compiled) },
    };

    public static IEnumerable<Lexem> Split(string src)
    {
        Queue<Lexem> lexems = new();

        while (src.Length != 0)
        {
            Lexem lexem = ReadNext(ref src);

            lexems.Enqueue(lexem);
        }

        return lexems;
    }

    private static Lexem ReadNext(ref string text)
    {
        foreach (var pattern in _patterns)
        {
            var match = pattern.Value.Match(text);

            if (match.Success)
            {
                string content = text[..match.Length];
                text = text.Remove(0, match.Length).TrimStart();

                return new(pattern.Key, content);
            }
        }

        throw new Exception($"Unparsable part: {text}");
    }
}
