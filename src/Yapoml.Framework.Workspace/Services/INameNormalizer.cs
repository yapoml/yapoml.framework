namespace Yapoml.Framework.Workspace.Services;

/// <summary>
/// Defines the contract for normalizing raw names into valid identifiers.
/// </summary>
public interface INameNormalizer
{
    /// <summary>
    /// Normalizes the specified name into a valid identifier.
    /// </summary>
    /// <param name="name">The raw name to normalize.</param>
    /// <returns>The normalized name.</returns>
    string Normalize(string name);
}
