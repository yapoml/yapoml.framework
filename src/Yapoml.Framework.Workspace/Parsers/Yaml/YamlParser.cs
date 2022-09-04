using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using Yapoml.Framework.Workspace.Parsers.Yaml.Converters;

namespace Yapoml.Framework.Workspace.Parsers.Yaml
{
    public class YamlParser
    {
        private readonly IDeserializer _deserializer;

        public YamlParser()
        {
            var urlConverter = new UrlConverter();
            var byConverter = new ByConverter();
            var componentConverter = new ComponentConverter(byConverter);
            var pageConverter = new PageConverter(componentConverter, urlConverter);

            _deserializer = new DeserializerBuilder()
                .WithTypeConverter(pageConverter)
                .WithTypeConverter(componentConverter)
                .WithTypeConverter(byConverter)
                .WithTypeConverter(urlConverter)
                .WithNamingConvention(LowerCaseNamingConvention.Instance)
                .Build();
        }

        public T Parse<T>(string content)
        {
            using (var stringReader = new StringReader(content))
            {
                return Parse<T>(GetParser(stringReader));
            }
        }

        public IList<T> ParseMany<T>(string content)
        {
            var results = new List<T>();

            using (var stringReader = new StringReader(content))
            {
                var parser = GetParser(stringReader);

                parser.Consume<YamlDotNet.Core.Events.StreamStart>();

                while (parser.TryConsume<YamlDotNet.Core.Events.DocumentStart>(out _))
                {
                    while (!parser.TryConsume<YamlDotNet.Core.Events.DocumentEnd>(out _))
                    {
                        var result = Parse<T>(parser);

                        results.Add(result);
                    }
                }

                parser.Consume<YamlDotNet.Core.Events.StreamEnd>();

                // resolve empty content as default T
                if (results.Count == 0) results.Add(Activator.CreateInstance<T>());
            }

            return results;
        }

        private IParser GetParser(TextReader textReader)
        {
            var scanner = new Scanner(textReader, skipComments: true);

            var parser = new Parser(scanner);

            return parser;
        }

        private T Parse<T>(IParser parser)
        {
            var result = _deserializer.Deserialize<T>(parser);

            if (result == null)
            {
                result = Activator.CreateInstance<T>();
            }

            return result;
        }
    }
}
