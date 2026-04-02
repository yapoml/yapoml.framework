using System.Collections.Generic;

namespace Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;

/// <summary>
/// Represents a parsed component definition from a YAML file.
/// </summary>
public class Component
{
    /// <summary>
    /// Gets or sets the name of the component.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the locator strategy for the component.
    /// </summary>
    public By By { get; set; }

    /// <summary>
    /// Gets or sets the name of the base component this component inherits from.
    /// </summary>
    public string BaseComponent { get; set; }

    /// <summary>
    /// Gets or sets the nested components within this component.
    /// </summary>
    public IList<Component> Components { get; set; }
}
