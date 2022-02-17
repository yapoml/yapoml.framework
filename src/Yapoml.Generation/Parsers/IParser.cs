using Yapoml.Generation.Parsers.Yaml.Pocos;

namespace Yapoml.Generation.Parsers
{
    public interface IParser
    {
        Page ParsePage(string filePath);

        Component ParseComponent(string filePath);
    }
}
