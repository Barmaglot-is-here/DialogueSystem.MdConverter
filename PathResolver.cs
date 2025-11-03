using System.Xml.Linq;

namespace DialogueConverter;
internal class PathResolver
{
    private readonly Dictionary<string, int> _pathIdPairs;

    public PathResolver()
    {
        _pathIdPairs = new();
    }

    public void Add(string path, int blockId) 
        => _pathIdPairs[path] = blockId;

    public void Resolve(XElement block)
    {
        foreach (var choice in block.Elements("choice"))
        {
            var next    = choice.Element("next")!;
            var path    = next.Value;
            var id      = _pathIdPairs[path];

            next.Value = id.ToString();
        }
    }
}
