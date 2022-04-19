using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Yapoml.Workspace.Parsers.Yaml.Pocos;

namespace Yapoml.Workspace.Parsers.Yaml.Converters
{
    class PageConverter : IYamlTypeConverter
    {
        private ComponentConverter _componentConverter = new ComponentConverter();

        private UrlConverter _urlConverter = new UrlConverter();

        public bool Accepts(Type type)
        {
            return typeof(Pocos.Page) == type;
        }

        public object ReadYaml(IParser parser, Type type)
        {
            var page = new Pocos.Page();

            parser.Consume<MappingStart>();

            while (!parser.TryConsume<MappingEnd>(out _))
            {
                if (parser.TryConsume<Scalar>(out var scalar))
                {
                    if (scalar.Value.ToLower() == "base" || scalar.Value.ToLower() == "extends")
                    {
                        page.BasePage = parser.Consume<Scalar>().Value;
                    }
                    else if (scalar.Value.ToLower() == "url")
                    {
                        page.Url = (Url)_urlConverter.ReadYaml(parser, typeof(Url));
                    }
                    else
                    {
                        var componentName = scalar.Value;

                        var component = (Pocos.Component)_componentConverter.ReadYaml(parser, typeof(Pocos.Component));

                        component.Name = componentName;

                        if (page.Components == null) page.Components = new List<Pocos.Component>();
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
