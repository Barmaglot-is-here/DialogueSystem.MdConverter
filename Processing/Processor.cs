namespace DialogueConverter;
internal static class Processor
{
    public static IEnumerable<Lexem> Process(this IEnumerable<Lexem> lexems)
    {
        foreach (var lexem in lexems)
        {
            if (lexem.Type == LexemType.Text)
                lexem.Content = lexem.Content.Trim();
            else if (lexem.Type == LexemType.Comment)
                lexem.Content = lexem.Content.Replace("//", "");
            else if (lexem.Type == LexemType.Choice)
                lexem.Content = FormatChoice(lexem.Content);

            yield return lexem;
        }
    }

    private static string FormatChoice(string choice)
    {
        int beginIndex  = choice.LastIndexOf('[');
        int endIndex    = choice.IndexOf(']');

        return choice[++beginIndex..endIndex];
    }
}
