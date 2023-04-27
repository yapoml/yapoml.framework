using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;

namespace Yapoml.Framework.Workspace.Parsers.Yaml.Converters
{
    class ComponentConverter : IYamlTypeConverter
    {
        private readonly ByConverter _byConverter;

        public ComponentConverter(ByConverter byConverter)
        {
            _byConverter = byConverter;
        }

        public bool Accepts(Type type)
        {
            return typeof(Component) == type;
        }

        public object ReadYaml(IParser parser, Type type)
        {
            var component = new Component();

            if (parser.TryConsume<Scalar>(out var byScalar))
            {
                component.By = _byConverter.ParseScalar(byScalar);
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
                        else if (scalar.Value.ToLower() == "base" || scalar.Value.ToLower() == "extends" || scalar.Value.Equals("ref", StringComparison.OrdinalIgnoreCase))
                        {
                            component.BaseComponent = parser.Consume<Scalar>().Value;
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

            return component;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            throw new NotImplementedException();
        }
    }
}
