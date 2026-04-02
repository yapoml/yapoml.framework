using System.Collections.Generic;

namespace Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;

/// <summary>
/// Represents a parsed page definition from a YAML file.
/// </summary>
public class Page
{
    /// <summary>
    /// Gets or sets the name of the page.
    /// </summary>
    public string Name { get; set; }

    /// <summary>
    /// Gets or sets the components defined within this page.
    /// </summary>
    public IList<Component> Components { get; set; }

    /// <summary>
    /// Gets or sets the name of the base page this page inherits from.
    /// </summary>
    public string BasePage { get; set; }

    /// <summary>
    /// Gets or sets the URL definition for this page.
    /// </summary>
    public Url Url { get; set; }
}
