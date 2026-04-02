using System;

namespace Yapoml.Framework.Workspace.Parsers.Yaml;

/// <summary>
/// The exception that is thrown when a YAML scalar cannot be mapped to a known property.
/// </summary>
public class InvalidYamlMappingException : FormatException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="InvalidYamlMappingException"/> class.
    /// </summary>
    /// <param name="propertyName">The YAML property name that could not be mapped.</param>
    /// <param name="targetType">The target type that the property was being mapped to.</param>
    public InvalidYamlMappingException(string propertyName, Type targetType)
        : base($"Cannot map '{propertyName}' yaml scalar to any property of {targetType.Name} type.")
    {
        PropertyName = propertyName;
        TargetType = targetType;
    }

    /// <summary>
    /// Gets the YAML property name that could not be mapped.
    /// </summary>
    public string PropertyName { get; }

    /// <summary>
    /// Gets the target type that the property was being mapped to.
    /// </summary>
    public Type TargetType { get; }
}
