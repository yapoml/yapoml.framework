using Yapoml.Parsers.Yaml.Pocos;

namespace Yapoml.Parsers
{
    public interface IParser
    {
        Page ParsePage(string filePath);

        Component ParseComponent(string filePath);
    }
}
