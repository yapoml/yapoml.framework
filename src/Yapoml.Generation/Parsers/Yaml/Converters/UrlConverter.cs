using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Yapoml.Generation.Parsers.Yaml.Pocos;
using static Yapoml.Generation.Parsers.Yaml.Pocos.Url;

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
                        else if (urlProperty.Value.Equals("query", StringComparison.OrdinalIgnoreCase))
                        {
                            var queryParams = new List<QueryParam>();

                            if (parser.TryConsume<SequenceStart>(out _))
                            {
                                while (!parser.TryConsume<SequenceEnd>(out _))
                                {
                                    var queryParam = new QueryParam();

                                    if (parser.TryConsume<Scalar>(out var queryParamScalar))
                                    {
                                        queryParam.Name = queryParamScalar.Value;
                                    }
                                    else if (parser.TryConsume<MappingStart>(out _))
                                    {
                                        while (parser.TryConsume<Scalar>(out var queryParamMap))
                                        {
                                            if (queryParamMap.Value.Equals("name", StringComparison.OrdinalIgnoreCase))
                                            {
                                                var nameValue = parser.Consume<Scalar>().Value;

                                                queryParam.Name = nameValue;
                                            }
                                            else if (queryParamMap.Value.Equals("optional", StringComparison.OrdinalIgnoreCase))
                                            {
                                                var isOptional = new Deserializer().Deserialize<bool>(parser);

                                                queryParam.IsOptional = isOptional;
                                            }
                                            else if (queryParamMap.Value.Equals("required", StringComparison.OrdinalIgnoreCase))
                                            {
                                                var isRequired = new Deserializer().Deserialize<bool>(parser);

                                                queryParam.IsOptional = !isRequired;
                                            }
                                            else
                                            {
                                                parser.SkipThisAndNestedEvents();
                                            }
                                        }

                                        parser.Consume<MappingEnd>();
                                    }

                                    queryParams.Add(queryParam);
                                }
                            }

                            url.QueryParams = queryParams;
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
