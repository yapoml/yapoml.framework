﻿using System.IO;
using System.Reflection;

namespace Yapoml.Selenium
{
    internal class TemplateReader
    {
        private Assembly assembly;
        public TemplateReader()
        {
            assembly = typeof(TemplateReader).Assembly;
        }

        public string Read(string templateName)
        {
            using (Stream stream = assembly.GetManifestResourceStream(assembly.GetName().Name + ".Templates." + templateName + ".scriban"))
            using (StreamReader reader = new StreamReader(stream))
            {
                return reader.ReadToEnd();
            }
        }
    }
}
