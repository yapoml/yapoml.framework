using System;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;

namespace Yapoml.Framework.Workspace.Parsers.Yaml.Converters
{
    class ByConverter : IYamlTypeConverter
    {
        public bool Accepts(Type type)
        {
            return typeof(By) == type;
        }

        public object ReadYaml(IParser parser, Type type)
        {
            if (parser.TryConsume<Scalar>(out var scalar))
            {
                return ParseScalar(scalar);
            }
            else
            {
                parser.Consume<MappingStart>();

                var by = new By();

                while (!parser.TryConsume<MappingEnd>(out _))
                {
                    var propertyName = parser.Consume<Scalar>().Value;

                    var propertyScalar = parser.Consume<Scalar>();
                    by = ParseScalar(propertyScalar);
                    var propertyValue = propertyScalar.Value;

                    switch (propertyName.ToLowerInvariant())
                    {
                        case "css":
                            by.Method = By.ByMethod.Css;
                            by.Value = propertyValue;
                            break;
                        case "xpath":
                            by.Method = By.ByMethod.XPath;
                            by.Value = propertyValue;
                            break;
                        case "id":
                            by.Method = By.ByMethod.Id;
                            by.Value = propertyValue;
                            break;
                        case "testid":
                            by.Method = By.ByMethod.TestId;
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

        public By ParseScalar(Scalar scalar)
        {
            var value = scalar.Value;

            var region = new Region(
                new Region.Position((uint)scalar.Start.Line, (uint)scalar.Start.Column),
                new Region.Position((uint)scalar.End.Line, (uint)scalar.End.Column - 1)
            );

            if (value.StartsWith("by ", StringComparison.OrdinalIgnoreCase))
            {
                value = value.Substring(3);
            }

            if (value.StartsWith("xpath ", StringComparison.OrdinalIgnoreCase))
            {
                return new By { Method = By.ByMethod.XPath, Value = value.Substring(6, value.Length - 6), Region = region };
            }
            else if (value.StartsWith("css ", StringComparison.OrdinalIgnoreCase))
            {
                return new By { Method = By.ByMethod.Css, Value = value.Substring(4, value.Length - 4), Region = region };
            }
            else if (value.StartsWith("id ", StringComparison.OrdinalIgnoreCase))
            {
                return new By { Method = By.ByMethod.Id, Value = value.Substring(3, value.Length - 3), Region = region };
            }
            else
            {
                return new By { Method = By.ByMethod.None, Value = value, Region = region };
            }
        }
    }
}
