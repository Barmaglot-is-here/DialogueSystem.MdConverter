using System.Xml.Linq;

namespace DialogueConverter;
internal static class Extensions
{
    public static int AttributeInt(this XElement element, XName name)
    {
        var value = element.Attribute(name)!.Value;

        return int.Parse(value);
    }
}
