namespace DialogueConverter;
internal class Lexem
{
    public LexemType Type { get; }
    public string Content { get; set; }

    public Lexem(LexemType type, string content)
    {
        Type    = type;
        Content = content;
    }
}
