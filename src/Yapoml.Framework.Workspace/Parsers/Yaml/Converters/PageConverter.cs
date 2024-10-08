﻿using System;
using System.Collections.Generic;
using YamlDotNet.Core;
using YamlDotNet.Core.Events;
using YamlDotNet.Serialization;
using Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;

namespace Yapoml.Framework.Workspace.Parsers.Yaml.Converters
{
    class PageConverter : IYamlTypeConverter
    {
        private readonly ComponentConverter _componentConverter;

        private readonly UrlConverter _urlConverter;

        public PageConverter(ComponentConverter componentConverter, UrlConverter urlConverter)
        {
            _componentConverter = componentConverter;
            _urlConverter = urlConverter;
        }

        public bool Accepts(Type type)
        {
            return typeof(Page) == type;
        }

        public object ReadYaml(IParser parser, Type type, ObjectDeserializer rootDeserializer)
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
                    else if (scalar.Value.ToLower() == "url")
                    {
                        page.Url = (Url)_urlConverter.ReadYaml(parser, typeof(Url), rootDeserializer);
                    }
                    else
                    {
                        var componentName = scalar.Value;

                        var component = (Component)_componentConverter.ReadYaml(parser, typeof(Component), rootDeserializer);

                        component.Name = componentName;

                        if (page.Components == null) page.Components = new List<Component>();
                        page.Components.Add(component);
                    }
                }
                else
                {
                    parser.SkipThisAndNestedEvents();
                }
            }

            return page;
        }

        public void WriteYaml(IEmitter emitter, object value, Type type, ObjectSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
