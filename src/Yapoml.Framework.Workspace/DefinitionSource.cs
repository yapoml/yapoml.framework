using Yapoml.Framework.Workspace.Parsers;

namespace Yapoml.Framework.Workspace;

/// <summary>
/// Identifies the source file and region where a workspace element is defined.
/// </summary>
public class DefinitionSource
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DefinitionSource"/> class.
    /// </summary>
    /// <param name="relativeFilePath">The file path relative to the workspace root.</param>
    /// <param name="region">The text region within the file.</param>
    public DefinitionSource(string relativeFilePath, Region region)
    {
        RelativeFilePath = relativeFilePath;
        Region = region;
    }

    /// <summary>
    /// Gets the file path relative to the workspace root.
    /// </summary>
    public string RelativeFilePath { get; }

    /// <summary>
    /// Gets the text region within the file where the element is defined.
    /// </summary>
    public Region Region { get; }
}
