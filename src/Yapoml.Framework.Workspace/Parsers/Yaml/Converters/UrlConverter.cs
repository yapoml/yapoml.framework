using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;

namespace Yapoml.Framework.Workspace.Parsers.Yaml.Converters
{
    class UrlConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return typeof(Url) == type;
        }

        public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
        {
            Url url = null;

            if (parser.TryConsume<Scalar>(out var scalar))
            {
                url = new Url { Path = scalar.Value };
            }
            else if (parser.TryConsume<MappingStart>(out _))
            {
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
                    else
                    {
                        parser.SkipThisAndNestedEvents();
                    }
                }
            }
            else
            {
                parser.SkipThisAndNestedEvents();
            }

            return url;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type, ObjectSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
