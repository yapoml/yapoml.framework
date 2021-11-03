using System;
using System.Collections.Generic;
using System.IO;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;
using YamlDotNet.Serialization.NodeDeserializers;

namespace YamlTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var deserializer = new DeserializerBuilder()
                .WithTypeConverter(new ByConverter())
                .WithNamingConvention(LowerCaseNamingConvention.Instance)
                .Build();

            var page = deserializer.Deserialize<Page>(File.ReadAllText("Test.yapc.yaml"));

            Console.WriteLine(page.By.XPath);
        }
    }

    public class Page
    {
        public string Name { get; set; }

        public By By { get; set; }

        public Dictionary<string, Page> Ya { get; set; }
    }

    public class By
    {
        public string XPath { get; set; }

        public string Css { get; set; }
    }

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
                return new By { XPath = scalar.Value };
            }
            else
            {
                parser.Consume<MappingStart>();

                var by = new By();

                while (!parser.TryConsume<MappingEnd>(out _))
                {
                    var propertyName = parser.Consume<Scalar>().Value;
                    var propertyValue = parser.Consume<Scalar>().Value;

                    switch (propertyName.ToLower())
                    {
                        case "xpath":
                            by.XPath = propertyValue;
                            break;
                        case "css":
                            by.Css = propertyValue;
                            break;
                        default:
                            throw new Exception($"Unrecognized property '{propertyName}' of {type.Name} type.");
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
