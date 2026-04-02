using System;

namespace Yapoml.Framework.Workspace.Services;

/// <summary>
/// The exception that is thrown when a workspace reference (base page or component) cannot be resolved.
/// </summary>
public class ReferenceResolutionException : InvalidOperationException
{
    public ReferenceResolutionException(string message) : base(message)
    {
    }
}
