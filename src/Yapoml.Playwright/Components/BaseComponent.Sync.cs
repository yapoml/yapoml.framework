namespace Yapoml.Playwright.Components
{
    public partial class BaseComponent
    {
        /// <inheritdoc cref="Microsoft.Playwright.IElementHandle.TextContentAsync"/>
        public string TextContent() => TextContentAsync().GetAwaiter().GetResult();
    }
}