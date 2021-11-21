using System;
using System.Collections.Generic;
using System.Text;
using Yapoml.Parsers.Yaml.Pocos;

namespace Yapoml.Parsers
{
    public interface IComponentParser
    {
        Component Parse(string fileName);
    }
}
