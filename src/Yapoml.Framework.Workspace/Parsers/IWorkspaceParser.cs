using System.Collections.Generic;

namespace Yapoml.Framework.Workspace.Parsers;

/// <summary>
/// Defines the contract for parsing workspace file content into page and component models.
/// </summary>
public interface IWorkspaceParser
{
    /// <summary>
    /// Parses the specified content into a list of page models.
    /// </summary>
    /// <param name="content">The file content to parse.</param>
    /// <returns>A list of parsed page models.</returns>
    IList<Yaml.Pocos.Page> ParsePages(string content);

    /// <summary>
    /// Parses the specified content into a component model.
    /// </summary>
    /// <param name="content">The file content to parse.</param>
    /// <returns>The parsed component model.</returns>
    Yaml.Pocos.Component ParseComponent(string content);
}
