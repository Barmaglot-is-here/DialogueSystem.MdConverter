using System.Xml.Linq;

namespace DialogueConverter;

internal class Program
{
    static void Main()
    {
        var document = CreateDocument();

        Console.WriteLine("Enter source path");

        string sourceFile = Console.ReadLine()!;

        try
        {
            Fill(document, sourceFile);
        }
        catch
        {
            Console.WriteLine($"Error in: {sourceFile}\n");

            throw;
        }

        Save(document, sourceFile);

        Console.ReadKey();
    }

    private static XDocument CreateDocument()
    {
        XDocument document  = new();
        XElement root       = new("dialog");

        document.Add(root);

        return document;
    }

    private static void Fill(XDocument document, string sourceFile)
    {
        PathResolver resolver = new();

        List<string> parsingPaths = new();
        parsingPaths.Add(sourceFile);

        while (parsingPaths.Count > 0)
        {
            sourceFile = parsingPaths[0];

            string content = File.ReadAllText(sourceFile);

            var lexems = Lexer.Split(content)
                              .Process()
                              .ToArray();

            var paths = lexems.Where(lexem => lexem.Type == LexemType.Choice)
                              .Select(lexem => PathParser.GetPath(lexem.Content,
                                                                  sourceFile));

            var block = lexems.GenerateXml(sourceFile);

            resolver.Add(sourceFile, block.AttributeInt("id"));
            document.Root!.Add(block);

            parsingPaths.RemoveAt(0);
            parsingPaths.AddRange(paths);
        }

        foreach (var element in document.Root!.Elements())
            resolver.Resolve(element);
    }

    private static void Save(XDocument document, string sourceFile)
    {
        var name            = Path.GetFileNameWithoutExtension(sourceFile);
        var saveDirectory   = Path.GetDirectoryName(sourceFile)!;
        var savePath        = Path.Combine(saveDirectory, name + ".xml");

        document.Save(savePath);

        Console.WriteLine("Saved at: " + savePath);
    }
}
