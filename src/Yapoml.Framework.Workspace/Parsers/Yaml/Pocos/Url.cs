using System.Collections.Generic;

namespace Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;

/// <summary>
/// Represents a parsed URL definition from a YAML file.
/// </summary>
public class Url
{
    /// <summary>
    /// Gets or sets the URL path template.
    /// </summary>
    public string Path { get; set; }

    /// <summary>
    /// Gets or sets the list of query parameter names.
    /// </summary>
    public IList<string> Params { get; set; }
}
