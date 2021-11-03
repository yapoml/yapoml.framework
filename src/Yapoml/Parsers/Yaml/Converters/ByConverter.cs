﻿using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Yapoml.Parsers.Yaml.Pocos;
using static Yapoml.Parsers.Yaml.Pocos.By;

namespace Yapoml.Parsers.Yaml.Converters
{
    public class ByConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return typeof(By) == type;
        }
        public object ReadYaml(IParser parser, Type type)
        {
            if (parser.TryConsume<Scalar>(out var scalar))
            {
                return new By { Method = ByMethod.XPath, Value = scalar.Value };
            }
            else
            {
                parser.Consume<MappingStart>();

                var by = new By();

                while (!parser.TryConsume<MappingEnd>(out _))
                {
                    var propertyName = parser.Consume<Scalar>().Value;
                    var propertyValue = parser.Consume<Scalar>().Value;

                    switch (propertyName.ToLowerInvariant())
                    {
                        case "xpath":
                            by.Method = ByMethod.XPath;
                            by.Value = propertyValue;
                            break;
                        case "css":
                            by.Method = ByMethod.Css;
                            by.Value = propertyValue;
                            break;
                        default:
                            throw new Exception($"Cannot map '{propertyName}' yaml scalar to property of {type.Name} type.");
                    }
                }

                return by;
            }
        }
        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            throw new NotImplementedException();
        }
    }
}
