using System;
using System.Collections.Generic;
using System.Text;

namespace Yapoml.Framework.Workspace.Services;

/// <summary>
/// Provides services for determining plurality and converting between singular and plural word forms.
/// </summary>
public interface IPluralizationService
{
    /// <summary>
    /// Determines whether the specified word is in plural form.
    /// </summary>
    /// <param name="word">The word to check.</param>
    /// <returns><see langword="true"/> if the word is plural; otherwise, <see langword="false"/>.</returns>
    bool IsPlural(string word);

    /// <summary>
    /// Converts the specified word to its singular form.
    /// </summary>
    /// <param name="word">The word to singularize.</param>
    /// <returns>The singular form of the word.</returns>
    string Singularize(string word);
}
