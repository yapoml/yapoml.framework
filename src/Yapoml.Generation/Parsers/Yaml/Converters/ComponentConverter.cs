using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Yapoml.Generation.Parsers.Yaml.Pocos;

namespace Yapoml.Generation.Parsers.Yaml.Converters
{
    class ComponentConverter : IYamlTypeConverter
    {
        private ByConverter _byConverter = new ByConverter();

        public bool Accepts(Type type)
        {
            return typeof(Component) == type;
        }

        public object ReadYaml(YamlDotNet.Core.IParser parser, Type type)
        {
            var component = new Component();

            if (parser.TryConsume<Scalar>(out var byScalar))
            {
                component.By = _byConverter.ParseScalarValue(byScalar.Value);
            }
            else if (parser.TryConsume<MappingStart>(out _))
            {
                while (!parser.TryConsume<MappingEnd>(out _))
                {
                    if (parser.TryConsume<Scalar>(out var scalar))
                    {
                        if (scalar.Value.Equals("by", StringComparison.OrdinalIgnoreCase))
                        {
                            var by = (By)_byConverter.ReadYaml(parser, typeof(By));

                            component.By = by;
                        }
                        else
                        {
                            var componentName = scalar.Value;

                            var innerComponent = (Component)ReadYaml(parser, typeof(Component));
                            innerComponent.Name = componentName;

                            if (component.Components == null) component.Components = new List<Component>();
                            component.Components.Add(innerComponent);
                        }
                    }
                }
            }

            return component;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            throw new NotImplementedException();
        }
    }
}
