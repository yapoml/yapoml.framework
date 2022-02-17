using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Yapoml.Generation.Parsers.Yaml.Pocos;

namespace Yapoml.Generation.Parsers.Yaml.Converters
{
    class ByConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return typeof(By) == type;
        }

        public object ReadYaml(YamlDotNet.Core.IParser parser, Type type)
        {
            if (parser.TryConsume<Scalar>(out var scalar))
            {
                var value = scalar.Value;

                if (value.StartsWith("xpath "))
                {
                    return new By { Method = By.ByMethod.XPath, Value = value.Substring(6, value.Length - 6) };
                }
                else if (value.StartsWith("css "))
                {
                    return new By { Method = By.ByMethod.Css, Value = value.Substring(4, value.Length - 4) };
                }
                else if (value.StartsWith("id "))
                {
                    return new By { Method = By.ByMethod.Id, Value = value.Substring(3, value.Length - 3) };
                }
                else
                {
                    return new By { Method = By.ByMethod.XPath, Value = value };
                }


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
                            by.Method = By.ByMethod.XPath;
                            by.Value = propertyValue;
                            break;
                        case "css":
                            by.Method = By.ByMethod.Css;
                            by.Value = propertyValue;
                            break;
                        case "id":
                            by.Method = By.ByMethod.Id;
                            by.Value = propertyValue;
                            break;
                        default:
                            throw new Exception($"Cannot map '{propertyName}' yaml scalar to any property of {type.Name} type.");
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
