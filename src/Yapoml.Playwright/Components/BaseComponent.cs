using Microsoft.Playwright;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using Yapoml.Options;
using Yapoml.Playwright.Events;

namespace Yapoml.Playwright.Components
{
    /// <inheritdoc/>
    public partial class BaseComponent : IElementHandle
    {
        protected IPage Page { get; private set; }

        public IElementHandle WrappedElementHandle { get; private set; }

        protected ISpaceOptions SpaceOptions { get; private set; }

        protected IComponentEventSource EventSource { get; private set; }

        public BaseComponent(IPage page, IElementHandle elementHandle, ISpaceOptions spaceOptions)
        {
            Page = page;
            WrappedElementHandle = elementHandle;
            SpaceOptions = spaceOptions;

            EventSource = spaceOptions.Get<IEventSource>().ComponentEventSource;
        }

        public Task<ElementHandleBoundingBoxResult> BoundingBoxAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task CheckAsync(ElementHandleCheckOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task ClickAsync(ElementHandleClickOptions options = null)
        {
            return WrappedElementHandle.ClickAsync(options);
        }

        public Task<IFrame> ContentFrameAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task DblClickAsync(ElementHandleDblClickOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task DispatchEventAsync(string type, object eventInit = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> EvalOnSelectorAsync<T>(string selector, string expression, object arg = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> EvalOnSelectorAllAsync<T>(string selector, string expression, object arg = null)
        {
            throw new System.NotImplementedException();
        }

        public Task FillAsync(string value, ElementHandleFillOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task FocusAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<string> GetAttributeAsync(string name)
        {
            throw new System.NotImplementedException();
        }

        public Task HoverAsync(ElementHandleHoverOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> InnerHTMLAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<string> InnerTextAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<string> InputValueAsync(ElementHandleInputValueOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsCheckedAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsDisabledAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsEditableAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsEnabledAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsHiddenAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsVisibleAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<IFrame> OwnerFrameAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task PressAsync(string key, ElementHandlePressOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<IElementHandle> QuerySelectorAsync(string selector)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<IElementHandle>> QuerySelectorAllAsync(string selector)
        {
            throw new System.NotImplementedException();
        }

        public Task<byte[]> ScreenshotAsync(ElementHandleScreenshotOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task ScrollIntoViewIfNeededAsync(ElementHandleScrollIntoViewIfNeededOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(string values, ElementHandleSelectOptionOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(IElementHandle values, ElementHandleSelectOptionOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(IEnumerable<string> values, ElementHandleSelectOptionOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(SelectOptionValue values, ElementHandleSelectOptionOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(IEnumerable<IElementHandle> values, ElementHandleSelectOptionOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<IReadOnlyList<string>> SelectOptionAsync(IEnumerable<SelectOptionValue> values, ElementHandleSelectOptionOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task SelectTextAsync(ElementHandleSelectTextOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task SetCheckedAsync(bool checkedState, ElementHandleSetCheckedOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task SetInputFilesAsync(string files, ElementHandleSetInputFilesOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task SetInputFilesAsync(IEnumerable<string> files, ElementHandleSetInputFilesOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task SetInputFilesAsync(FilePayload files, ElementHandleSetInputFilesOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task SetInputFilesAsync(IEnumerable<FilePayload> files, ElementHandleSetInputFilesOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task TapAsync(ElementHandleTapOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<string> TextContentAsync()
        {
            return WrappedElementHandle.TextContentAsync();
        }

        public Task TypeAsync(string text, ElementHandleTypeOptions options = null)
        {
            return WrappedElementHandle.TypeAsync(text, options);
        }

        public Task UncheckAsync(ElementHandleUncheckOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task WaitForElementStateAsync(ElementState state, ElementHandleWaitForElementStateOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<IElementHandle> WaitForSelectorAsync(string selector, ElementHandleWaitForSelectorOptions options = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<JsonElement?> EvalOnSelectorAsync(string selector, string expression, object arg = null)
        {
            throw new System.NotImplementedException();
        }

        public IElementHandle AsElement()
        {
            throw new System.NotImplementedException();
        }

        public Task<T> EvaluateAsync<T>(string expression, object arg = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<IJSHandle> EvaluateHandleAsync(string expression, object arg = null)
        {
            throw new System.NotImplementedException();
        }

        public Task<Dictionary<string, IJSHandle>> GetPropertiesAsync()
        {
            throw new System.NotImplementedException();
        }

        public Task<IJSHandle> GetPropertyAsync(string propertyName)
        {
            throw new System.NotImplementedException();
        }

        public Task<T> JsonValueAsync<T>()
        {
            throw new System.NotImplementedException();
        }

        public Task<JsonElement?> EvaluateAsync(string expression, object arg = null)
        {
            throw new System.NotImplementedException();
        }

        public ValueTask DisposeAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}