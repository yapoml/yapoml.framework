using System;

namespace Yapoml.Framework.Workspace.Services;

/// <summary>
/// The exception that is thrown when a workspace reference (base page or component) cannot be resolved.
/// </summary>
public class ReferenceResolutionException : InvalidOperationException
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ReferenceResolutionException"/> class with a specified error message.
    /// </summary>
    /// <param name="message">The message that describes the error.</param>
    public ReferenceResolutionException(string message) : base(message)
    {
    }
}
