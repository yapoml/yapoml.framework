using Yapoml.Parsers.Yaml.Pocos;

namespace Yapoml.Parsers
{
    public interface IParser
    {
        Page ParsePage(string fileName);

        Component ParseComponent(string fileName);
    }
}
