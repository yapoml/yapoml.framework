namespace Yapoml.Playwright.Components
{
    /// <inheritdoc/>
    public partial class BaseComponent
    {
        public string TextContent() => TextContentAsync().GetAwaiter().GetResult();
    }
}