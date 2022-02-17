using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Yapoml.Generation.Parsers.Yaml.Converters;

namespace Yapoml.Generation.Parsers.Yaml
{
    public class YamlParser
    {
        public T Parse<T>(string content)
        {
            var deserializer = new DeserializerBuilder()
                .WithTypeConverter(new ByConverter())
                .WithNamingConvention(LowerCaseNamingConvention.Instance)
                .Build();

            return deserializer.Deserialize<T>(content);
        }
    }
}
