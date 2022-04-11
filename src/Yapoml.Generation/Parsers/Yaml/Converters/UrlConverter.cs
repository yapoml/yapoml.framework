using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Yapoml.Generation.Parsers.Yaml.Pocos;

namespace Yapoml.Generation.Parsers.Yaml.Converters
{
    class UrlConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return typeof(Url) == type;
        }

        public object ReadYaml(YamlDotNet.Core.IParser parser, Type type)
        {
            Url url = null;

            if (parser.TryConsume<Scalar>(out var scalar))
            {
                url = new Url { Path = scalar.Value };
            }
            else
            {
                parser.TryConsume<MappingStart>(out _);

                while (!parser.TryConsume<MappingEnd>(out _))
                {
                    if (parser.TryConsume<Scalar>(out var urlProperty))
                    {
                        if (urlProperty.Value.Equals("path", StringComparison.OrdinalIgnoreCase))
                        {
                            var pathValue = parser.Consume<Scalar>().Value;

                            url = new Url { Path = pathValue };
                        }
                        else if (urlProperty.Value.Equals("params", StringComparison.OrdinalIgnoreCase) || urlProperty.Value.Equals("query", StringComparison.OrdinalIgnoreCase))
                        {
                            var queryParams = new List<string>();

                            if (parser.TryConsume<SequenceStart>(out _))
                            {
                                while (!parser.TryConsume<SequenceEnd>(out _))
                                {
                                    var queryParam = parser.Consume<Scalar>().Value;

                                    queryParams.Add(queryParam);
                                }
                            }
                            else
                            {
                                parser.SkipThisAndNestedEvents();
                            }

                            url.Params = queryParams;
                        }
                        else
                        {
                            parser.SkipThisAndNestedEvents();
                        }
                    }


                    //parser.SkipThisAndNestedEvents();
                }
            }

            return url;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            throw new NotImplementedException();
        }
    }
}
