namespace Yapoml.Framework.Workspace.Parsers.Yaml.Pocos;

/// <summary>
/// Represents a parsed element locator definition from a YAML file.
/// </summary>
public class By
{
    /// <summary>
    /// Gets or sets the locator method.
    /// </summary>
    public ByMethod Method { get; set; }

    /// <summary>
    /// Gets or sets the locator value.
    /// </summary>
    public string Value { get; set; }

    /// <summary>
    /// Specifies the method used to locate an element.
    /// </summary>
    public enum ByMethod
    {
        /// <summary>
        /// No locator method specified.
        /// </summary>
        None,

        /// <summary>
        /// Locate by XPath expression.
        /// </summary>
        XPath,

        /// <summary>
        /// Locate by CSS selector.
        /// </summary>
        Css,

        /// <summary>
        /// Locate by element ID.
        /// </summary>
        Id,

        /// <summary>
        /// Locate by test ID attribute.
        /// </summary>
        TestId
    }

    /// <summary>
    /// Gets or sets the scope in which to search for the element.
    /// </summary>
    public ByScope Scope { get; set; }

    /// <summary>
    /// Specifies the scope for element lookup.
    /// </summary>
    public enum ByScope
    {
        /// <summary>
        /// Search within the parent element.
        /// </summary>
        Parent,

        /// <summary>
        /// Search from the root of the document.
        /// </summary>
        Root
    }

    /// <summary>
    /// Gets or sets the source file region where this locator is defined.
    /// </summary>
    public Region Region { get; set; }
}
