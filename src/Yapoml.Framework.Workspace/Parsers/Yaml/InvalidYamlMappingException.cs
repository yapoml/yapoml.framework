using System;

namespace Yapoml.Framework.Workspace.Parsers.Yaml;

/// <summary>
/// The exception that is thrown when a YAML scalar cannot be mapped to a known property.
/// </summary>
public class InvalidYamlMappingException : FormatException
{
    public InvalidYamlMappingException(string propertyName, Type targetType)
        : base($"Cannot map '{propertyName}' yaml scalar to any property of {targetType.Name} type.")
    {
        PropertyName = propertyName;
        TargetType = targetType;
    }

    public string PropertyName { get; }
    public Type TargetType { get; }
}
