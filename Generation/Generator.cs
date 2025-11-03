using System.Xml.Linq;

namespace DialogueConverter;
internal static class Generator
{
    private static int _id;

    public static XElement GenerateXml(this IEnumerable<Lexem> lexems, string sourcePath)
    {
        XElement block  = new("block");
        XAttribute id   = new("id", _id);

        block.Add(id);

        foreach (var lexem in lexems)
        {
            block.Add(lexem.Type switch
            {
                LexemType.Text => ToText(lexem.Content),
                LexemType.Choice => ToChoice(lexem.Content, sourcePath),
                LexemType.Comment => ToComment(lexem.Content),
                _ => throw new NotImplementedException()
            });
        }

        _id++;

        return block;
    }

    private static XElement ToText(string content)
    {
        XElement element = new("text");

        element.Value = content;

        return element;
    }

    private static XComment ToComment(string content)
        => new(content);

    private static XElement ToChoice(string content, string sourcePath)
    {
        string path = PathParser.GetPath(content, sourcePath, out string name);

        XElement element    = new("choice");
        XElement text       = ToText(name);
        XElement next       = new("next");

        next.Value = path;

        element.Add(text);
        element.Add(next);

        return element;
    }
}
