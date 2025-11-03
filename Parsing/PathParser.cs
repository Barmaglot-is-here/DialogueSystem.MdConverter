namespace DialogueConverter;
internal static class PathParser
{
    public static string GetPath(string rawChoice, string filePath)
        => GetPath(rawChoice, filePath, out _);

    public static string GetPath(string rawChoice, string filePath, out string name)
    {
        string directory    = Path.GetDirectoryName(filePath)!;
        string path         = GetPath(rawChoice, out name);

        return Path.Combine(directory, path + ".md");
    }

    public static string GetPath(string rawChoice, out string name)
    {
        int splitterIndex = rawChoice.IndexOf('|');

        string path = splitterIndex != -1
                    ? rawChoice[..splitterIndex]
                    : rawChoice;
        name        = splitterIndex != -1
                    ? rawChoice[++splitterIndex..]
                    : rawChoice;

        return path;
    }
}
