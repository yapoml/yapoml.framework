using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Yapoml.Workspace.Parsers.Yaml.Pocos;

namespace Yapoml.Workspace.Parsers.Yaml.Converters
{
    class ComponentConverter : IYamlTypeConverter
    {
        private ByConverter _byConverter = new ByConverter();

        public bool Accepts(Type type)
        {
            return typeof(Pocos.Component) == type;
        }

        public object ReadYaml(IParser parser, Type type)
        {
            var component = new Pocos.Component();

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

                            var innerComponent = (Pocos.Component)ReadYaml(parser, typeof(Pocos.Component));
                            innerComponent.Name = componentName;

                            if (component.Components == null) component.Components = new List<Pocos.Component>();
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
