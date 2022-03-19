using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Yapoml.Generation.Parsers.Yaml.Pocos;

namespace Yapoml.Generation.Parsers.Yaml.Converters
{
    class PageConverter : IYamlTypeConverter
    {
        private ComponentConverter _componentConverter = new ComponentConverter();

        public bool Accepts(Type type)
        {
            return typeof(Page) == type;
        }

        public object ReadYaml(YamlDotNet.Core.IParser parser, Type type)
        {
            var page = new Page();

            parser.Consume<MappingStart>();

            while (!parser.TryConsume<MappingEnd>(out _))
            {
                if (parser.TryConsume<Scalar>(out var scalar))
                {
                    if (scalar.Value.ToLower() == "base" || scalar.Value.ToLower() == "extends")
                    {
                        page.BasePage = parser.Consume<Scalar>().Value;
                    }
                    else
                    {

                        var componentName = scalar.Value;

                        var component = (Component)_componentConverter.ReadYaml(parser, typeof(Component));

                        component.Name = componentName;

                        if (page.Components == null) page.Components = new List<Component>();
                        page.Components.Add(component);
                    }
                }
            }

            return page;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type)
        {
            throw new NotImplementedException();
        }
    }
}
